#include "FreeTypeLibWrapper.h"

#include "ft2build.h"
#include "freetype/ftglyph.h"
#include "freetype/freetype.h"
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;

#using <mscorlib.dll>


void FreeTypeLibWrapper::FreeType::Initialize(System::String^ filePath)
{
	FT_Library Library;

	if(FT_Init_FreeType(&Library))
	{
		//
		return;
	}


}