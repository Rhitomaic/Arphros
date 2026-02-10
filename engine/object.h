#pragma once
#include <cstdint>
#include <string>

namespace Arphros {
class Object {
   public:
    Object();
    virtual ~Object() = default;

    uint64_t GetInstanceID() const { return instanceID; }

    const std::string& GetName() const { return name; }
    void SetName(const std::string& n) { name = n; }

    bool operator==(const Object& other) const {
        return instanceID == other.instanceID;
    }

   protected:
    std::string name = "Object";

   private:
    uint64_t instanceID;
    static uint64_t nextID;
};
}