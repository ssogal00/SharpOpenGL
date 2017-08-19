#pragma once
#using <OpenTK.dll>

#include "ParsedFBXAnimStack.h"
#include "ParsedFBXMeshBone.h"
#include "ParsedFBXMesh.h"

#include "fbxsdk.h"

namespace FBXWrapper
{
	public ref class ParsedFBXAnimation
	{
	public:
		ParsedFBXAnimation(ParsedFBXMesh^ pMesh, ParsedFBXAnimStack^ pAnimStack);

		OpenTK::Matrix4 GetTransform(System::String^ BoneName, int KeyTimeIndex);
			
		ParsedFBXMesh^ Mesh = nullptr;
	protected:
		ParsedFBXAnimStack^ AnimStack = nullptr;
	};
};
