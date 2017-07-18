

#include "fbxsdk.h"

ref class Hello
{
public:
	void Print()
	{
		System::Console::Write("Hello world");

		FbxManager* pManager = FbxManager::Create();

		if (!pManager)
		{
			FBXSDK_printf("Error: Unable to create FBX Manager!\n");			
		}
		else
		{
			FBXSDK_printf("Autodesk FBX SDK version %s\n", pManager->GetVersion());
		}
	}
};