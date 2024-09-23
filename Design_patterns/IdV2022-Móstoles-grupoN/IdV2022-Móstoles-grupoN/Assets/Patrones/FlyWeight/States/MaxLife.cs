using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patrones.FlyWeight.Interfaces;

namespace Patrones.FlyWeight.States
{
    public class MaxLife : AEnemiState
    {
        public int vidaCambioMax = 0;

        public MaxLife(IEnemi enemi) : base(enemi)
        {
        }

        public override void Enter()
        {
            vidaCambioMax = (int)(enemi.getMaxLife() * 0.9);
            enemi.setSpeed(0);

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
                enemi.SetState(new HalfLife(enemi));
            }
        }

    }
}
