using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{

    public string PlayerName { get; private set; }
    //public ulong ClientId { get; private set; }

    public PlayerData(string playerName)
    {
        PlayerName = playerName;
        //ClientId = clientId;
    }

}




