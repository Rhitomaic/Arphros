#pragma once
#include "raylib.h"

namespace Arphros {
    class Core {
    public:
        Core(int screenWidth, int screenHeight, const char* title);
        ~Core();

        void Run();
    };
}