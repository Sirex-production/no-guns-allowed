using Extensions;

namespace Ingame
{
    public class InvokableDoor : Invokable
    {
        public override void Invoke()
        {
            this.SetGameObjectInactive();
        }
    }
}