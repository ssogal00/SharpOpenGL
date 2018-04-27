
#include "MatrixHelper.h"

void MatrixScale(FbxAMatrix& matrix, double value)
{
	for(int i =0; i < 4; i++)
	{
		for(int j =0; j < 4; j++)
		{
			matrix[i][j] *= value;
		}
	}
}

void MatrixAdd(FbxAMatrix& destMatrix, FbxAMatrix& sourceMatrix)
{
	for(int i = 0; i < 4; i++)
	{
		for(int j = 0; j < 4; j++)
		{
			destMatrix[i][j] += sourceMatrix[i][j];
		}
	}
}

void MatrixAddToDiagonal(FbxAMatrix& matrix, double value)
{
	matrix[0][0] += value;
	matrix[1][1] += value;
	matrix[2][2] += value;
	matrix[3][3] += value;
}
