using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrones.FlyWeight.Interfaces
{
    public abstract class AEnemiState : IState
    {
        protected IEnemi enemi;

        public AEnemiState(IEnemi enemi)
        {
            this.enemi = enemi;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}
