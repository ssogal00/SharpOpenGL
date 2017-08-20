
#include "fbxsdk.h"
#include "FBXSDKWrapper.h"
#include "BoneIterator.h"
#include "ParsedFBXAnimStack.h"
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;
using namespace OpenTK;
using namespace System::Collections::Generic;
using namespace System;
using namespace FBXWrapper;


bool FBXSDKWrapper::InitializeSDK()
{
	FBXManager = FbxManager::Create();

	if (!FBXManager)
	{
		return false;
	}

	FbxIOSettings* ios = FbxIOSettings::Create(FBXManager, IOSROOT);
	FBXManager->SetIOSettings(ios);
	
	FbxString lPath = FbxGetApplicationDirectory();
	FBXManager->LoadPluginsDirectory(lPath.Buffer());
		
	Scene = FbxScene::Create(FBXManager, "My Scene");
	if (!Scene)
	{
		FBXSDK_printf("Error: Unable to create FBX scene!\n");
		return false;
	}

	return true;
}

ParsedFBXMesh^ FBXSDKWrapper::ParseFbxMesh(FbxNode* Node)
{
	if (Node != nullptr)
	{
		for (int i = 0; i < Node->GetChildCount(); ++i)
		{
			FbxNode* ChildNode = Node->GetChild(i);

			if (ChildNode && ChildNode->GetNodeAttribute() && ChildNode->GetNodeAttribute()->GetAttributeType() == FbxNodeAttribute::eMesh)
			{
				return ParseFbxMesh((FbxMesh*)(ChildNode->GetNodeAttribute()), ChildNode);
			}
		}
	}

	for (int i = 0; i < Node->GetChildCount(); i++)
	{		
		return ParseFbxMesh(Node->GetChild(i));
	}

	return nullptr;
}

ParsedFBXMesh^ FBXSDKWrapper::ParseFbxMesh(FbxMesh* Mesh, FbxNode* Node)
{
	ParsedFBXMesh^ ResultMesh = gcnew ParsedFBXMesh();

	System::Console::WriteLine(gcnew System::String(Node->GetName()));
		
	ResultMesh->VertexList = ParseFbxMeshVertex(Mesh);
	ResultMesh->NormalList = ParseFbxMeshNormal(Mesh);
	ResultMesh->UVList = ParseFbxMeshUV(Mesh);
	ResultMesh->ControlPointList = ParseFbxControlPointList(Mesh);
	ResultMesh->IndexList = ParseFBXMeshIndex(Mesh);
	ResultMesh->BoneMap = ParseFbxMeshBone(Mesh);
	ResultMesh->RootBone= ParseBoneHierarchy(Scene->GetRootNode());

	for (BoneIterator It(ResultMesh->RootBone); !It.IsEnd(); It.MoveNext())
	{
		ParsedFBXMeshBone^ CurrentBone = It.Current();
		System::String^ BoneName = CurrentBone->BoneName;
		if (BoneName != nullptr)
		{
			CurrentBone->LinkTransform = ResultMesh->BoneMap[BoneName]->LinkTransform;
			CurrentBone->Transform = ResultMesh->BoneMap[BoneName]->Transform;
		}
	}

	for (BoneIterator It(ResultMesh->RootBone); !It.IsEnd(); It.MoveNext())
	{
		ParsedFBXMeshBone^ CurrentBone = ResultMesh->BoneMap[It.Current()->BoneName];

		for (int i = 0; i < CurrentBone->ControlPointIndexList->Count; ++i)
		{
			SkinningInfo^ NewSkinningInfo = gcnew SkinningInfo();
			NewSkinningInfo->nControlPointIndex = CurrentBone->ControlPointIndexList[i];
			NewSkinningInfo->Weight = CurrentBone->ControlPointWeightList[i];
			NewSkinningInfo->BoneName = CurrentBone->BoneName;
			NewSkinningInfo->OffsetMatrix = CurrentBone->Transform;

			if (ResultMesh->SkinningInfoMap->ContainsKey(NewSkinningInfo->nControlPointIndex))
			{
				ResultMesh->SkinningInfoMap[NewSkinningInfo->nControlPointIndex]->Add(NewSkinningInfo);				
			}
			else
			{
				List<SkinningInfo^>^ NewList = gcnew List<SkinningInfo^>();
				NewList->Add(NewSkinningInfo);
				ResultMesh->SkinningInfoMap->Add(NewSkinningInfo->nControlPointIndex, NewList);	
			}
		}
	}
	 
	return ResultMesh;
}

