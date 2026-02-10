#include "game_object.h"

namespace Arphros {
GameObject::GameObject() {
    name = "GameObject";
}

GameObject::~GameObject() {
    for (auto* c : components) delete c;
}
}