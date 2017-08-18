#pragma once

#include "fbxsdk.h"
#include "ParsedFBXEnum.h"
#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXAnimKey
	{
	public:
		System::String^ KeyTimeString = nullptr;
		InterpolationMethod KeyInterpolationMethod = InterpolationMethod::EInterpolationUnknown;
		float KeyValue;
	};
};