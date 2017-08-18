#pragma once

#using <OpenTK.dll>

using namespace System::Collections::Generic;

namespace FBXWrapper
{
	public ref class ParsedFBXAnimStack
	{
	public :
		System::String^ StackName;
		int NumOfLayers;
	};

};