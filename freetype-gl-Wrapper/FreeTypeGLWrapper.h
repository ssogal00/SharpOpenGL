#pragma once

#include "freetype-gl.h"

using namespace System;

namespace FreeTypeGLWrapper
{	
    public ref class ManagedTextureAtlas
    {
        /**
         *  Width (in pixels) of the underlying texture
         */
        size_t width;

        /**
         * Height (in pixels) of the underlying texture
         */
        size_t height;

        /**
         * Depth (in bytes) of the underlying texture
         */
        size_t depth;

        /**
         * Allocated surface size
         */
        size_t used;

        /**
         * Atlas data
         */
        array<unsigned char>^ data;        

    };

	public ref class FreeTypeGL
	{
	public:
        ManagedTextureAtlas^ GenerateTextureAtlas(int width, int height, int fontsize, String^ fontpath);
		// TODO: Add your methods for this class here.
	};
}
