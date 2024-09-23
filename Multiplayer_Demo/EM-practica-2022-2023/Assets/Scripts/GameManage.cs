using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using System.Runtime.CompilerServices;

public class GameManage : NetworkBehaviour
{
    public event Action EventoIniciarPartida;
    public event Action SalaLlena;
    public int JugadoresCreados = 0;

    


    [ClientRpc]
    public void IniciarPartidaClientRpc()
    {
        EventoIniciarPartida?.Invoke();
    }

    
    public void DejarMover()
    {        
        JugadoresCreados++;
        if (JugadoresCreados == 3)
        {
            IniciarPartidaClientRpc();
             SalaLlena?.Invoke();
        }
    } 

}
