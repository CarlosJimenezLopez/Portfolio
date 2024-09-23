using JetBrains.Annotations;
using Netcode.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : NetworkBehaviour
{
    [SerializeField] int min, seg;
    [SerializeField] Text tiempo;
    private bool parar = false;
    

    private NetworkVariable<float> restante = new NetworkVariable<float>();


    // Update is called once per frame
    void Update()
    {
        if (parar) return;
        OnTiempo(Time.deltaTime);

    }


    public void OnTiempo(float tiempo)
    {
        if (!IsServer) return;

        restante.Value -= tiempo;
        //enMarcha.Value = true;


    }

    public override void OnNetworkSpawn()
    {
        GameManage Gestor = GameObject.FindObjectOfType<GameManage>();
        Gestor.EventoIniciarPartida += () => {
            
            if (IsServer)
            {
                restante.Value = (min * 60) + seg;
            }
            restante.OnValueChanged += OnenMarchaChanged;
        };

    }

    private void OnenMarchaChanged(float previous, float current)
    {

        //Debug.Log(current);
        if (restante.Value < 1)
        {
            parar = true;
            //Mueren todos
        }
        int tempMin = Mathf.FloorToInt(restante.Value / 60);
        int tempSeg = Mathf.FloorToInt(restante.Value % 60);
        tiempo.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
    }
}
