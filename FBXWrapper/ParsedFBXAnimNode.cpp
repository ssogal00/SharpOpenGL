
#include "fbxsdk.h"
#include "ParsedFBXAnimCurve.h"
#include "ParsedFBXAnimNode.h"

using namespace FBXWrapper;

ParsedFBXAnimNode::ParsedFBXAnimNode()
{
	TXCurve = gcnew ParsedFBXAnimCurve();
	TYCurve = gcnew ParsedFBXAnimCurve();
	TZCurve = gcnew ParsedFBXAnimCurve();

	RXCurve = gcnew ParsedFBXAnimCurve();
	RYCurve = gcnew ParsedFBXAnimCurve();
	RZCurve = gcnew ParsedFBXAnimCurve();

	SXCurve = gcnew ParsedFBXAnimCurve();
	SYCurve = gcnew ParsedFBXAnimCurve();
	SZCurve = gcnew ParsedFBXAnimCurve();
}

void FBXWrapper::ParsedFBXAnimNode::ParseNativeFBXAnimNode(FbxAnimLayer* NativeAnimLayer, FbxNode* NativeNode)
{
	if (NativeNode != nullptr)
	{
		NodeName = gcnew System::String(NativeNode->GetName());

		FbxAnimCurve* pCurve = NativeNode->LclTranslation.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_X);
		if (pCurve != nullptr && TXCurve != nullptr)
		{
			TXCurve->ParseNativeFBXAnimCurve(pCurve);
		}

		pCurve = NativeNode->LclTranslation.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_Y);
		if (pCurve != nullptr && TYCurve != nullptr)
		{
			TYCurve->ParseNativeFBXAnimCurve(pCurve);
		}

		pCurve = NativeNode->LclTranslation.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_Z);
		if (pCurve)
		{
			TZCurve->ParseNativeFBXAnimCurve(pCurve);
		}
	}
}