using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patrones.FlyWeight
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "GameItems/NPCs/Enemy", order = 0)]

    public class FlyWeightEnemi : ScriptableObject
    {
        public int vidaMaxima;
        public int velocidad;
        public float distancia;
        public LayerMask suelo;
        public bool Jefe;
    }
}
