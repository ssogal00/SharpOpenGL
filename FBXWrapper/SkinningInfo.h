#pragma once

#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class SkinningInfo
	{
	public:
		int nControlPointIndex = 0;
		System::String^ BoneName = nullptr;
		float Weight = 0;		
	};
};
