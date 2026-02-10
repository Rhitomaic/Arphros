#pragma once
#include "object.h"

namespace Arphros {
    class GameObject;
    class Transform;
    class Component : public Object {
    public:
        virtual ~Component() = default;

        GameObject* GetGameObject() const { return gameObject; }
        Transform* GetTransform() const { return transform; }

        virtual void Start() {}
        virtual void Update(float dt) {}

    protected:
        friend class GameObject;
        GameObject* gameObject = nullptr;
        Transform* transform = nullptr;
    };
}  // namespace Arphros