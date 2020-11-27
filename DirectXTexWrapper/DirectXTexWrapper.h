#pragma once

#include "DirectXTex.h"

using namespace System;
using namespace System::Collections::Generic;

namespace DirectXTexWrapper
{
	public ref class ManagedTexMetaData
	{
	public:
		ManagedTexMetaData(const DirectX::TexMetadata& metaData);
		size_t          width;
		size_t          height;     // Should be 1 for 1D textures
		size_t          depth;      // Should be 1 for 1D or 2D textures
		size_t          arraySize;  // For cubemap, this is a multiple of 6
		size_t          mipLevels;
		uint32_t        miscFlags;
		uint32_t        miscFlags2;
		DXGI_FORMAT     format;
		DirectX::TEX_DIMENSION   dimension;
	};

	public ref class ManagedImage
	{
	public:
		ManagedImage(const DirectX::Image& image);
		size_t      width;
		size_t      height;
		DXGI_FORMAT format;
		size_t      rowPitch;
		size_t      slicePitch;
		array<uint8_t>^ pixels;
	};

	public ref class ManagedScratchImage
	{
	public:
		ManagedScratchImage(const DirectX::ScratchImage& scratchImage);
		size_t      m_nimages;
		size_t      m_size;
		ManagedTexMetaData^ m_metadata;
		array<ManagedImage^>^ m_image;
	};	
	
	public ref class DXTLoader
	{
		// TODO: Add your methods for this class here.
	public:
		ManagedScratchImage^ LoadFromDDSFile(System::String^ filePath);
		ManagedScratchImage^ LoadFromTGAFile(System::String^ filePath);
		ManagedScratchImage^ LoadFromHDRFile(System::String^ filePath);
	};
}
