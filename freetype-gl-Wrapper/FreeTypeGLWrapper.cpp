#include "pch.h"

#include "FreeTypeGLWrapper.h"

#include <string>
#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <cmath>
using namespace msclr::interop;
using namespace System;
using namespace System::Collections::Generic;
#using <mscorlib.dll>

FreeTypeGLWrapper::ManagedTextureAtlas^ FreeTypeGLWrapper::FreeTypeGL::GenerateTextureAtlas(int width, int height, int fontsize, String^ fontpath)
{
	/*texture_atlas_t* pAtlas = texture_atlas_new(width, height, 1);

	std::string fontFilePath = msclr::interop::marshal_as<std::string>(fontpath);	

	texture_font_t* pFont = texture_font_new_from_file(pAtlas, 72, fontFilePath.c_str());

	pFont->rendermode = RENDER_SIGNED_DISTANCE_FIELD;

	const char* cache = " !\"#$%&'()*+,-./0123456789:;<=>?"
		"@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_"
		"`abcdefghijklmnopqrstuvwxyz{|}~";

	texture_font_load_glyph(pFont, cache);

	texture_font_delete(pFont);

	ManagedTextureAtlas^ result = gcnew ManagedTextureAtlas();

	result->height = pAtlas->height;
	result->width = pAtlas->width;

	return result;*/
	return nullptr;
}
