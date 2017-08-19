
#include "fbxsdk.h"
#include "ParsedFBXAnimStack.h"
#include "ParsedFBXAnimLayer.h"

using namespace FBXWrapper;


void ParsedFBXAnimStack::ParseNativeFBXAnimStack(FbxAnimStack* NativeAnimStack, FbxNode* NativeRootNode)
{
	if (NativeAnimStack != nullptr)
	{
		int AnimLayers = NativeAnimStack->GetMemberCount<FbxAnimLayer>();

		StackName = gcnew System::String(NativeAnimStack->GetName());

		for (int i = 0; i < AnimLayers; i++)
		{
			FbxAnimLayer* pAnimLayer = NativeAnimStack->GetMember<FbxAnimLayer>(i);

			ParsedFBXAnimLayer^ NewLayer = gcnew ParsedFBXAnimLayer();
			NewLayer->ParseNativeFBXAnimLayer(NativeAnimStack, pAnimLayer, NativeRootNode);
			AnimLayerList->Add(NewLayer);
		}
	}
}

ParsedFBXAnimNode^ ParsedFBXAnimStack::GetAnimNode(int nLayerIndex, System::String^ NodeName)
{
	if (nLayerIndex < AnimLayerList->Count)
	{
		return AnimLayerList[nLayerIndex]->AnimNodeMap[NodeName];
	}

	return nullptr;
}
