using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using Movement.Components;
using Unity.Collections;
using Netcode.UI;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {
        public GameObject []characterPrefab;
        public static int chara = 0;
        
        public static string ip;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            FixedString32Bytes NickName = GameObject.FindGameObjectWithTag("nombre").GetComponent<UIHandler>().Nombre;             
            InstantiateCharacterServerRpc(OwnerClientId,chara,ip, NickName);
            
        }
    
        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id, int chara, string ippasado, FixedString32Bytes name)
        {

            GameObject characterGameObject = Instantiate(characterPrefab[chara], SpawnRandom(), Quaternion.identity);
            characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.transform.SetParent(transform, false);

        }

        public Vector3 SpawnRandom()
        {
            Vector3 Posicion;
            int random = Random.Range(1, 4);

            if (random == 1)
            {
                return Posicion = new Vector3(Random.Range(-9.0F, 9.0F), -2.485005f, Random.Range(-10.0F, 10.0F));
            }
            else if (random == 2)
            {
                return Posicion = new Vector3(Random.Range(-7.0F, 7.0F), -0.4835552f, Random.Range(-10.0F, 10.0F));
            }
            else if (random == 3)
            {
                return Posicion = new Vector3(Random.Range(-2.5f, 8.0F), 1.514987f, Random.Range(-10.0F, 10.0F));
            }

            return Vector3.zero;

        }
    }
}
