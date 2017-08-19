#pragma once

#include "fbxsdk.h"

#include "ParsedFBXAnimLayer.h"
#include "ParsedFBXAnimNode.h"

#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXAnimStack
	{
	public :
		ParsedFBXAnimNode^ GetAnimNode(int nLayerIndex, System::String^ NodeName);

		System::String^ StackName;
		int NumOfLayers;
		List<ParsedFBXAnimLayer^>^ AnimLayerList = gcnew List<ParsedFBXAnimLayer^>();
		void ParseNativeFBXAnimStack(FbxAnimStack* NativeAnimStack, FbxNode* NativeRootNode);		

	};
};