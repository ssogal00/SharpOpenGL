#pragma once

using namespace System::Collections::Generic;

namespace ImageLibWrapper
{
	public ref class ImageInfo
	{
	public:
		ImageInfo()
		{			
		}

		int Width = 0;
		int Height = 0;
		int Channels = 0;
		int ChannelsOrder = 0;
	};

	public ref class ImageLibrary
	{
	public:
		array<float>^ LoadAsFloat(System::String^ path, ImageInfo^ outImageInfo);
		array<unsigned char>^ LoadAsByte(System::String^ path, ImageInfo^ outImageInfo);
	};

	
}