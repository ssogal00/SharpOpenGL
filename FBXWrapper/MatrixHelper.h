#pragma once


#include "fbxsdk.h"

void MatrixScale(FbxAMatrix& matrix, double value);

void MatrixAdd(FbxAMatrix& destMatrix, FbxAMatrix& sourceMatrix);

void MatrixAddToDiagonal(FbxAMatrix& destMatrix, double weight);