using System;
using Patrones.DirtyFlag.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;

namespace Patrones.DirtyFlag.Data
{
    /*
     * Este codigo esta basado en el ejercicio de clase del DirtyFlag, ya que la funcion de guardado no tiene mucha variacion. 
     */
    [Serializable]
    public abstract class ASaveableGameObjectData : ISaveableGameObjectData
    {
        public float[] position;
        public bool isActive;

        public ASaveableGameObjectData(GameObject gameObject)
        {
            position = Vector3ToFloat(gameObject.transform.position);
            isActive = gameObject.activeSelf;
        }

        public void ToGameObject(MonoBehaviour component)
        {
            component.gameObject.transform.position = FloatToVector3(position);
            component.gameObject.SetActive(isActive);
        }
        
        protected static float[] Vector3ToFloat(Vector3 vector)
        {
            float[] data = new float[3];
            data[0] = vector.x;
            data[1] = vector.y;
            data[2] = vector.z;
            return data;
        }

        protected static Vector3 FloatToVector3(float[] data)
        {
            Assert.AreEqual(data.Length, 3);
            Vector3 vector = new Vector3(data[0], data[1], data[2]);
            return vector;
        }
    }
}