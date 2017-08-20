#pragma once

#include "fbxsdk.h"
#using <OpenTK.dll>
#include "ParsedFBXMeshBone.h"
#include "SkinningInfo.h"

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXMesh
	{
	public:
		List<OpenTK::Vector3>^ VertexList = gcnew List<OpenTK::Vector3>();
		List<OpenTK::Vector3>^ NormalList = gcnew List<OpenTK::Vector3>();
		List<OpenTK::Vector2>^ UVList = gcnew List<OpenTK::Vector2>();
		List<OpenTK::Vector3>^ ControlPointList = gcnew List<OpenTK::Vector3>();
		Dictionary<int, List<SkinningInfo^>^>^ SkinningInfoMap = gcnew Dictionary<int, List<SkinningInfo^>^>();
		Dictionary<System::String^, ParsedFBXMeshBone^>^ BoneMap = gcnew Dictionary<System::String^, ParsedFBXMeshBone^>();
		List<int>^ IndexList = gcnew List<int>();
		ParsedFBXMeshBone^ RootBone = nullptr;
		int PolygonCount = 0;
	};
};