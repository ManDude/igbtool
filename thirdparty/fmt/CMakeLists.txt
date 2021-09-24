if (UNIX)
  set(CMAKE_CXX_FLAGS "-O2")
else ()
  set(CMAKE_CXX_FLAGS "/O2 /EHsc")
endif (UNIX)

include_directories(../)
add_library(fmt SHARED format.cc)
target_compile_definitions(fmt PRIVATE FMT_EXPORT INTERFACE FMT_SHARED PUBLIC FMT_SHARED)
