#pragma once

#include "fbxsdk.h"
#include "ParsedFBXAnimStack.h"
#include "ParsedFBXMeshBone.h"
#include "ParsedFBXMesh.h"
#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class FBXSDKWrapper
	{
	public:
		bool InitializeSDK();

		ParsedFBXMesh^ ImportFBXMesh(System::String^ FilePath);

		ParsedFBXAnimStack^ ImportFBXAnimation(System::String^ FilePath);
		
	protected:
		bool LoadScene(FbxManager* pFBXManager, FbxScene* pFBXScene, System::String^ FileName);

		ParsedFBXMesh^ ParseFbxMesh(FbxNode* Node);
		ParsedFBXMesh^ ParseFbxMesh(FbxMesh* Mesh, FbxNode* Node);

		List<OpenTK::Vector3>^ ParseFbxMeshVertex(FbxMesh* Mesh);
		List<OpenTK::Vector3>^ ParseFbxMeshNormal(FbxMesh* Mesh);
		List<OpenTK::Vector2>^ ParseFbxMeshUV(FbxMesh* Mesh);
		List<OpenTK::Vector3>^ ParseFbxControlPointList(FbxMesh* Mesh);
		Dictionary<System::String^, ParsedFBXMeshBone^>^	ParseFbxMeshBone(FbxMesh* Mesh);
		ParsedFBXMeshBone^ ParseBoneHierarchy(FbxNode* SceneRootNode);
		FbxNode* FindFirstBoneNode(FbxNode* SceneRootNode);
		void ParseBoneHierarchyRecursive(FbxNode* VisitNode, ParsedFBXMeshBone^ ParentBone);

		//
		OpenTK::Vector2 Parse2DVector(FbxVector2 Value);
		OpenTK::Vector3 Parse3DVector(FbxVector4 Value);
		OpenTK::Vector4 Parse4DVector(FbxVector4 Value);
		OpenTK::Matrix4 ParseFbxAMatrix(FbxAMatrix Value);
		void Print4DVector(FbxVector4 Value);
		//

		//
		ParsedFBXAnimStack^ ParseFBXAnimation(FbxAnimStack* AnimStack, FbxNode* RootNode);		
		//

		class FbxScene* Scene = nullptr;
		class FbxManager* FBXManager = nullptr;		
	};
};

