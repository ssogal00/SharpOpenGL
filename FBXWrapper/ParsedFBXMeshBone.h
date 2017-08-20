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
		List<int>^ ControlPointIndexList = gcnew List<int>();
		List<float>^ ControlPointWeightList = gcnew List<float>();
		ParsedFBXMeshBone^ ParentBone = nullptr;
		List<ParsedFBXMeshBone^>^ ChildBoneList = gcnew List<ParsedFBXMeshBone^>();
	};
};