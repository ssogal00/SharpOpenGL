#pragma once

#include "FBXSDKWrapper.h"

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class BoneIterator
	{
	public:
		BoneIterator(ParsedFBXMeshBone^ RootBone);
		void MoveNext();		
		bool IsEnd();
		void Reset();
		ParsedFBXMeshBone^ Current();
	protected:
		List<ParsedFBXMeshBone^>^ BoneList = gcnew List<ParsedFBXMeshBone^>();
		void AddBoneRecursive(ParsedFBXMeshBone^ ParentBone);
		int Index = 0;
	};
}

