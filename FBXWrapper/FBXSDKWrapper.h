#pragma once

#include "fbxsdk.h"
namespace FBXWrapper
{
	public ref class FBXSDKWrapper
	{
	public:
		bool InitializeSDK();

		bool ImportFBXMesh(System::String^ FilePath);		

	protected:
		bool LoadScene(FbxManager* pFBXManager, FbxScene* pFBXScene, System::String^ FileName);

		class FbxScene* Scene = nullptr;
		class FbxManager* FBXManager = nullptr;
	};
};

