#pragma once

#include "fbxsdk.h"

#using <OpenTK.dll>

namespace FBXWrapper
{
	public ref class FBXSDKWrapper
	{
	public:
		bool InitializeSDK();
		bool ImportFBXMesh(System::String^ FilePath);
		
	protected:
		bool LoadScene(FbxManager* pFBXManager, FbxScene* pFBXScene, System::String^ FileName);

		void ParseNode(FbxNode* Node);
		void ParseFbxMesh(FbxMesh* Mesh);

		OpenTK::Vector2 Parse2DVector(FbxVector2 Value);
		OpenTK::Vector3 Parse3DVector(FbxVector4 Value);
		OpenTK::Vector4 Parse4DVector(FbxVector4 Value);

		class FbxScene* Scene = nullptr;
		class FbxManager* FBXManager = nullptr;		
	};
};

