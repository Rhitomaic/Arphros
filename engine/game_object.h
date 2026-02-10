#pragma once
#include <algorithm>
#include <type_traits>
#include <vector>

#include "component.h"
#include "object.h"
#include "transform.h"

namespace Arphros {

class GameObject : public Object {
   public:
    GameObject();
    ~GameObject();

    Transform transform;

    template <typename T, typename... Args>
    T* AddComponent(Args&&... args);

    template <typename T>
    T* GetComponent() const;

   private:
    std::vector<Component*> components;
};

template <typename T, typename... Args>
T* GameObject::AddComponent(Args&&... args) {
    static_assert(std::is_base_of<Component, T>::value,
                  "T must derive from Component");

    T* comp = new T(std::forward<Args>(args)...);
    comp->gameObject = this;
    comp->transform = &transform;

    components.push_back(comp);
    comp->Start();
    return comp;
}

template <typename T>
T* GameObject::GetComponent() const {
    for (auto* c : components) {
        if (auto* casted = dynamic_cast<T*>(c)) return casted;
    }
    return nullptr;
}

}  // namespace Arphros
