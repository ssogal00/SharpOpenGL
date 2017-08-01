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

		class FbxScene* Scene = nullptr;
		class FbxManager* FBXManager = nullptr;		
	};
};

