#pragma once

#include "fbxsdk.h"
#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXMeshBone
	{
	public:
		System::String^ BoneName = nullptr;
		OpenTK::Matrix4^ Transform = nullptr;
		OpenTK::Matrix4^ LinkTransform = nullptr;
		List<unsigned int> ControlPointIndexList = gcnew List<unsigned int>();
		List<float> ControlPointWeight = gcnew List<float>();
		ParsedFBXMeshBone^ ParentBone = nullptr;
		List<ParsedFBXMeshBone^>^ ChildBoneList = gcnew List<ParsedFBXMeshBone^>();
	};
};