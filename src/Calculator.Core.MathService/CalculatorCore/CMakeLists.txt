cmake_minimum_required(VERSION 3.0)

project(cpp LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)

add_library(calculatorcore SHARED calculator.cc model_maths.cc model_token.cc)
target_include_directories(calculatorcore PRIVATE .)
add_compile_options(calculatorcore -O2)

if (WIN32)
    set(CMAKE_SHARED_LIBRARY_PREFIX "")
else ()
    set(CMAKE_SHARED_LIBRARY_PREFIX "lib")
endif ()

add_custom_command(TARGET calculatorcore POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_if_different
        $<TARGET_FILE_DIR:calculatorcore>/$<TARGET_FILE_NAME:calculatorcore>
        "${PROJECT_SOURCE_DIR}/../${CMAKE_SHARED_LIBRARY_PREFIX}$<TARGET_NAME:calculatorcore${CMAKE_SHARED_LIBRARY_SUFFIX}>")