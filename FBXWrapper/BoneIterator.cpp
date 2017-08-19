
#include "FBXSDKWrapper.h"
#include "BoneIterator.h"

FBXWrapper::BoneIterator::BoneIterator(ParsedFBXMeshBone^ RootBone)
{
	AddBoneRecursive(RootBone);
}

void FBXWrapper::BoneIterator::AddBoneRecursive(ParsedFBXMeshBone^ ParentBone)
{
	BoneList->Add(ParentBone);	

	for (int i = 0; i < ParentBone->ChildBoneList->Count; ++i)
	{
		AddBoneRecursive(ParentBone->ChildBoneList[i]);
	}
}

void FBXWrapper::BoneIterator::MoveNext()
{
	++Index;
}

FBXWrapper::ParsedFBXMeshBone^ FBXWrapper::BoneIterator::Current()
{
	if (Index < BoneList->Count)
	{
		return BoneList[Index];
	}

	return nullptr;
}

bool FBXWrapper::BoneIterator::IsEnd()
{
	if (Index >= BoneList->Count)
	{
		return true;
	}
	else
	{
		return false;
	}
}

void FBXWrapper::BoneIterator::Reset()
{
	Index = 0;
}
