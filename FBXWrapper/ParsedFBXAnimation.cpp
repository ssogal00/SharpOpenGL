#include "fbxsdk.h"
#include "ParsedFBXAnimation.h"
#include "ParsedFBXAnimStack.h"
#include "ParsedFBXAnimNode.h"

using namespace FBXWrapper;
using namespace OpenTK;

ParsedFBXAnimation::ParsedFBXAnimation(ParsedFBXMesh^ pMesh, ParsedFBXAnimStack^ pAnimStack)
	: Mesh(pMesh), AnimStack(pAnimStack)
{
}

Matrix4 ParsedFBXAnimation::GetTransform(System::String^ BoneName, int KeyTimeIndex)
{
	ParsedFBXAnimNode^ Node = AnimStack->GetAnimNode(0, BoneName);

	if (Node != nullptr)
	{
		return Node->GetTransform(KeyTimeIndex);
	}

	return Matrix4::Identity;
}
