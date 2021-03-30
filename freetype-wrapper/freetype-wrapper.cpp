#include "pch.h"
#include "freetype-wrapper.h"
#define generic __identifier(generic)
#include "ft2build.h"
#include "freetype/freetype.h"
#include "freetype/ftglyph.h"
#undef generic
#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <cmath>
using namespace msclr::interop;
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
#using <mscorlib.dll>


FreeTypeWrapper::FontAtlas::FontAtlas(array<unsigned char>^ bitmap, Dictionary<wchar_t, Glyph^>^ glyphMap, int squareSize)
{
	this->Bitmap = bitmap;
	this->GlyphMap = glyphMap;
	this->SquarePixelSize = squareSize;
}

FreeTypeWrapper::BBox::BBox()
{
	this->XMax = this->YMax = INT_MIN;
	this->XMin = this->YMin = INT_MAX;
}


FreeTypeWrapper::FontAtlas^ FreeTypeWrapper::FreeType::GetFontAtlas(System::String^ fontPath, System::String^ charSet, int pixelSize)
{
	FT_Library lib;

	FT_Init_FreeType(&lib);

	FT_Face face;

	FT_New_Face(lib, "C:\\windows\\fonts\\batang.ttc", 2, &face);

	int length = charSet->Length;

	int squareSize = ceil(sqrt(length));	

	std::wstring charSets = marshal_as<std::wstring>(charSet);

	array<uint8_t>^ resultPixels = gcnew array<uint8_t>((pixelSize * squareSize) * (pixelSize*squareSize));

	int error;

	error = FT_Set_Pixel_Sizes(face, pixelSize, pixelSize);

	Dictionary<wchar_t, Glyph^>^ glyphMap = gcnew Dictionary<wchar_t, Glyph^>();

	const int scanlineSize = pixelSize * squareSize;

	int enclosingMinX = INT_MAX;
	int enclosingMaxX = INT_MIN;
	int enclosingMinY = INT_MAX;
	int enclosingMaxY = INT_MIN;

	for(int i = 0; i < length; ++i)
	{
		const int col = i % squareSize;
		const int row = i / squareSize;
		
		unsigned int charIndex = FT_Get_Char_Index(face, charSets[i]);		
		
		error = FT_Load_Glyph(face, charIndex, FT_LOAD_DEFAULT | FT_LOAD_NO_BITMAP);

		error = FT_Render_Glyph(face->glyph, FT_RENDER_MODE_NORMAL);

		FT_Glyph testGlyph;
		
		error = FT_Get_Glyph(face->glyph, &testGlyph);

		FT_BBox glyphBox;
		
		FT_Glyph_Get_CBox(testGlyph, FT_GLYPH_BBOX_PIXELS, &glyphBox);

		Glyph^ newGlyph = gcnew Glyph();
		newGlyph->BitmapWidth = face->glyph->bitmap.width;
		newGlyph->BitmapHeight = face->glyph->bitmap.rows;
		newGlyph->BitmapLeft = face->glyph->bitmap_left;
		newGlyph->BitmapTop = face->glyph->bitmap_top;
		newGlyph->TexCoordLeftX = (pixelSize * col) / static_cast<float>(scanlineSize);
		newGlyph->TexCoordRightX = (pixelSize * col + face->glyph->bitmap.width) / static_cast<float>(scanlineSize);
		newGlyph->TexCoordTopY = (pixelSize * row) / static_cast<float>(scanlineSize);		
		newGlyph->TexCoordBottomY = (pixelSize * row + face->glyph->bitmap.rows) / static_cast<float>(scanlineSize);
		newGlyph->AdvanceX = face->glyph->advance.x;
		newGlyph->AdvanceY = face->glyph->advance.y;
		newGlyph->Box = gcnew BBox();
		newGlyph->Box->XMin = glyphBox.xMin;
		newGlyph->Box->XMax = glyphBox.xMax;
		newGlyph->Box->YMin = glyphBox.yMin;
		newGlyph->Box->YMax = glyphBox.yMax;

		// update enclosing box
		if(glyphBox.xMin < enclosingMinX)
		{
			enclosingMinX = glyphBox.xMin;
		}

		if(glyphBox.xMax > enclosingMaxX)
		{
			enclosingMaxX = glyphBox.xMax;
		}

		if(glyphBox.yMin < enclosingMinY)
		{
			enclosingMinY = glyphBox.yMin;
		}

		if(glyphBox.yMax > enclosingMaxY)
		{
			enclosingMaxY = glyphBox.yMax;
		}
		
		glyphMap->Add(charSets[i], newGlyph);

		const int dstStartIndex = (pixelSize*pixelSize*squareSize) * row + (pixelSize * pixelSize) * col;
		
		for (int y = 0; y < face->glyph->bitmap.rows; y++)
		{
			for (int x = 0; x < face->glyph->bitmap.width; x++)
			{
				const int dstX = col * pixelSize + x;
				const int dstY = scanlineSize * (row * pixelSize + y);

				const int dstIndex = dstY + dstX;
				
				const int srcIndex = y * face->glyph->bitmap.width + x;
				resultPixels[dstIndex] = face->glyph->bitmap.buffer[srcIndex];
			}
		}
	}

	BBox^ box = gcnew BBox();
	box->XMin = face->bbox.xMin;
	box->XMax = face->bbox.xMax;
	box->YMax = face->bbox.yMax;
	box->YMin = face->bbox.yMin;
		
	FT_Done_FreeType(lib);

	FontAtlas^ result = gcnew FontAtlas(resultPixels, glyphMap, pixelSize * squareSize);
	
	return result;
}
 