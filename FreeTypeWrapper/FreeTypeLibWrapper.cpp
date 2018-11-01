#include "FreeTypeLibWrapper.h"

#define generic GenericFromFreeTypeLibrary
#include "ft2build.h"
#include "freetype/ftglyph.h"
#undef generic

#include "FreeTypeLibWrapper.h"
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