#pragma once

#using <OpenTK.dll>

#include "ParsedFBXAnimNode.h"

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXAnimLayer
	{
	public:
		System::String^ LayerName;
		
		List<ParsedFBXAnimNode^>^ AnimNodeList = gcnew List<ParsedFBXAnimNode^>();

		void ParseNativeFBXAnimLayer(FbxAnimStack* NativeAnimStack, FbxAnimLayer* NativeAnimLayer, FbxNode* NativeNode);
	};
};