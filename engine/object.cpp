#include "object.h"

namespace Arphros {
uint64_t Object::nextID = 1;

Object::Object() {
    instanceID = nextID++;
}
}