
#include "GetPosition.h"

FbxAMatrix GetGlobalPosition(FbxNode* pNode, const FbxTime& time, FbxPose* pPose, FbxAMatrix* pParentGlobalPosition)
{
	FbxAMatrix globalPosition;

	bool bPositionFound = false;

	if (pPose)
	{
		int nodeIndex = pPose->Find(pNode);

		if (nodeIndex > -1)
		{
			if (pPose->IsBindPose() || !pPose->IsLocalMatrix(nodeIndex))
			{
				globalPosition = GetPoseMatrix(pPose, nodeIndex);
			}
			else
			{
				FbxAMatrix parentGlobalPosition;

				if (pParentGlobalPosition != nullptr)
				{
					parentGlobalPosition = *pParentGlobalPosition;
				}
				else
				{
					if (pNode->GetParent())
					{
						parentGlobalPosition = GetGlobalPosition(pNode->GetParent(), time, pPose);
					}
				}

				FbxAMatrix localPosition = GetPoseMatrix(pPose, nodeIndex);
				globalPosition = parentGlobalPosition * localPosition;
			}

			bPositionFound = true;
		}
	}

	if (!bPositionFound)
	{
		globalPosition = pNode->EvaluateGlobalTransform(time);
	}

	return globalPosition;
}

FbxAMatrix GetPoseMatrix(FbxPose* pPose, int nodeIndex)
{
	FbxAMatrix poseMatrix;
	
	FbxMatrix matrix = pPose->GetMatrix(nodeIndex);

	memcpy((double*)poseMatrix, (double*)matrix, sizeof(matrix.mData));

	return poseMatrix;
}

FbxAMatrix GetGeometry(FbxNode* pNode)
{
	const FbxVector4 T = pNode->GetGeometricTranslation(FbxNode::eSourcePivot);
	const FbxVector4 R = pNode->GetGeometricRotation(FbxNode::eSourcePivot);
	const FbxVector4 S = pNode->GetGeometricScaling(FbxNode::eSourcePivot);

	return FbxAMatrix(T, R, S);
}