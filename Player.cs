using Raylib_cs;

namespace Arphros;

public class Player : Behaviour
{
    public Transform Transform = new();
    public float Speed = 10f;

    public override void ProcessInput()
    {
        if (Raylib.IsKeyDown(KeyboardKey.A))
            Transform.LocalPosition.X -= Speed * Raylib.GetFrameTime();

        if (Raylib.IsKeyDown(KeyboardKey.D))
            Transform.LocalPosition.X += Speed * Raylib.GetFrameTime();
    }

    public override void Update(float dt)
    {
    }

    public override void FixedUpdate(float fixedDt)
    {
    }

    public override void Render()
    {
        Raylib.DrawCube(
            Transform.WorldPosition,
            1, 1, 1,
            Color.Red
        );
    }
}