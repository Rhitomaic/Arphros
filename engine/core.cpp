#include "core.h"
#include "game_object.h"
#include "components/camera.h"

namespace Arphros {
Core::Core(int screenWidth, int screenHeight, const char* title) {
    InitWindow(screenWidth, screenHeight, title);
    SetWindowState(FLAG_WINDOW_RESIZABLE | FLAG_VSYNC_HINT);
}

Core::~Core() {
    CloseWindow();
}

void Core::Run() {
    Vector3 cubePosition = {0.0f, 0.0f, 0.0f};
    GameObject* playerObj = new GameObject();
    playerObj->SetName("Player");

    GameObject* camObj = new GameObject();
    camObj->SetName("Main Camera");

    auto* camera = camObj->AddComponent<Camera>();

    camObj->transform.SetPosition({0, 2, 10});
    camObj->transform.SetEulerAngles({-45, 180, 0});

    while (!WindowShouldClose()) {
        BeginDrawing();
        ClearBackground(RAYWHITE);

        camera->UpdateCamera();
        BeginMode3D(camera->GetCamera());

        DrawCube(cubePosition, 2.0f, 2.0f, 2.0f, RED);
        DrawCubeWires(cubePosition, 2.0f, 2.0f, 2.0f, MAROON);

        DrawGrid(100, 1.0f);

        EndMode3D();

        DrawText("Welcome to the third dimension!", 10, 40, 20, DARKGRAY);

        DrawFPS(10, 10);
        EndDrawing();
    }

    delete playerObj;
    delete camObj;
}
}  // namespace Arphros