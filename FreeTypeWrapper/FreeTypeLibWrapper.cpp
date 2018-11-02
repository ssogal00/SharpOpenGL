
#include "FreeTypeLibWrapper.h"
#include "GlyphInfo.h"
#include "Windows.h"
#include <msclr/marshal_cppstd.h>
using namespace msclr::interop;
using namespace System;
using namespace System::Collections::Generic;
#using <mscorlib.dll>


bool FreeTypeLibWrapper::FreeType::Initialize(System::String^ filePath, int _resolution, System::String^ characters)
{
	FT_Library testLib;

	FT_Face fontFace;

	FT_Error err = FT_Init_FreeType(&testLib);
	if(err != 0)
	{
		//
		return false;
	}

	std::string FileName = marshal_as<std::string>(filePath);
	const int numGlyph = characters->Length;

	if (FT_New_Face(testLib, FileName.c_str(), 0, &fontFace) != 0)
	{		
		return false;
	}

	bool bUseKerning = FT_HAS_KERNING(fontFace);

	int calculatedResolution;
	int calculatedMargin;

	resolution = _resolution;

	if(GetCorrectResolution(resolution, numGlyph,calculatedResolution, calculatedMargin))
	{
		squareSize = static_cast<float>(calculatedResolution + calculatedMargin);

		FT_Set_Pixel_Sizes(fontFace, calculatedResolution, calculatedResolution);

		const int numGlyphs = numGlyph;

		const int NumGlyphsPerRow = (int)ceilf(std::sqrtf(static_cast<float>(numGlyphs))); //=numRows (texture is a square)
		const int TexSize = static_cast<int>((NumGlyphsPerRow) * squareSize);

		//
		int RealTexSize = NextPowerOf2(TexSize);
		realTextureSize = RealTexSize;		
		textureDimension = (squareSize) / (float)RealTexSize;

		textureData->Capacity = RealTexSize * RealTexSize * 2;
		for(int i =0 ; i< RealTexSize * RealTexSize * 2;++i)
		{
			textureData->Add(0);
		}

		int texAtlasX = 0;
		int texAtlasY = 0;
		FT_UInt gindex = 0;
		
		for (FT_ULong charcode = FT_Get_First_Char(fontFace, &gindex); gindex != 0; charcode = FT_Get_Next_Char(fontFace, charcode, &gindex))		
		{
			if(charcode > 126)
			{
				continue;
			}

			FT_Error loadErr = FT_Load_Glyph(fontFace, gindex, FT_LOAD_DEFAULT);
			if (loadErr != 0)
			{
				continue;
			}

			FT_GlyphSlot glyph = fontFace->glyph;
			
			FT_Render_Glyph(glyph, FT_RENDER_MODE_NORMAL);

			//Calculate glyph informations
			GlyphInfo^ glyphInfo = gcnew GlyphInfo();
			glyphInfo->CharCode = charcode;
			// Get texture offset in the image
			glyphInfo->AtlasX = (texAtlasX*squareSize) / (float)RealTexSize;
			glyphInfo->AtlasY = (texAtlasY*squareSize) / (float)RealTexSize;
			glyphInfo->HoriBearingX = glyph->metrics.horiBearingX;
			glyphInfo->HoriBearingY = glyph->metrics.horiBearingY;
			glyphInfo->HoriAdvance = glyph->metrics.horiAdvance;
			glyphInfo->Width = glyph->metrics.width;
			glyphInfo->Height = glyph->metrics.height;

			//advance is stored in fractional pixel format (=1/64 pixels), as per free type specifications
			glyphInfo->AdvanceHorizontal = static_cast<float>(glyph->advance.x);
			glyphInfo->AdvanceVertical = static_cast<float>(glyph->advance.y);
			glyphMap->Add(charcode, glyphInfo);


			
			//Copy the bitmap to the atlas
			GenerateTextureFromGlyph(glyph, texAtlasX, texAtlasY, RealTexSize, calculatedResolution, calculatedMargin, false);

			texAtlasX++;
			if (texAtlasX >= NumGlyphsPerRow)
			{
				texAtlasX = 0;
				texAtlasY++;
			}
		}
	}

	FT_Done_FreeType(testLib);

	return true;
}

FreeTypeLibWrapper::GlyphInfo^ FreeTypeLibWrapper::FreeType::GetGlyphInfo(unsigned long charCode)
{
	if (glyphMap->ContainsKey(charCode))
	{
		return glyphMap[charCode];
	}
	else
	{
		return nullptr;
	}
}

void FreeTypeLibWrapper::FreeType::GenerateTextureFromGlyph(FT_GlyphSlot glyph, int atlasX, int atlasY, int texSize, int resolution, int marginSize, bool drawBorder)
{
	const int squareSize = resolution + marginSize;
	const int baseOffset = atlasX * squareSize + atlasY * squareSize*texSize;

	if (drawBorder)
	{
		for (int w = 0; w < squareSize; w++)
		{
			SetPixel(baseOffset, texSize, w, 0, 255);
		}

		for (int h = 1; h < squareSize; h++)
		{
			for (int w = 0; w < squareSize; w++)
			{
				SetPixel(baseOffset, texSize, w, h, (w == 0 || w == squareSize - 1) ? 255 : (h == squareSize - 1) ? 255 : 0);
			}
		}
	}

	const int gr = glyph->bitmap.rows;
	const int gw = glyph->bitmap.width;
	
	for (int h = 0; h < gr; h++)
	{
		for (int w = 0; w < gw; w++)
		{
			SetPixel(baseOffset + marginSize, texSize, w, marginSize + h, glyph->bitmap.buffer[w + h * gw]);
		}
	}
}

void FreeTypeLibWrapper::FreeType::SetPixel(int offset, int size, int x, int y, unsigned char val)
{
	textureData[2 * (offset + x + y * size)] = val;
	textureData[2 * (offset + x + y * size) + 1] = val;
}

int FreeTypeLibWrapper::FreeType::NextPowerOf2(const int n)
{
	int ReturnValue = 1;
	while (ReturnValue < n)
	{
		ReturnValue <<= 1;
	}
	return ReturnValue;
}

bool FreeTypeLibWrapper::FreeType::GetCorrectResolution(int resolution, int numglyph, int& outNewResolution, int& outNewMargin)
{
	int GlyphMargin = 0;

	int MaxTextureSize = 4096;

	while (resolution > 0)
	{
		GlyphMargin = static_cast<int> (ceil(resolution * 0.1f));

		const auto NumGlyph = numglyph;

		const auto SquareSize = resolution + GlyphMargin;

		const int NumGlyphsPerRow = static_cast<int>(ceilf(sqrt(NumGlyph)));

		const int TexSize = NumGlyphsPerRow * SquareSize;

		int RealTexSize = NextPowerOf2(TexSize);

		if (RealTexSize <= MaxTextureSize)
		{
			break;
		}

		resolution = resolution - 5;
	}

	if (resolution > 0)
	{
		outNewResolution = resolution;
		outNewMargin = GlyphMargin;
		return true;
	}
	else
	{
		return false;
	}
}
