using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrones.FlyWeight.Interfaces
{
    public interface IEnemi 
    {
        //public GameObject GetGameObject();
        public void SetState(IState state);
        public IState GetState();

        public void Girar();
        public void OnDrawGizmosSelected();
        public void OnCollisionEnter2D(Collision2D other);
        public void enemigoPierdeVida(int daño);
        public void enemigoMuere();
        public int getLife();
        public int getMaxLife();
        public void setSpeed(int speed);
    }
}
