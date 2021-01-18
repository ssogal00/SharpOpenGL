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
	this->format = static_cast<DirectXTexWrapper::BRIDGE_DXGI_FORMAT>(metaData.format);
	this->dimension = static_cast<DirectXTexWrapper::TEX_DIMENSION>(metaData.dimension);
	this->miscFlags = metaData.miscFlags;
	this->miscFlags2 = metaData.miscFlags2;	
}

bool DirectXTexWrapper::ManagedTexMetaData::IsCubemap()
{
	return (miscFlags & (uint32_t) TEX_MISC_FLAG::TEX_MISC_TEXTURECUBE) != 0;
}

DirectXTexWrapper::ManagedScratchImage::ManagedScratchImage(const DirectX::ScratchImage& scratchImage)
{
	
	m_metadata = gcnew ManagedTexMetaData(scratchImage.GetMetadata());
	m_nimages = scratchImage.GetImageCount();
	m_image = gcnew array<ManagedImage^>(m_nimages);	
	
	for (int i = 0; i < m_nimages; ++i)
	{		
		m_image[i] = gcnew ManagedImage(scratchImage.GetImages()[i]);
	}
}

DirectXTexWrapper::ManagedImage::ManagedImage(const DirectX::Image& image)
{
	this->width = image.width;
	this->height = image.height;
	this->format = static_cast<DirectXTexWrapper::BRIDGE_DXGI_FORMAT>(image.format);
	this->rowPitch = image.rowPitch;
	this->slicePitch = image.slicePitch;
	pixels = gcnew array<uint8_t>(image.slicePitch);	
	
	Marshal::Copy(IntPtr((void*)image.pixels), this->pixels, 0, image.slicePitch);
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

DirectXTexWrapper::ManagedScratchImage^ DirectXTexWrapper::DXTLoader::LoadFromTGAFile(System::String^ filePath)
{
	std::wstring FileName = marshal_as<std::wstring>(filePath);

	DirectX::TexMetadata metaData;
	DirectX::ScratchImage image;

	HRESULT result = DirectX::LoadFromTGAFile(FileName.c_str(), DirectX::TGA_FLAGS_NONE, &metaData, image);

	if (result != S_OK)
	{
		return nullptr;
	}	

	return gcnew ManagedScratchImage(image);
}


DirectXTexWrapper::ManagedScratchImage^ DirectXTexWrapper::DXTLoader::LoadFromHDRFile(System::String^ filePath)
{
	std::wstring FileName = marshal_as<std::wstring>(filePath);

	DirectX::TexMetadata metaData;
	DirectX::ScratchImage image;

	HRESULT result = DirectX::LoadFromHDRFile(FileName.c_str(), &metaData, image);

	if (result != S_OK)
	{
		return nullptr;
	}	

	return gcnew ManagedScratchImage(image);
}

DirectXTexWrapper::ManagedScratchImage^ DirectXTexWrapper::DXTLoader::LoadFromJPGFile(System::String^ filePath)
{
	std::wstring FileName = marshal_as<std::wstring>(filePath);

	DirectX::TexMetadata metaData;
	DirectX::ScratchImage image;

	HRESULT result = DirectX::LoadFromWICFile(FileName.c_str(), DirectX::WIC_FLAGS_NONE, &metaData, image, nullptr);

	if (result != S_OK)
	{
		return nullptr;
	}

	return gcnew ManagedScratchImage(image);
}

bool DirectXTexWrapper::DXTSaver::SaveAsDDSFile(System::String^ filePath, ManagedImage^ managedImage)
{
	std::wstring fileName = marshal_as<std::wstring>(filePath);
	
	DirectX::Image image;
	image.width = managedImage->width;
	image.height = managedImage->height;
	image.slicePitch = managedImage->slicePitch;
	image.rowPitch = managedImage->rowPitch;
	image.format = (DXGI_FORMAT) (managedImage->format);

	Marshal::Copy(IntPtr((void*)image.pixels), managedImage->pixels, 0, managedImage->slicePitch);	
	
	HRESULT result = DirectX::SaveToDDSFile(image, DirectX::DDS_FLAGS_NONE, fileName.c_str());	
	
	if(result != S_OK)
	{
		return false;
	}

	return true;
}

bool DirectXTexWrapper::DXTSaver::SaveAsTGAFile(System::String^ filePath, ManagedImage^ managedImage)
{
	return true;
}

bool DirectXTexWrapper::DXTSaver::SaveAsJPGFile(System::String^ filePath, ManagedImage^ image)
{
	return true;
}

