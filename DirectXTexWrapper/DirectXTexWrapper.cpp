#include "pch.h"
#include "DirectXTexWrapper.h"
#include <msclr/marshal_cppstd.h>
using namespace msclr::interop;
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
#using <mscorlib.dll>

DirectXTexWrapper::ManagedTexMetaData::ManagedTexMetaData(const DirectX::TexMetadata& metaData)
{
	this->width = metaData.width;
	this->height = metaData.height;
	this->arraySize = metaData.arraySize;
	this->mipLevels = metaData.mipLevels;
	this->depth = metaData.depth;
	this->format = metaData.format;
	this->dimension = metaData.dimension;
	this->miscFlags = metaData.miscFlags;
	this->miscFlags2 = metaData.miscFlags2;
	
}

DirectXTexWrapper::ManagedScratchImage^ DirectXTexWrapper::DXTLoader::LoadFromDDSFile(System::String^ filePath)
{
	std::wstring FileName = marshal_as<std::wstring>(filePath);

	DirectX::TexMetadata metaData;
	DirectX::ScratchImage image;

	HRESULT result = DirectX::LoadFromDDSFile(FileName.c_str(), DirectX::DDS_FLAGS_NONE, &metaData, image );

	if(result != S_OK)
	{
		return nullptr;
	}

	return gcnew ManagedScratchImage(image);
}


DirectXTexWrapper::ManagedScratchImage::ManagedScratchImage(const DirectX::ScratchImage& scratchImage)
{
	m_metadata = gcnew ManagedTexMetaData(scratchImage.GetMetadata());
	m_nimages = scratchImage.GetImageCount();
	m_image = gcnew array<ManagedImage^>(m_nimages);
	
	for(int i = 0; i < m_nimages; ++i)
	{
		m_image[i] = gcnew ManagedImage(scratchImage.GetImages()[i]);
	}
}

DirectXTexWrapper::ManagedImage::ManagedImage(const DirectX::Image& image)
{
	this->width = image.width;
	this->height = image.height;
	this->format = image.format;
	this->rowPitch = image.rowPitch;
	this->slicePitch = image.slicePitch;	
	pixels = gcnew array<uint8_t>(image.rowPitch);
	Marshal::Copy(IntPtr((void*)image.pixels), this->pixels, 0, image.rowPitch);
}
