#include "camera.h"
#include "../transform.h"

namespace Arphros {
Camera::Camera() {
    camera.fovy = 45.0f;
    camera.projection = CAMERA_PERSPECTIVE;
}

void Camera::SetFOV(float fovy) {
    camera.fovy = fovy;
}

void Camera::SetProjection(int projectionType) {
    camera.projection = projectionType;
}

void Camera::UpdateCamera() {
    Vector3 pos = transform->GetPosition();
    Quaternion rot = transform->GetRotation();

    Vector3 forward = Vector3RotateByQuaternion({0, 0, -1}, rot);
    Vector3 up = Vector3RotateByQuaternion({0, 1, 0}, rot);

    camera.position = pos;
    camera.target = Vector3Add(pos, forward);
    camera.up = up;
}
}
