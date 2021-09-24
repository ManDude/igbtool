add_library(common
        SHARED
        log.cpp
        print_float.cpp
        )

target_link_libraries(common fmt)

if(UNIX)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -O2")
elseif(WIN32)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /O2")
endif()

install(TARGETS common)
