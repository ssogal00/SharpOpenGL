#pragma once

#pragma once

#include "ParsedFBXAnimCurve.h"

#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXAnimNode
	{
	public:
		ParsedFBXAnimNode();

		System::String^ NodeName;

		void ParseNativeFBXAnimNode(FbxAnimLayer* NativeAnimLayer, FbxNode* NativeNode);

		OpenTK::Matrix4 GetTransform(int KeyTimeIndex);
		
		ParsedFBXAnimCurve^ TXCurve = nullptr;
		ParsedFBXAnimCurve^ TYCurve = nullptr;
		ParsedFBXAnimCurve^ TZCurve = nullptr;

		ParsedFBXAnimCurve^ RXCurve = nullptr;
		ParsedFBXAnimCurve^ RYCurve = nullptr;
		ParsedFBXAnimCurve^ RZCurve = nullptr;

		ParsedFBXAnimCurve^ SXCurve = nullptr;
		ParsedFBXAnimCurve^ SYCurve = nullptr;
		ParsedFBXAnimCurve^ SZCurve = nullptr;		

		List<OpenTK::Matrix4> TransformList = gcnew List<OpenTK::Matrix4>();
	};

};
