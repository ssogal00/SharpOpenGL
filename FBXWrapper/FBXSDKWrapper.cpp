
#include "fbxsdk.h"
#include "FBXSDKWrapper.h"
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;
using namespace OpenTK;
using namespace System::Collections::Generic;

bool FBXWrapper::FBXSDKWrapper::InitializeSDK()
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

FBXWrapper::ParsedFBXMesh^ FBXWrapper::FBXSDKWrapper::ParseFbxMesh(FbxNode* Node)
{
	if (Node != nullptr)
	{
		for (int i = 0; i < Node->GetChildCount(); ++i)
		{
			FbxNode* ChildNode = Node->GetChild(i);

			if (ChildNode && ChildNode->GetNodeAttribute() && ChildNode->GetNodeAttribute()->GetAttributeType() == FbxNodeAttribute::eMesh)
			{
				return ParseFbxMesh((FbxMesh*)(ChildNode->GetNodeAttribute()));
			}
		}
	}

	for (int i = 0; i < Node->GetChildCount(); i++)
	{		
		return ParseFbxMesh(Node->GetChild(i));
	}

	return nullptr;
}

FBXWrapper::ParsedFBXMesh^ FBXWrapper::FBXSDKWrapper::ParseFbxMesh(FbxMesh* Mesh)
{
	ParsedFBXMesh^ ResultMesh = gcnew ParsedFBXMesh();
	 	
	ResultMesh->VertexList = ParseFbxMeshVertex(Mesh);
	ResultMesh->NormalList = ParseFbxMeshNormal(Mesh);
	ResultMesh->UVList = ParseFbxMeshUV(Mesh);
	 
	return ResultMesh;
}

System::Collections::Generic::List<OpenTK::Vector3>^ FBXWrapper::FBXSDKWrapper::ParseFbxMeshVertex(FbxMesh* Mesh)
{
	int nPolygonCount = Mesh->GetPolygonCount();
	int nControlPointCount = Mesh->GetControlPointsCount();
	
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

System::Collections::Generic::List<OpenTK::Vector3>^ FBXWrapper::FBXSDKWrapper::ParseFbxMeshNormal(FbxMesh* Mesh)
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

List<OpenTK::Vector2>^ FBXWrapper::FBXSDKWrapper::ParseFbxMeshUV(FbxMesh* Mesh)
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

FBXWrapper::ParsedFBXMesh^ FBXWrapper::FBXSDKWrapper::ImportFBXMesh(System::String^ FilePath)
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


Vector2 FBXWrapper::FBXSDKWrapper::Parse2DVector(FbxVector2 Value)
{
	Vector2 Result;
	Result.X = static_cast<float>(Value[0]);
	Result.Y = static_cast<float>(Value[1]);

	return Result;
}

Vector3 FBXWrapper::FBXSDKWrapper::Parse3DVector(FbxVector4 Value)
{
	Vector3 Result;
	Result.X = static_cast<float>(Value[0]);
	Result.Y = static_cast<float>(Value[1]);
	Result.Z = static_cast<float>(Value[2]);

	return Result;
}

Vector4 FBXWrapper::FBXSDKWrapper::Parse4DVector(FbxVector4 Value)
{
	Vector4 Result;
	Result.X = static_cast<float>(Value[0]);
	Result.Y = static_cast<float>(Value[1]);
	Result.Z = static_cast<float>(Value[2]);
	Result.W = static_cast<float>(Value[3]);
	return Result;
}

bool FBXWrapper::FBXSDKWrapper::LoadScene(FbxManager* pFBXManager, FbxScene* pFBXScene, System::String^ FileName)
{
	int lFileMajor, lFileMinor, lFileRevision;
	int lSDKMajor, lSDKMinor, lSDKRevision;
	//int lFileFormat = -1;
	int i, lAnimStackCount;
	bool lStatus;
	char lPassword[1024];	

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

