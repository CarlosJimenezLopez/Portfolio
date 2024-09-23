using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patrones.FlyWeight.Interfaces;

namespace Patrones.FlyWeight.States
{
    public class MinLife : AEnemiState
    {
      

        public MinLife(IEnemi enemi) : base(enemi)
        {
        }

        public override void Enter()
        {
            enemi.setSpeed(8);
            //Debug.Log($"Zombie {zombie.GetGameObject().name} started searching for player");
        }

        public override void Exit()
        {
            // Debug.Log($"Zombie {zombie.GetGameObject().name} ended searching for player");
        }

        public override void Update()
        {
            
        }

    }
}
