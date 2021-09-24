add_executable(igbtool
        igbtool.cpp
        igb.cpp)

target_link_libraries(igbtool
        common
        fmt)

install(TARGETS igbtool)
