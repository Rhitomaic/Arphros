using System;
using System.Collections.Generic;
using System.Numerics;

namespace Arphros;

public class Transform
{
    private Vector3 _localPosition;
    private Quaternion _localRotation = Quaternion.Identity;
    private Vector3 _localScale = Vector3.One;

    public Vector3 LocalPosition
    {
        get => _localPosition;
        set { _localPosition = value; MarkDirty(); }
    }

    public Quaternion LocalRotation
    {
        get => _localRotation;
        set { _localRotation = value; MarkDirty(); }
    }

    public Vector3 LocalEulerAngles
    {
        get => ToEuler(_localRotation);
        set { _localRotation = FromEuler(value); MarkDirty(); }
    }

    public Vector3 LocalScale
    {
        get => _localScale;
        set { _localScale = value; MarkDirty(); }
    }

    public Vector3 Position
    {
        get => WorldMatrix.Translation;
        set
        {
            if (Parent == null)
            {
                LocalPosition = value;
            }
            else
            {
                Matrix4x4.Invert(Parent.WorldMatrix, out var invParent);
                LocalPosition = Vector3.Transform(value, invParent);
            }
        }
    }

    public Quaternion Rotation
    {
        get
        {
            if (Parent == null)
                return _localRotation;
            return Quaternion.Normalize(
                Parent.Rotation * _localRotation
            );
        }
        set
        {
            if (Parent == null)
                LocalRotation = value;
            else
                LocalRotation = Quaternion.Normalize(
                    Quaternion.Inverse(Parent.Rotation) * value
                );
        }
    }

    public Vector3 EulerAngles
    {
        get => ToEuler(Rotation);
        set => Rotation = FromEuler(value);
    }

    public Vector3 Scale
    {
        get
        {
            if (Parent == null)
                return _localScale;
            return Parent.Scale * _localScale;
        }
        set
        {
            if (Parent == null)
                LocalScale = value;
            else
                LocalScale = value / Parent.Scale;
        }
    }

    public Transform? Parent { get; private set; }
    public List<Transform> Children { get; } = new();

    public void SetParent(Transform? newParent)
    {
        if (Parent == newParent)
            return;

        Parent?.Children.Remove(this);
        Parent = newParent;
        Parent?.Children.Add(this);

        MarkDirty();
    }

    private Matrix4x4 _localMatrix;
    private Matrix4x4 _worldMatrix;
    private bool _dirty = true;

    public Matrix4x4 LocalMatrix
    {
        get
        {
            if (_dirty)
                RecalculateMatrices();
            return _localMatrix;
        }
    }

    public Matrix4x4 WorldMatrix
    {
        get
        {
            if (_dirty)
                RecalculateMatrices();
            return _worldMatrix;
        }
    }

    public Vector3 Forward =>
        Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitZ, WorldMatrix));

    public Vector3 Right =>
        Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitX, WorldMatrix));

    public Vector3 Up =>
        Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitY, WorldMatrix));

    private void RecalculateMatrices()
    {
        _localMatrix =
            Matrix4x4.CreateScale(_localScale) *
            Matrix4x4.CreateFromQuaternion(_localRotation) *
            Matrix4x4.CreateTranslation(_localPosition);

        _worldMatrix = Parent == null
            ? _localMatrix
            : _localMatrix * Parent.WorldMatrix;

        _dirty = false;
    }

    private void MarkDirty()
    {
        if (_dirty) return;
        _dirty = true;
        foreach (var child in Children)
            child.MarkDirty();
    }

    private const float Deg2Rad = MathF.PI / 180f;
    private const float Rad2Deg = 180f / MathF.PI;

    public static Quaternion FromEuler(Vector3 eulerDeg)
    {
        return Quaternion.CreateFromYawPitchRoll(
            eulerDeg.Y * Deg2Rad,
            eulerDeg.X * Deg2Rad,
            eulerDeg.Z * Deg2Rad
        );
    }

    /// <summary>
    /// Mom I'm scared
    /// </summary>
    public static Vector3 ToEuler(Quaternion q)
    {
        Vector3 euler;

        // roll (x-axis rotation) = atan2(sinr/cosr)
        float sinp = 2 * (q.W * q.X + q.Y * q.Z);
        float cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
        euler.X = MathF.Atan2(sinp, cosp);

        // pitch (y-axis rotation) = asin(siny)
        float siny = 2 * (q.W * q.Y - q.Z * q.X);
        euler.Y = MathF.Abs(siny) >= 1
            ? MathF.CopySign(MathF.PI / 2, siny)
            : MathF.Asin(siny);

        // yaw (z-axis rotation) = atan2(sinr/cosr)
        float sinr = 2 * (q.W * q.Z + q.X * q.Y);
        float cosr = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
        euler.Z = MathF.Atan2(sinr, cosr);

        return euler * Rad2Deg;
    }
}