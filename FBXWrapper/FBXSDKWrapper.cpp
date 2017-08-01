
#include "fbxsdk.h"
#include "FBXSDKWrapper.h"
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;

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

bool FBXWrapper::FBXSDKWrapper::ImportFBXMesh(System::String^ FilePath)
{
	OpenTK::Vector3 Test;	
	LoadScene(FBXManager, Scene, FilePath);
	return true;
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
		FBXSDK_printf("Call to FbxImporter::Initialize() failed.\n");
		FBXSDK_printf("Error returned: %s\n\n", error.Buffer());

		if (lImporter->GetStatus().GetCode() == FbxStatus::eInvalidFileVersion)
		{
			FBXSDK_printf("FBX file format version for this FBX SDK is %d.%d.%d\n", lSDKMajor, lSDKMinor, lSDKRevision);
			FBXSDK_printf("FBX file format version for file '%s' is %d.%d.%d\n\n", stdFileName.c_str(), lFileMajor, lFileMinor, lFileRevision);
		}

		return false;
	}


	FBXSDK_printf("FBX file format version for this FBX SDK is %d.%d.%d\n", lSDKMajor, lSDKMinor, lSDKRevision);

	if (lImporter->IsFBX())
	{
		FBXSDK_printf("FBX file format version for file '%s' is %d.%d.%d\n\n", stdFileName.c_str(), lFileMajor, lFileMinor, lFileRevision);

		// From this point, it is possible to access animation stack information without
		// the expense of loading the entire file.

		FBXSDK_printf("Animation Stack Information\n");

		lAnimStackCount = lImporter->GetAnimStackCount();

		FBXSDK_printf("    Number of Animation Stacks: %d\n", lAnimStackCount);
		FBXSDK_printf("    Current Animation Stack: \"%s\"\n", lImporter->GetActiveAnimStackName().Buffer());
		FBXSDK_printf("\n");

		for (i = 0; i < lAnimStackCount; i++)
		{
			FbxTakeInfo* lTakeInfo = lImporter->GetTakeInfo(i);

			FBXSDK_printf("    Animation Stack %d\n", i);
			FBXSDK_printf("         Name: \"%s\"\n", lTakeInfo->mName.Buffer());
			FBXSDK_printf("         Description: \"%s\"\n", lTakeInfo->mDescription.Buffer());

			// Change the value of the import name if the animation stack should be imported 
			// under a different name.
			FBXSDK_printf("         Import Name: \"%s\"\n", lTakeInfo->mImportName.Buffer());

			// Set the value of the import state to false if the animation stack should be not
			// be imported. 
			FBXSDK_printf("         Import State: %s\n", lTakeInfo->mSelect ? "true" : "false");
			FBXSDK_printf("\n");
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
