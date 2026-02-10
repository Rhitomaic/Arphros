#pragma once
#include "../component.h"
#include "raylib.h"
#include "raymath.h"

namespace Arphros {

class Camera : public Component {
   public:
    Camera();
    ~Camera() = default;

    Camera3D& GetCamera() { return camera; }

    void SetFOV(float fovy);
    void SetProjection(int projectionType);

    void UpdateCamera();

   private:
    Camera3D camera;
};

}  // namespace Arphros
