using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patrones.FlyWeight.Interfaces;

namespace Patrones.FlyWeight.States
{
    public class HalfLife : AEnemiState
    {
        public int vidaCambioMax = 0;

        public HalfLife(IEnemi enemi) : base(enemi)
        {
        }

        public override void Enter()
        {
            vidaCambioMax = (int)(enemi.getMaxLife() * 0.3);
            enemi.setSpeed(4);

            //Debug.Log($"Zombie {zombie.GetGameObject().name} started searching for player");
        }

        public override void Exit()
        {
            // Debug.Log($"Zombie {zombie.GetGameObject().name} ended searching for player");
        }

        public override void Update()
        {
            if (enemi.getLife() <= vidaCambioMax)
            {
                enemi.SetState(new MinLife(enemi));
            }
        }

    }
}

