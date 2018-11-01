#include "FreeTypeLibWrapper.h"



#include "FreeTypeLibWrapper.h"
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;

#using <mscorlib.dll>


bool FreeTypeLibWrapper::FreeType::Initialize(System::String^ filePath)
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

	if (FT_New_Face(testLib, FileName.c_str(), 0, &fontFace) != 0)
	{		
		return false;
	}

	bool bUseKerning = FT_HAS_KERNING(fontFace);



	return true;
}