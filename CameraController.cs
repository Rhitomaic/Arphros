using Raylib_cs;
using System.Numerics;

namespace Arphros;

public class CameraController : Behaviour
{
    public Transform? Target;

    public Vector3 PivotOffset = Vector3.Zero;
    public Vector3 TargetRotation = new(45, 60, 0);
    public float TargetDistance = 20f;
    public float SmoothFactor = 5f;
    public float Fov = 60f;

    Camera3D camera;

    public CameraController()
    {
        camera = new Camera3D();
    }

    public override void Update(float dt)
    {
        if (Target == null) return;

        Vector3 desiredPivot =
            Target.LocalPosition + PivotOffset;

        Transform.LocalPosition = Lerp(
            Transform.LocalPosition,
            desiredPivot,
            SmoothFactor * dt
        );

        Transform.LocalEulerAngles = TargetRotation;
    }

    public Camera3D GetCamera()
    {
        Vector3 forward =
            Game.ForwardFromRotation(Transform.LocalEulerAngles);

        Vector3 cameraPos =
            Transform.LocalPosition - forward * TargetDistance;

        camera.Position = cameraPos;
        camera.Target = Transform.LocalPosition;
        camera.Up = Vector3.UnitY;
        camera.FovY = Fov;
        camera.Projection = CameraProjection.Perspective;

        return camera;
    }

    static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }
}
