namespace Arphros;

public abstract class Behaviour
{
    public virtual void ProcessInput() { }
    public virtual void Update(float dt) { }
    public virtual void FixedUpdate(float fixedDt) { }
    public virtual void Render() { }
}