#pragma once



using namespace System::Collections::Generic;

namespace FreeTypeWrapper
{
	public ref class FreeType
	{
	public:
		static array<unsigned char>^ GetFontAtlas(System::String^ fontPath, System::String^ charSet);
	protected:

	private:
	};
};
