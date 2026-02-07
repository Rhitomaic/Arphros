using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace Arphros;

public class Game
{
    private const float fixedTimeStep = 1f / 60f;
    private float accumulator = 0f;

    private readonly List<Behaviour> behaviours = [];
    private CameraController camera = null!;

    public void Run()
    {
        Raylib.InitWindow(1280, 720, "Arphros");
        Raylib.SetTargetFPS(60);

        camera = new CameraController();
        Player player = new() { Transform = new Transform() { LocalPosition = new(0, 0, 0) } };

        camera.Target = player.Transform;

        behaviours.Add(camera);
        behaviours.Add(player);

        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();
            accumulator += dt;

            foreach (var b in behaviours)
                b.ProcessInput();

            while (accumulator >= fixedTimeStep)
            {
                foreach (var b in behaviours)
                    b.FixedUpdate(fixedTimeStep);

                accumulator -= fixedTimeStep;
            }

            foreach (var b in behaviours)
                b.Update(dt);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            Raylib.BeginMode3D(camera.GetCamera());

            foreach (var b in behaviours)
                b.Render();

            Raylib.EndMode3D();

            // 2D UI can go here

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public static Vector3 ForwardFromRotation(Vector3 rot)
    {
        float yaw = MathF.PI / 180f * rot.Y;
        float pitch = MathF.PI / 180f * rot.X;

        return new Vector3(
            MathF.Cos(pitch) * MathF.Sin(yaw),
            -MathF.Sin(pitch),
            MathF.Cos(pitch) * MathF.Cos(yaw)
        );
    }
}