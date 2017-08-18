#pragma once

namespace FBXWrapper
{
	public enum class InterpolationMethod : int
	{
		EInterpolationUnknown = 0,
		EInterpolationConstant = 0x00000002,	//!< Constant value until next key.
		EInterpolationLinear = 0x00000004,		//!< Linear progression to next key.
		EInterpolationCubic = 0x00000008		//!< Cubic progression to next key.
	};
};
