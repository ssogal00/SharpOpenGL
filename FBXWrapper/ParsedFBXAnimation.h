#pragma once
#using <OpenTK.dll>

#include "ParsedFBXAnimStack.h"
#include "ParsedFBXMeshBone.h"

#include "fbxsdk.h"

namespace FBXWrapper
{
	public ref class ParsedFBXAnimation
	{
	public:
		

		OpenTK::Matrix4 GetTransform(System::String^ BoneName, int KeyTimeIndex);

	protected:
		ParsedFBXAnimStack^ AnimStack = nullptr;
	};
};
