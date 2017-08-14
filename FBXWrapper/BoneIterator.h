#pragma once

#include "FBXSDKWrapper.h"

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class BoneIterator
	{
	public:
		BoneIterator(FBXMeshBone^ RootBone);
		void MoveNext();		
		bool IsEnd();
		void Reset();
		FBXMeshBone^ Current();
	protected:
		List<FBXMeshBone^>^ BoneList = gcnew List<FBXMeshBone^>();
		void AddBoneRecursive(FBXMeshBone^ ParentBone);
		int Index = 0;
	};
}

