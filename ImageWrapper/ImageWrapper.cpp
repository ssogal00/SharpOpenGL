// ImageWrapper.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#define STB_IMAGE_IMPLEMENTATION

#include "stb_image.h"
#include <iostream>
#include "ImageWrapper.h"
#include <msclr/marshal_cppstd.h>
using namespace msclr::interop;
using namespace msclr::interop::details;
using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

array<float>^ ImageLibWrapper::ImageLibrary::LoadAsFloat(System::String^ path, ImageInfo^ outImageInfo)
{
	std::string FileName = marshal_as<std::string>(path);

	int width=0, height=0, channels=0;

	float* data = stbi_loadf(FileName.c_str(), &width, &height, &channels, 0);	

	array<float>^ returnValue = gcnew array<float>(width * height * channels);

	Marshal::Copy(IntPtr((void*)data), returnValue, 0, width* height* channels);

	outImageInfo->Width = width;
	outImageInfo->Height = height;
	outImageInfo->Channels = channels;
	
	return returnValue;
}

array<unsigned char>^ ImageLibWrapper::ImageLibrary::LoadAsByte(System::String^ path, ImageInfo^ outImageInfo)
{
	std::string FileName = marshal_as<std::string>(path);

	int width = 0, height = 0, channels = 0;
	int channels_order = 0;

	stbi_uc* data = stbi_load(FileName.c_str(), &width, &height, &channels, 0);

	array<unsigned char>^ returnValue = gcnew array<unsigned char>(width * height * channels);

	Marshal::Copy(IntPtr((void*)data), returnValue, 0, width* height* channels);

	outImageInfo->Width = width;
	outImageInfo->Height = height;
	outImageInfo->Channels = channels;
	outImageInfo->ChannelsOrder = channels_order;

	return returnValue;
}
