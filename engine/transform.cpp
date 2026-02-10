#include "transform.h"
#include <algorithm>
#include <cmath>

namespace Arphros {
Transform::Transform() {
    MarkDirty();
}

// ---------- Local ----------

Vector3 Transform::GetLocalPosition() const {
    return localPosition;
}

void Transform::SetLocalPosition(Vector3 v) {
    localPosition = v;
    MarkDirty();
}

Quaternion Transform::GetLocalRotation() const {
    return localRotation;
}

void Transform::SetLocalRotation(Quaternion q) {
    localRotation = QuaternionNormalize(q);
    MarkDirty();
}

Vector3 Transform::GetLocalEulerAngles() const {
    return ToEuler(localRotation);
}

void Transform::SetLocalEulerAngles(Vector3 euler) {
    localRotation = FromEuler(euler);
    MarkDirty();
}

Vector3 Transform::GetLocalScale() const {
    return localScale;
}

void Transform::SetLocalScale(Vector3 v) {
    localScale = v;
    MarkDirty();
}

// ---------- World ----------

Vector3 Transform::GetPosition() {
    Matrix m = GetWorldMatrix();
    return {m.m12, m.m13, m.m14};
}

void Transform::SetPosition(Vector3 v) {
    if (!parent) {
        SetLocalPosition(v);
    } else {
        Matrix invParent = MatrixInvert(parent->GetWorldMatrix());
        localPosition = Vector3Transform(v, invParent);
        MarkDirty();
    }
}

Quaternion Transform::GetRotation() {
    if (!parent) return localRotation;
    return QuaternionNormalize(
        QuaternionMultiply(parent->GetRotation(), localRotation));
}

void Transform::SetRotation(Quaternion q) {
    if (!parent) {
        SetLocalRotation(q);
    } else {
        Quaternion invParent = QuaternionInvert(parent->GetRotation());
        localRotation = QuaternionNormalize(QuaternionMultiply(invParent, q));
        MarkDirty();
    }
}

Vector3 Transform::GetEulerAngles() {
    return ToEuler(GetRotation());
}

void Transform::SetEulerAngles(Vector3 euler) {
    SetRotation(FromEuler(euler));
}

Vector3 Transform::GetScale() const {
    if (!parent) return localScale;
    return Vector3Multiply(parent->GetScale(), localScale);
}

void Transform::SetScale(Vector3 v) {
    if (!parent) {
        SetLocalScale(v);
    } else {
        Vector3 parentScale = parent->GetScale();
        localScale = {v.x / parentScale.x, v.y / parentScale.y,
                      v.z / parentScale.z};
        MarkDirty();
    }
}

// ---------- Hierarchy ----------

void Transform::SetParent(Transform* newParent) {
    if (parent == newParent) return;

    if (parent) {
        auto& siblings = parent->children;
        siblings.erase(std::remove(siblings.begin(), siblings.end(), this),
                       siblings.end());
    }

    parent = newParent;

    if (parent) parent->children.push_back(this);

    MarkDirty();
}

// ---------- Matrices ----------

Matrix Transform::GetLocalMatrix() {
    if (dirty) RecalculateMatrices();
    return localMatrix;
}

Matrix Transform::GetWorldMatrix() {
    if (dirty) RecalculateMatrices();
    return worldMatrix;
}

void Transform::RecalculateMatrices() {
    localMatrix = MatrixMultiply(
        MatrixMultiply(MatrixScale(localScale.x, localScale.y, localScale.z),
                       QuaternionToMatrix(localRotation)),
        MatrixTranslate(localPosition.x, localPosition.y, localPosition.z));

    worldMatrix = parent ? MatrixMultiply(localMatrix, parent->GetWorldMatrix())
                         : localMatrix;

    dirty = false;
}

void Transform::MarkDirty() {
    if (dirty) return;
    dirty = true;
    for (auto* child : children) child->MarkDirty();
}

// ---------- Directions ----------

Vector3 Transform::Forward() {
    return Vector3Normalize(
        Vector3RotateByQuaternion({0, 0, 1}, GetRotation()));
}

Vector3 Transform::Right() {
    return Vector3Normalize(
        Vector3RotateByQuaternion({1, 0, 0}, GetRotation()));
}

Vector3 Transform::Up() {
    return Vector3Normalize(
        Vector3RotateByQuaternion({0, 1, 0}, GetRotation()));
}

Quaternion Transform::FromEuler(Vector3 eulerDeg) {
    return QuaternionFromEuler(eulerDeg.x * DEG2RAD, eulerDeg.y * DEG2RAD,
                               eulerDeg.z * DEG2RAD);
}

/** @brief Mom I'm scared */
Vector3 Transform::ToEuler(Quaternion q) {
    Vector3 euler;

    float sinp = 2 * (q.w * q.x + q.y * q.z);
    float cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
    euler.x = std::atan2(sinp, cosp);

    float siny = 2 * (q.w * q.y - q.z * q.x);
    if (std::abs(siny) >= 1)
        euler.y = std::copysign(PI / 2, siny);
    else
        euler.y = std::asin(siny);

    float sinr = 2 * (q.w * q.z + q.x * q.y);
    float cosr = 1 - 2 * (q.y * q.y + q.z * q.z);
    euler.z = std::atan2(sinr, cosr);

    return {euler.x * RAD2DEG, euler.y * RAD2DEG, euler.z * RAD2DEG};
}
}
