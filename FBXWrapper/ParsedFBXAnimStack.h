#pragma once

#include "fbxsdk.h"

#include "ParsedFBXAnimLayer.h"

#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXAnimStack
	{
	public :
		System::String^ StackName;		
		int NumOfLayers;

		List<ParsedFBXAnimLayer^>^ AnimLayerList = gcnew List<ParsedFBXAnimLayer^>();
		void ParseNativeFBXAnimStack(FbxAnimStack* NativeAnimStack, FbxNode* NativeRootNode);
	};
};