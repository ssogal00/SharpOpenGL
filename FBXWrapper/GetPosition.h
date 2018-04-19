#pragma once
#include "fbxsdk.h"

FbxAMatrix GetGlobalPosition(FbxNode* pNode, const FbxTime& time, FbxPose* pPose = nullptr, FbxAMatrix* pParentGlobalPosition = nullptr);

FbxAMatrix GetPoseMatrix(FbxPose* pPose, int nodeIndex);

FbxAMatrix GetGeometry(FbxNode* pNode);