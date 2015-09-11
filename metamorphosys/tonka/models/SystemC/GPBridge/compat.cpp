#include "compat.h"

#if defined(_WIN32)
#pragma comment(lib, "ws2_32.lib")

static bool perfCountFreqInitialized = false;
static LARGE_INTEGER perfCountFreq;

int64_t nanosec_timer()
{
    LARGE_INTEGER now;
    QueryPerformanceCounter(&now);

    if (!perfCountFreqInitialized) {
        QueryPerformanceFrequency(&perfCountFreq);
        perfCountFreqInitialized = true;
    }

    return (now.QuadPart * 1000000000ll / perfCountFreq.QuadPart);
}

#elif defined(__linux) || defined(linux)

int64_t nanosec_timer()
{
    struct timespec now;
    clock_gettime(CLOCK_MONOTONIC, &now);
    return now.tv_sec * 1000000000ll + now.tv_nsec;
}

#endif
