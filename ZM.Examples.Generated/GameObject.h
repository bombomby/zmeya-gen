// ----------------------------------------------------------------------------
//		THIS CODE WAS GENERATED BY A TOOL
//		CHANGES TO THIS FILE WILL BE LOST IF THE CODE IS REGENERATED
//		Version: 1.0.0.0
//		Date: 01/24/2022 04:13:46
// ----------------------------------------------------------------------------
#pragma once
#include <cstdint>
#include "Position.h"
#include "Zmeya.h"

namespace zmdb
{
	struct GameObject 
	{
		Position Pos;
		uint32_t Health;
		zm::String Name;
		zm::Array<int32_t> Items;
		zm::Array<zm::String> Lines;
		zm::Pointer<GameObject> Target;
	};
	
}