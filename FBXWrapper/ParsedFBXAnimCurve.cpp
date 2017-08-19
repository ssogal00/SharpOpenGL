#include "fbxsdk.h"
#include "ParsedFBXAnimCurve.h"
#include "ParsedFBXAnimKey.h"

using namespace FBXWrapper;
using namespace OpenTK;

float ParsedFBXAnimCurve::GetValue(int KeyTimeIndex)
{
	if ( KeyTimeIndex >= 0 && KeyTimeIndex < AnimKeyList->Count )
	{
		return AnimKeyList[KeyTimeIndex]->KeyValue;
	}
	
	return -1;
}



void FBXWrapper::ParsedFBXAnimCurve::ParseNativeFBXAnimCurve(FbxAnimCurve* pCurve)
{
	if (pCurve != nullptr)
	{
		FbxTime   lKeyTime;
		float   lKeyValue;
		char    lTimeString[256];
		FbxString lOutputString;
		int     lCount;
		int lKeyCount = pCurve->KeyGetCount();

		KeyCount = lKeyCount;

		for (lCount = 0; lCount < lKeyCount; lCount++)
		{
			ParsedFBXAnimKey^ AnimKey = gcnew ParsedFBXAnimKey();

			AnimKey->KeyValue = static_cast<float>(pCurve->KeyGetValue(lCount));
			lKeyTime = pCurve->KeyGetTime(lCount);
			AnimKey->KeyTimeString = gcnew System::String(lKeyTime.GetTimeString(lTimeString, FbxUShort(256)));

			// constant
			if (pCurve->KeyGetInterpolation(lCount) & FbxAnimCurveDef::eInterpolationConstant)
			{
				AnimKey->KeyInterpolationMethod = InterpolationMethod::EInterpolationConstant;
			}
			// Cubic
			else if (pCurve->KeyGetInterpolation(lCount) & FbxAnimCurveDef::eInterpolationCubic)
			{
				AnimKey->KeyInterpolationMethod = InterpolationMethod::EInterpolationCubic;
			}

			AnimKeyList->Add(AnimKey);
		}
	}
}