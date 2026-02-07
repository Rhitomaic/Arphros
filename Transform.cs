using System.Numerics;
using System.Collections.Generic;
using System;

namespace Arphros;

public class Transform
{
    public Vector3 LocalPosition;
    public Vector3 LocalRotation;
    public Vector3 LocalScale = Vector3.One;

    public Transform? Parent;
    public List<Transform> Children = [];

    private const float Deg2Rad = MathF.PI / 180f;

    public Matrix4x4 GetLocalMatrix()
    {
        // Order is critical in 3D: Scale first, then Rotate, then Translate
        return Matrix4x4.CreateScale(LocalScale) *
               Matrix4x4.CreateFromYawPitchRoll(
                   LocalRotation.Y * Deg2Rad, 
                   LocalRotation.X * Deg2Rad, 
                   LocalRotation.Z * Deg2Rad) *
               Matrix4x4.CreateTranslation(LocalPosition);
    }

    public Matrix4x4 GetWorldMatrix()
    {
        if (Parent == null)
        {
            return GetLocalMatrix();
        }

        // Multiply Parent World Matrix by our Local Matrix
        // This effectively "adds" the transformations together
        return GetLocalMatrix() * Parent.GetWorldMatrix();
    }

    // Helper to get actual World Position as a Vector3
    public Vector3 WorldPosition => GetWorldMatrix().Translation;

    public void SetParent(Transform? newParent)
    {
        Parent?.Children.Remove(this);
        Parent = newParent;
        Parent?.Children.Add(this);
    }
}