List<Vector3>^ FBXSDKWrapper::ParseFbxMeshVertex(FbxMesh* Mesh)
{
	const int nPolygonCount = Mesh->GetPolygonCount();
	const int nControlPointCount = Mesh->GetControlPointsCount();
			
	FbxVector4* pControlPoints = Mesh->GetControlPoints();	

	List<OpenTK::Vector3>^ ResultVertexList = gcnew List<OpenTK::Vector3>();
		
	for (int i = 0; i < nPolygonCount; ++i)
	{
		int nPolygonSize = Mesh->GetPolygonSize(i);

		for (int j = 0; j < nPolygonSize; ++j)
		{
			int nControlPointIndex = Mesh->GetPolygonVertex(i, j);
			ResultVertexList->Add(Parse3DVector(pControlPoints[nControlPointIndex]));			
		}
	}

	return ResultVertexList;
}

List<unsigned int>^ FBXSDKWrapper::ParseFBXMeshIndex(FbxMesh* Mesh)
{
	const int nPolygonCount = Mesh->GetPolygonCount();
	const int nControlPointCount = Mesh->GetControlPointsCount();

	List<unsigned int>^ IndexList = gcnew List<unsigned int>();

	for (int i = 0; i < nPolygonCount; ++i)
	{
		int nPolygonSize = Mesh->GetPolygonSize(i);

		for (int j = 0; j < nPolygonSize; ++j)
		{
			int nControlPointIndex = Mesh->GetPolygonVertex(i, j);
			IndexList->Add(static_cast<unsigned int>(nControlPointIndex));
		}
	}

	return IndexList;
}

List<OpenTK::Vector3>^ FBXSDKWrapper::ParseFbxMeshNormal(FbxMesh* Mesh)
{	
	int nPolygonCount = Mesh->GetPolygonCount();	
	
	List<OpenTK::Vector3>^ NormalList = gcnew List<OpenTK::Vector3>();
	
	for (int i = 0; i < nPolygonCount; i++)
	{
		const int nPolygonSize = Mesh->GetPolygonSize(i);

		for (int j = 0; j < nPolygonSize; ++j)
		{
			FbxVector4 NormalVector;
			Mesh->GetPolygonVertexNormal(i, j, NormalVector);			
			NormalList->Add(Parse3DVector(NormalVector));			
		}
	}
	return NormalList;
}

List<Vector2>^ FBXSDKWrapper::ParseFbxMeshUV(FbxMesh* Mesh)
{
	int nPolygonCount = Mesh->GetPolygonCount();

	List<OpenTK::Vector2>^ UVList = gcnew List<OpenTK::Vector2>();
		
	for (int i = 0; i < nPolygonCount; i++)
	{
		const int nPolygonSize = Mesh->GetPolygonSize(i);

		for (int j = 0; j < nPolygonSize; ++j)
		{
			const int ControlPointIndex = Mesh->GetPolygonVertex(i, j);
			const int TextureUVIndex = Mesh->GetTextureUVIndex(i, j);
			
			for (int k = 0; k < Mesh->GetElementUVCount(); ++k)
			{
				FbxGeometryElementUV* pUV = Mesh->GetElementUV(k);

				if (pUV->GetMappingMode() == FbxGeometryElement::eByControlPoint)
				{
					if (pUV->GetReferenceMode() == FbxGeometryElement::eDirect)
					{
						UVList->Add(Parse2DVector(pUV->GetDirectArray().GetAt(ControlPointIndex)));
					}
					else if (pUV->GetReferenceMode() == FbxGeometryElement::eIndexToDirect)
					{
						int id = pUV->GetIndexArray().GetAt(ControlPointIndex);
						UVList->Add(Parse2DVector(pUV->GetDirectArray().GetAt(id)));
					}
				}
				else if (pUV->GetMappingMode() == FbxLayerElement::eByPolygonVertex)
				{
					if (pUV->GetReferenceMode() == FbxLayerElement::eDirect || pUV->GetReferenceMode() == FbxLayerElement::eIndexToDirect)
					{
						UVList->Add(Parse2DVector(pUV->GetDirectArray().GetAt(TextureUVIndex)));
					}
				}				
			}			
		}
	}

	return UVList;
}

