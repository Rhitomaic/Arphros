using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;
using System;

namespace Arphros;

public class Player : Behaviour
{
    public float Speed = 5f;
    private bool _started = false;

    public List<Transform> Tails = new();
    public Transform? CurrentTail;
    private int Index;

    public override void ProcessInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (!_started)
            {
                _started = true;
                CreateTail();
            }
            else
            {
                Turn();
                CreateTail();
            }
        }
    }

    private void Turn()
    {
        if (Index % 2 == 0)
        {
            Transform.LocalEulerAngles += new Vector3(0, 90, 0);
        }
        else
        {
            Transform.LocalEulerAngles += new Vector3(0, -90, 0);
        }
        Index++;
    }

    public override void Update(float dt)
    {
        if (!_started) return;

        Transform.Position += Transform.Forward * Speed * dt;

        if (CurrentTail != null)
        {
            CurrentTail.Position += CurrentTail.Forward * Speed * dt * 0.5f;
            CurrentTail.LocalScale += new Vector3(0, 0, Speed * dt);
        }
    }

    private void CreateTail()
    {
        var tail = new Transform
        {
            Position = Transform.Position,
            Rotation = Transform.Rotation,
            LocalScale = Transform.LocalScale
        };

        Tails.Add(tail);
        CurrentTail = tail;
    }

    public override void Render()
    {
        DrawTransform(MeshLibrary.Cube, Transform, Color.Red);

        foreach (var tail in Tails)
            DrawTransform(MeshLibrary.Cube, tail, Color.Green);
    }

    /// <summary>
    /// A ridiculous function to draw a transform with a model since Raylib-cs doesn't expose
    /// the gl functions directly.
    /// </summary>
    private static void DrawTransform(Model model, Transform t, Color color)
    {
        Quaternion q = t.Rotation;
        q = Quaternion.Normalize(q);

        float angle = 2f * MathF.Acos(q.W) * (180f / MathF.PI);
        Vector3 axis = new(q.X, q.Y, q.Z);

        if (axis.LengthSquared() < 0.0001f)
            axis = Vector3.UnitY;
        else
            axis = Vector3.Normalize(axis);

        Raylib.DrawModelEx(
            model,
            t.Position,
            axis,
            angle,
            t.Scale,
            color
        );
    }

}