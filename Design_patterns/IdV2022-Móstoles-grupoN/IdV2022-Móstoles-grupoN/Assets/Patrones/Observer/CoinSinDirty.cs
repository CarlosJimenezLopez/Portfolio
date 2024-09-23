using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSinDirty : MonoBehaviour, IPickableCoinSinDirty
{
   
    [SerializeField]
    public int Value = 1;
   

    void Update()
    {
       
    }

    public float PickDirty()
    {
        Destroy(gameObject);
        return Value;
    }


}