List<Vector3>^ FBXSDKWrapper::ParseFbxControlPointList(FbxMesh* Mesh)
{
	List<OpenTK::Vector3>^ ResultControlPointList = gcnew List<OpenTK::Vector3>();

	const int nControlPointCount = Mesh->GetControlPointsCount();

	for (int i = 0; i < nControlPointCount; ++i)
	{
		ResultControlPointList->Add(Parse3DVector(Mesh->GetControlPointAt(i)));
	}

	return ResultControlPointList;
}

Dictionary<String^ ,ParsedFBXMeshBone^>^ FBXSDKWrapper::ParseFbxMeshBone(FbxMesh* Mesh)
{
	Dictionary<System::String^, FBXWrapper::ParsedFBXMeshBone^>^ ResultBoneList = gcnew Dictionary<System::String^, FBXWrapper::ParsedFBXMeshBone^>();

	int SkinCount = Mesh->GetDeformerCount(FbxDeformer::eSkin);

	for (int i = 0; i < SkinCount; ++i)
	{
		FbxSkin* pSkin = ((FbxSkin*)Mesh->GetDeformer(i, FbxDeformer::eSkin));

		int ClusterCount = pSkin->GetClusterCount();

		for (int j = 0; j < ClusterCount; ++j)
		{
			ParsedFBXMeshBone^ NewBone = gcnew ParsedFBXMeshBone();

			FbxCluster* pCluster = pSkin->GetCluster(j);

			if (pCluster->GetLink() != nullptr)
			{
				FbxAMatrix LinkMatrix;
				FbxAMatrix TransfromMatrix;
				LinkMatrix = pCluster->GetTransformLinkMatrix(LinkMatrix);
				TransfromMatrix = pCluster->GetTransformMatrix(TransfromMatrix);		
				const int nControlPointCount = pCluster->GetControlPointIndicesCount();
				int* pIndices = pCluster->GetControlPointIndices();
				double* pWeights = pCluster->GetControlPointWeights();

				for (int k = 0; k < nControlPointCount; k++)
				{
					NewBone->ControlPointIndexList->Add(pIndices[k]);
					NewBone->ControlPointWeightList->Add(static_cast<float>(pWeights[k]));
				}

				NewBone->LinkTransform = ParseFbxAMatrix(LinkMatrix);
				NewBone->Transform = ParseFbxAMatrix(TransfromMatrix);
				NewBone->BoneName = gcnew System::String(pCluster->GetLink()->GetName());				

				ResultBoneList->Add(NewBone->BoneName, NewBone);
			}
		}
	}

	return ResultBoneList;
}

ParsedFBXMeshBone^ FBXSDKWrapper::ParseBoneHierarchy(FbxNode* SceneRootNode)
{
	FbxNode* BoneRootNode = FindFirstBoneNode(SceneRootNode);

	ParsedFBXMeshBone^ Root = gcnew ParsedFBXMeshBone();
	Root->BoneName = gcnew System::String(BoneRootNode->GetName());
	Root->ParentBone = nullptr;
	ParseBoneHierarchyRecursive(BoneRootNode, Root);

	return Root;
}

FbxNode* FBXSDKWrapper::FindFirstBoneNode(FbxNode* VisitNode)
{
	if (VisitNode && VisitNode->GetNodeAttribute() && VisitNode->GetNodeAttribute()->GetAttributeType() == FbxNodeAttribute::eSkeleton)
	{
		return VisitNode;
	}

	int nChildCount = VisitNode->GetChildCount();

	for (int i = 0; i < nChildCount; ++i)
	{
		FbxNode* ChildNode = VisitNode->GetChild(i);

		if (ChildNode->GetNodeAttribute() && ChildNode->GetNodeAttribute()->GetAttributeType() == FbxNodeAttribute::eSkeleton)
		{
			return ChildNode;
		}
		else
		{
			return FindFirstBoneNode(ChildNode);
		}
	}

	return nullptr;
}

