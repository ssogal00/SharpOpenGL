
#include "fbxsdk.h"
#include "ParsedFBXAnimCurve.h"
#include "ParsedFBXAnimNode.h"
#include "FBXSDKWrapper.h"

using namespace FBXWrapper;
using namespace OpenTK;
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

OpenTK::Matrix4 ParsedFBXAnimNode::GetTransform(int KeyTimeIndex)
{
	

	OpenTK::Vector3 Translation;
	Translation.X = TXCurve->GetValue(KeyTimeIndex);
	Translation.Y = TYCurve->GetValue(KeyTimeIndex);
	Translation.Z = TZCurve->GetValue(KeyTimeIndex);

	OpenTK::Matrix4 T = OpenTK::Matrix4::CreateTranslation(Translation);
			
	OpenTK::Vector3 Scale;
	Scale.X = SXCurve->GetValue(KeyTimeIndex);
	Scale.Y = SYCurve->GetValue(KeyTimeIndex);
	Scale.Z = SZCurve->GetValue(KeyTimeIndex);

	OpenTK::Matrix4 S = OpenTK::Matrix4::CreateScale(Scale);

	OpenTK::Vector3 Rotation;
	Rotation.X = OpenTK::MathHelper::DegreesToRadians(RXCurve->GetValue(KeyTimeIndex));
	Rotation.Y = OpenTK::MathHelper::DegreesToRadians(RYCurve->GetValue(KeyTimeIndex));
	Rotation.Z = OpenTK::MathHelper::DegreesToRadians(RZCurve->GetValue(KeyTimeIndex));
	
	Matrix4 RX = OpenTK::Matrix4::CreateRotationX(Rotation.X);
	Matrix4 RY = OpenTK::Matrix4::CreateRotationY(Rotation.Y);
	Matrix4 RZ = OpenTK::Matrix4::CreateRotationZ(Rotation.Z);
	
	Matrix4 R = RX * RY * RZ;
	//Matrix4 R = RZ * RY * RX;
	
	Matrix4 Result = S * R * T;
	//Matrix4 Result =  T * R * S;

	return Result;
}

void FBXWrapper::ParsedFBXAnimNode::ParseNativeFBXAnimNode(FbxAnimLayer* NativeAnimLayer, FbxNode* NativeNode)
{
	if (NativeNode != nullptr)
	{
		NodeName = gcnew System::String(NativeNode->GetName());

		// @start Translation
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
		// @end Translation

		// @start Rotation
		pCurve = NativeNode->LclRotation.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_X);
		if(pCurve)
		{
			RXCurve->ParseNativeFBXAnimCurve(pCurve);
		}

		pCurve = NativeNode->LclRotation.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_Y);
		if (pCurve)
		{
			RYCurve->ParseNativeFBXAnimCurve(pCurve);
		}
		
		pCurve = NativeNode->LclRotation.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_Z);
		if (pCurve)
		{
			RZCurve->ParseNativeFBXAnimCurve(pCurve);
		}
		// @end Rotation

		// @start Scale
		pCurve = NativeNode->LclScaling.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_X);
		if (pCurve)
		{
			SXCurve->ParseNativeFBXAnimCurve(pCurve);
		}

		pCurve = NativeNode->LclScaling.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_Y);
		if (pCurve)
		{
			SYCurve->ParseNativeFBXAnimCurve(pCurve);
		}

		pCurve = NativeNode->LclScaling.GetCurve(NativeAnimLayer, FBXSDK_CURVENODE_COMPONENT_Z);
		if (pCurve)
		{
			SZCurve->ParseNativeFBXAnimCurve(pCurve);
		}
		// @end Scale

// 		if (pCurve)
// 		{
// 			FbxTime   lKeyTime;
// 			int     lCount;
// 			int lKeyCount = pCurve->KeyGetCount();
// 
// 			for (lCount = 0; lCount < lKeyCount; lCount++)
// 			{
// 				lKeyTime = pCurve->KeyGetTime(lCount);
// 				TransformList.Add(FBXSDKWrapper::ParseFbxAMatrix(NativeNode->EvaluateLocalTransform(lKeyTime)));
// 			}
// 		}
	}
}