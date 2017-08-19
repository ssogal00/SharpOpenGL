#pragma once

#include "fbxsdk.h"
#include "ParsedFBXAnimKey.h"
#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{	
	public ref class ParsedFBXAnimCurve
	{
	public:
		int KeyCount;
		List<ParsedFBXAnimKey^>^ AnimKeyList = gcnew List<ParsedFBXAnimKey^>();
		void ParseNativeFBXAnimCurve(FbxAnimCurve* pCurve);
		float GetValue(int KeyTimeIndex);
	private:
	};
};