void FBXSDKWrapper::ParseBoneHierarchyRecursive(FbxNode* VisitNode, ParsedFBXMeshBone^ ParentBone)
{	
	int nChildCount = VisitNode->GetChildCount();	

	for (int i = 0; i < nChildCount; ++i)
	{
		FbxNode* ChildNode = VisitNode->GetChild(i);

		if (ChildNode && ChildNode->GetNodeAttribute() && ChildNode->GetNodeAttribute()->GetAttributeType() == FbxNodeAttribute::eSkeleton)
		{
			ParsedFBXMeshBone^ NewBone = gcnew ParsedFBXMeshBone();
			NewBone->BoneName = gcnew System::String(ChildNode->GetName());
			NewBone->ParentBone = ParentBone;
			ParentBone->ChildBoneList->Add(NewBone);
			ParseBoneHierarchyRecursive(ChildNode, NewBone);
		}
	}	
}

ParsedFBXMesh^ FBXSDKWrapper::ImportFBXMesh(System::String^ FilePath)
{	
	bool bImportSuccess = LoadScene(FBXManager, Scene, FilePath);

	ParsedFBXMesh^ Result = nullptr;

	if (Scene != nullptr && bImportSuccess == true)
	{
		FbxNode* RootNode = Scene->GetRootNode();
		
		if (RootNode)
		{
			Result = ParseFbxMesh(RootNode);
		}
	}

	return Result;
}

ParsedFBXAnimStack^ FBXSDKWrapper::ImportFBXAnimation(System::String^ FilePath)
{
	bool bImportSuccess = LoadScene(FBXManager, Scene, FilePath);

	if (Scene != nullptr && bImportSuccess == true)
	{
		for (int i = 0; i < Scene->GetSrcObjectCount<FbxAnimStack>(); i++)
		{
			FbxAnimStack* lAnimStack = Scene->GetSrcObject<FbxAnimStack>(i);

			ParsedFBXAnimStack^ Result = ParseFBXAnimation(lAnimStack, Scene->GetRootNode());

			return Result;
		}		
	}

	return nullptr;
}

ParsedFBXAnimStack^ FBXSDKWrapper::ParseFBXAnimation(FbxAnimStack* AnimStack, FbxNode* RootNode)
{
	ParsedFBXAnimStack^ NewStack = gcnew ParsedFBXAnimStack();

	NewStack->ParseNativeFBXAnimStack(AnimStack, RootNode);

	return NewStack;
}


Vector2 FBXSDKWrapper::Parse2DVector(FbxVector2 Value)
{
	Vector2 Result;
	Result.X = static_cast<float>(Value[0]);
	Result.Y = static_cast<float>(Value[1]);

	return Result;
}

Vector3 FBXSDKWrapper::Parse3DVector(FbxVector4 Value)
{
	Vector3 Result;
	Result.X = static_cast<float>(Value[0]);
	Result.Y = static_cast<float>(Value[1]);
	Result.Z = static_cast<float>(Value[2]);

	return Result;
}

Vector4 FBXSDKWrapper::Parse4DVector(FbxVector4 Value)
{
	Vector4 Result;
	Result.X = static_cast<float>(Value[0]);
	Result.Y = static_cast<float>(Value[1]);
	Result.Z = static_cast<float>(Value[2]);
	Result.W = static_cast<float>(Value[3]);
	return Result;
}

Matrix4 FBXSDKWrapper::ParseFbxAMatrix(FbxAMatrix Value)
{
	OpenTK::Matrix4 Result;

	Result.Row0 = Parse4DVector(Value.GetRow(0));
	Result.Row1 = Parse4DVector(Value.GetRow(1));	
	Result.Row2 = Parse4DVector(Value.GetRow(2));
	Result.Row3 = Parse4DVector(Value.GetRow(3));
	
	return Result;
}

void FBXSDKWrapper::Print4DVector(FbxVector4 Value)
{
	System::Console::WriteLine("X : {0}, Y : {1} , Z : {2} , W : {3}", Value[0], Value[1], Value[2], Value[3]);
}

