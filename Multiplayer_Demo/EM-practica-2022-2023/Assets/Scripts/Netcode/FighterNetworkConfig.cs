using Cinemachine;
using Movement.Components;
using System.Collections.Generic;
using Systems;
using Unity.Netcode;

using UnityEngine;


namespace Netcode
{
    public class FighterNetworkConfig : NetworkBehaviour
    {
       
        public override void OnNetworkSpawn()
        {   
            if (!IsOwner) return;
            
            FighterMovement fighterMovement = GetComponent<FighterMovement>();
            
            ICinemachineCamera virtualCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
            virtualCamera.Follow = transform;

            GameManage Gestor = GameObject.FindObjectOfType<GameManage>();
            Gestor.EventoIniciarPartida += () => {InputSystem.Instance.Character = fighterMovement;};

            OnServerRpc();
        }

        [ServerRpc]
        public void OnServerRpc()
        {
            
            GameManage Gestor = GameObject.FindObjectOfType<GameManage>();
            Gestor.DejarMover();
        }

    }
}