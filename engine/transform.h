#pragma once
#include "raylib.h"
#include "raymath.h"
#include <vector>

namespace Arphros {

class Transform {
public:
    Transform();

    // Local
    Vector3 GetLocalPosition() const;
    void SetLocalPosition(Vector3 v);

    Quaternion GetLocalRotation() const;
    void SetLocalRotation(Quaternion q);

    Vector3 GetLocalEulerAngles() const;
    void SetLocalEulerAngles(Vector3 euler);

    Vector3 GetLocalScale() const;
    void SetLocalScale(Vector3 v);

    // World
    Vector3 GetPosition();
    void SetPosition(Vector3 v);

    Quaternion GetRotation();
    void SetRotation(Quaternion q);

    Vector3 GetEulerAngles();
    void SetEulerAngles(Vector3 euler);

    Vector3 GetScale() const;
    void SetScale(Vector3 v);

    // Hierarchy
    void SetParent(Transform* newParent);
    Transform* GetParent() const { return parent; }
    const std::vector<Transform*>& GetChildren() const { return children; }

    // Matrices
    Matrix GetLocalMatrix();
    Matrix GetWorldMatrix();

    // Directions
    Vector3 Forward();
    Vector3 Right();
    Vector3 Up();

    // Euler helpers
    static Quaternion FromEuler(Vector3 eulerDeg);
    static Vector3 ToEuler(Quaternion q);

private:
    void RecalculateMatrices();
    void MarkDirty();

private:
    Vector3 localPosition = {0,0,0};
    Quaternion localRotation = QuaternionIdentity();
    Vector3 localScale = {1,1,1};

    Transform* parent = nullptr;
    std::vector<Transform*> children;

    Matrix localMatrix;
    Matrix worldMatrix;
    bool dirty = true;
};

}