bool FBXWrapper::FBXSDKWrapper::LoadScene(FbxManager* pFBXManager, FbxScene* pFBXScene, System::String^ FileName)
{
	int lFileMajor, lFileMinor, lFileRevision;
	int lSDKMajor, lSDKMinor, lSDKRevision;
	//int lFileFormat = -1;
	int i, lAnimStackCount;
	bool lStatus;		

	// Get the file version number generate by the FBX SDK.
	FbxManager::GetFileFormatVersion(lSDKMajor, lSDKMinor, lSDKRevision);

	// Create an importer.
	FbxImporter* lImporter = FbxImporter::Create(pFBXManager, "");

	// Initialize the importer by providing a filename.
	std::string stdFileName = marshal_as<std::string>(FileName);
	const bool lImportStatus = lImporter->Initialize(stdFileName.c_str(), -1, pFBXManager->GetIOSettings());
	lImporter->GetFileVersion(lFileMajor, lFileMinor, lFileRevision);

	if (!lImportStatus)
	{
		FbxString error = lImporter->GetStatus().GetErrorString();
		System::Console::Write("Call to FbxImporter::Initialize() failed.\n");		
		FBXSDK_printf("Error returned: %s\n\n", error.Buffer());

		if (lImporter->GetStatus().GetCode() == FbxStatus::eInvalidFileVersion)
		{
			FBXSDK_printf("FBX file format version for this FBX SDK is %d.%d.%d\n", lSDKMajor, lSDKMinor, lSDKRevision);
			FBXSDK_printf("FBX file format version for file '%s' is %d.%d.%d\n\n", stdFileName.c_str(), lFileMajor, lFileMinor, lFileRevision);
		}

		return false;
	}
	
	System::Console::WriteLine("FBX file format version for this FBX SDK is {0}.{1}.{2}", lSDKMajor, lSDKMinor, lSDKRevision);

	if (lImporter->IsFBX())
	{
		System::Console::WriteLine("FBX file format version for file '{0}' is {1}.{2}.{3}", FileName, lFileMajor, lFileMinor, lFileRevision);

		// From this point, it is possible to access animation stack information without
		// the expense of loading the entire file.

		System::Console::WriteLine("Animation Stack Information\n");

		lAnimStackCount = lImporter->GetAnimStackCount();

		System::Console::WriteLine("    Number of Animation Stacks: {0}", lAnimStackCount);
		System::String^ AnimationStackName = gcnew System::String(lImporter->GetActiveAnimStackName().Buffer());
		System::Console::WriteLine("    Current Animation Stack: \"{0}\"", AnimationStackName);
		System::Console::WriteLine("");

		for (i = 0; i < lAnimStackCount; i++)
		{
			FbxTakeInfo* lTakeInfo = lImporter->GetTakeInfo(i);

			System::Console::WriteLine("    Animation Stack {0}", i);
			System::Console::WriteLine("         Name: \"{0}\"", gcnew System::String(lTakeInfo->mName.Buffer()));
			System::Console::WriteLine("         Description: \"{0}\"", gcnew System::String(lTakeInfo->mDescription.Buffer()));

			// Change the value of the import name if the animation stack should be imported 
			// under a different name.
			System::Console::WriteLine("         Import Name: \"{0}\"\n", gcnew System::String(lTakeInfo->mImportName.Buffer()));

			// Set the value of the import state to false if the animation stack should be not
			// be imported. 
			System::Console::WriteLine("         Import State: {0}\n", lTakeInfo->mSelect ? "true" : "false");
			System::Console::WriteLine("\n");
		}

		// Set the import states. By default, the import states are always set to 
		// true. The code below shows how to change these states.
		pFBXManager->GetIOSettings()->SetBoolProp(IMP_FBX_MATERIAL, true);
		pFBXManager->GetIOSettings()->SetBoolProp(IMP_FBX_TEXTURE, true);
		pFBXManager->GetIOSettings()->SetBoolProp(IMP_FBX_LINK, true);
		pFBXManager->GetIOSettings()->SetBoolProp(IMP_FBX_SHAPE, true);
		pFBXManager->GetIOSettings()->SetBoolProp(IMP_FBX_GOBO, true);
		pFBXManager->GetIOSettings()->SetBoolProp(IMP_FBX_ANIMATION, true);
		pFBXManager->GetIOSettings()->SetBoolProp(IMP_FBX_GLOBAL_SETTINGS, true);
	}

	// Import the scene.
	lStatus = lImporter->Import(pFBXScene);
	
	// Destroy the importer.
	lImporter->Destroy();

	return lStatus;
}

