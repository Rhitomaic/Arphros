using Raylib_cs;

namespace Arphros;

public static class MeshLibrary
{
    public static readonly Model Cube;

    static MeshLibrary()
    {
        Mesh mesh = Raylib.GenMeshCube(1, 1, 1);
        Cube = Raylib.LoadModelFromMesh(mesh);
    }
}