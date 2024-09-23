using System;
using UnityEngine;

namespace Patrones.DirtyFlag.Data
{
    [Serializable]
    public class SaveableCoinData : ASaveableGameObjectData
    {
        public int Value;
        public SaveableCoinData(Coin coin) : base(coin.gameObject)
        {
            Value = coin.Value;
        }

        new public void ToGameObject(MonoBehaviour coin)
        {
            base.ToGameObject(coin);
            ((Coin)coin).Value = Value;
        }
    }
}