# -----------------------------------------------------------------------------
#		THIS CODE WAS GENERATED BY A TOOL
#		CHANGES TO THIS FILE WILL BE LOST IF THE CODE IS REGENERATED
#		Version: 1.0.0.0
#		Date: 01/24/2022 03:39:05
# -----------------------------------------------------------------------------
cmake_minimum_required(VERSION 3.12)
project(ZM.Examples.Generated)
file(GLOB GENERATED_SRC CONFIGURE_DEPENDS "*.h" "*.cpp")
add_library(ZM.Examples.Generated ${GENERATED_SRC})
target_include_directories(ZM.Examples.Generated PUBLIC include PRIVATE "../Zmeya")

