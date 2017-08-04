#pragma once

#include "fbxsdk.h"

#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXMesh
	{
	public:

		List<OpenTK::Vector3>^ VertexList = gcnew List<OpenTK::Vector3>();
		List<OpenTK::Vector3>^ NormalList = gcnew List<OpenTK::Vector3>();
		List<OpenTK::Vector2>^ UVList = gcnew List<OpenTK::Vector2>();
		List<int>^ IndexList = gcnew List<int>();
		int PolygonCount = 0;
	};

	public ref class FBXSDKWrapper
	{
	public:
		bool InitializeSDK();
		ParsedFBXMesh^ ImportFBXMesh(System::String^ FilePath);
		
	protected:
		bool LoadScene(FbxManager* pFBXManager, FbxScene* pFBXScene, System::String^ FileName);

		ParsedFBXMesh^ ParseFbxMesh(FbxNode* Node);

		ParsedFBXMesh^ ParseFbxMesh(FbxMesh* Mesh);

		List<OpenTK::Vector3>^ ParseFbxMeshVertex(FbxMesh* Mesh);
		List<OpenTK::Vector3>^ ParseFbxMeshNormal(FbxMesh* Mesh);
		List<OpenTK::Vector2>^ ParseFbxMeshUV(FbxMesh* Mesh);

		OpenTK::Vector2 Parse2DVector(FbxVector2 Value);
		OpenTK::Vector3 Parse3DVector(FbxVector4 Value);
		OpenTK::Vector4 Parse4DVector(FbxVector4 Value);

		class FbxScene* Scene = nullptr;
		class FbxManager* FBXManager = nullptr;		

		
	};
};

