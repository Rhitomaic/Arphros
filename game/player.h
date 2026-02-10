#pragma once
#include "raylib.h"
#include "../engine/component.h"

namespace Arphros {
    class Player : public Component
    {
    private:
        bool isAlive;
        bool isStarted;
        int turnIndex;
        Vector3 turnRotations[2];
    public:
        Player();
        ~Player();

        void Turn();
        GameObject* CreateTail();
    };
}