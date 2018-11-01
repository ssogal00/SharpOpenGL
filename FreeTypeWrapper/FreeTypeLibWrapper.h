#pragma once
#define generic GenericFromFreeTypeLibrary
#include "ft2build.h"
#include "freetype/ftglyph.h"
#undef generic

using namespace System::Collections::Generic;

namespace FreeTypeLibWrapper
{
	public ref class FreeType
	{
	public:
		bool Initialize(System::String^ filePath);

	private:
		
	};
}