using Movement.Components;
using UnityEditor;

namespace Movement.Commands
{
    public class DieCommand : AFightCommand
    {
        public DieCommand(IFighterReceiver receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            Client.Die();
            //Client.Destroy();
        }
    }
}