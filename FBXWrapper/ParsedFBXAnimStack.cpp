
#include "fbxsdk.h"
#include "ParsedFBXAnimStack.h"
#include "ParsedFBXAnimLayer.h"

using namespace FBXWrapper;

void ParsedFBXAnimStack::ParseNativeFBXAnimStack(FbxAnimStack* NativeAnimStack, FbxNode* NativeRootNode)
{
	int AnimLayers = NativeAnimStack->GetMemberCount<FbxAnimLayer>();
	
	for (int i = 0; i < AnimLayers; i++)
	{
		FbxAnimLayer* pAnimLayer = NativeAnimStack->GetMember<FbxAnimLayer>(i);		

		ParsedFBXAnimLayer^ NewLayer = gcnew ParsedFBXAnimLayer();
		NewLayer->ParseNativeFBXAnimLayer(NativeAnimStack, pAnimLayer, NativeRootNode);
		AnimLayerList->Add(NewLayer);
	}
}