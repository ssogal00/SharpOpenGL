#pragma once
#using <OpenTK.dll>

#include "fbxsdk.h"

namespace FBXWrapper
{
	public ref class ParsedFBXAnimation
	{
	public:
		OpenTK::Matrix4 GetTransform(System::String^ NodeName, System::String^ KeyTimeString);
	};
};
