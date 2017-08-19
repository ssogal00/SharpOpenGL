
#include "ParsedFBXAnimLayer.h"
#include "ParsedFBXAnimNode.h"

using namespace FBXWrapper;

void ParsedFBXAnimLayer::ParseNativeFBXAnimLayer(FbxAnimStack* NativeAnimStack, FbxAnimLayer* NativeAnimLayer, FbxNode* NativeNode)
{
	if (NativeNode != nullptr && NativeAnimLayer != nullptr)
	{
		ParsedFBXAnimNode^ NewNode = gcnew ParsedFBXAnimNode();

		NewNode->ParseNativeFBXAnimNode(NativeAnimLayer, NativeNode);

		AnimNodeList->Add(NewNode);

		AnimNodeMap->Add(NewNode->NodeName, NewNode);

		for (int nModelCount = 0; nModelCount < NativeNode->GetChildCount(); nModelCount++)
		{
			ParseNativeFBXAnimLayer(NativeAnimStack, NativeAnimLayer, NativeNode->GetChild(nModelCount));
		}
	}
}