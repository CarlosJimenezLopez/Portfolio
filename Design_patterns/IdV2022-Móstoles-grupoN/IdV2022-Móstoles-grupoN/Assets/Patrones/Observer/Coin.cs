using System.Collections;
using System.Collections.Generic;
using Patrones.DirtyFlag.Data;
using Patrones.DirtyFlag.Interfaces;
using Patrones.DirtyFlag;
using UnityEngine;

public class Coin : MonoBehaviour, IPickableCoin, ISaveableGameObject
{
    /*
     * Este codigo utiliza funciones del dirty flag hecho en clase, cambia el metodo Pick.     */
    [SerializeField]
    public int Value = 1;
    void Update()
    {
        
    }

    public float Pick()
    {
        dirtyFlag = true;
        gameObject.SetActive(false);
        FindObjectOfType<GameSaver>()?.GetComponent<GameSaver>()?.SetSceneDirty();
        return Value;
        
    }

    public float GetValue()
    {
        return Value;
    }

    private bool dirtyFlag;

    public bool NeedsSaving()
    {
        return dirtyFlag;
    }

    public ISaveableGameObjectData GetSavedObject()
    {
        dirtyFlag = false;
        return new SaveableCoinData(this);
    }

    public void ToGameObject(ISaveableGameObjectData data)
    {
        SaveableCoinData coinData = (SaveableCoinData)data;
        coinData.ToGameObject(this);
    }
}
