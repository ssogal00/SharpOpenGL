#include "pch.h"

#include "FreeTypeGLWrapper.h"

#include <string>
#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <cmath>
using namespace msclr::interop;
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
#using <mscorlib.dll>

FreeTypeGLWrapper::ManagedTextureAtlas^ FreeTypeGLWrapper::FreeTypeGL::GenerateTextureAtlas(int width, int height, int fontsize, String^ fontpath)
{
	texture_atlas_t* pAtlas = texture_atlas_new(width, height, 1);

	if(pAtlas == nullptr)
	{
		return nullptr;
	}

	std::string fontFilePath = msclr::interop::marshal_as<std::string>(fontpath);	

	texture_font_t* pFont = texture_font_new_from_file(pAtlas, 72, fontFilePath.c_str());

	if(pFont == nullptr)
	{
		return nullptr;
	}

	pFont->rendermode = RENDER_SIGNED_DISTANCE_FIELD;

	const char* cache = " !\"#$%&'()*+,-./0123456789:;<=>?"
		"@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_"
		"`abcdefghijklmnopqrstuvwxyz{|}~";

	int returncode = texture_font_load_glyphs(pFont, cache);

	texture_font_delete(pFont);

	ManagedTextureAtlas^ result = gcnew ManagedTextureAtlas();

	result->height = pAtlas->height;
	result->width = pAtlas->width;
	result->used = pAtlas->used;

	int length = pAtlas->width * pAtlas->height;

	result->data = gcnew array<uint8_t>(pAtlas->width * pAtlas->height);
	
	Marshal::Copy(IntPtr((void*)pAtlas->data), result->data, 0, length);	

	return result;
}
