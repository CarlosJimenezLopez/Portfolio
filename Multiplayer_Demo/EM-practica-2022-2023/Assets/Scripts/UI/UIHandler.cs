using Movement.Commands;
using Movement.Components;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


namespace Netcode { 
    namespace UI
    {
        public class UIHandler : NetworkBehaviour
        {
            public GameObject debugPanelCharacter;
            public GameObject debugPanelHC;
            public GameObject debugPlay;
            public GameObject HUD;
            
            public InputField inputfield;
            public Text nombre;
            public Image corrector;

            public Button hostButton;
            public Button clientButton;
            public Button OniButton;
            public Button HauntressButton;
            public Button AkaiButton;
            public Button Play;
            public Button Exit;
            public Button Inicio;

            public string Nombre;

            public Text salaLlena;
            public NetworkVariable<int> numJugadores = new NetworkVariable<int>();


            private void Awake()
            {
                corrector.color = Color.red;
            }

            private void Start()
            {
               

                hostButton.onClick.AddListener(OnHostButtonClicked);
                clientButton.onClick.AddListener(OnClientButtonClicked);
                OniButton.onClick.AddListener(OnOniButtonClicked);
                AkaiButton.onClick.AddListener(OnAkaiButtonClicked);
                HauntressButton.onClick.AddListener(OnHauntressButtonClicked);
                Play.onClick.AddListener(OnPlayButtonClicked);
                Exit.onClick.AddListener(OnExitButtonClicked);
                //Inicio.onClick.AddListener(OnInicioButtonClicked);
            }

            private void Update()
            {
                if (nombre.text.Length < 3)
                {
                    corrector.color = Color.red;
                    
                }

                if (nombre.text.Length >= 3)
                {
                    corrector.color = Color.green;
                    
                }
            }

            public override void OnNetworkSpawn()
            {
                
                
            }


            private void OnHostButtonClicked()
            {
                    PlayerNetworkConfig.ip = inputfield.text;
                    NetworkManager.Singleton.StartHost();
                    debugPanelHC.SetActive(false);
                    HUD.SetActive(true);
                numJugadores.Value++;
            }         

            private void OnClientButtonClicked() 
            {
                    PlayerNetworkConfig.ip = inputfield.text; 
                    NetworkManager.Singleton.StartClient();               
                    debugPanelHC.SetActive(false);
                    HUD.SetActive(true);
                numJugadores.Value++;
            }


            private void OnOniButtonClicked()
            {
                if (corrector.color == Color.red) return;  
                PlayerNetworkConfig.chara = 2;
                debugPanelCharacter.SetActive(false);
                if (numJugadores.Value < 2)
                {
                    debugPanelHC.SetActive(true);
                }
                Nombre = inputfield.text;
                PlayerPrefs.SetString("nombre", inputfield.text);
                
            }
            private void OnAkaiButtonClicked()
            {
                if (corrector.color == Color.red) return;
                PlayerNetworkConfig.chara = 1;
                debugPanelCharacter.SetActive(false);
                if (numJugadores.Value < 2)
                {
                    debugPanelHC.SetActive(true);
                }
                
                Nombre = inputfield.text;
                PlayerPrefs.SetString("nombre", inputfield.text);
            }
            private void OnHauntressButtonClicked()
            {
                if (corrector.color == Color.red) return;
                PlayerNetworkConfig.chara = 0;
                debugPanelCharacter.SetActive(false);
                if (numJugadores.Value < 2)
                {
                    debugPanelHC.SetActive(true);
                }
                Nombre = inputfield.text;
                PlayerPrefs.SetString("nombre", inputfield.text);
            }

            private void OnPlayButtonClicked()
            {
                debugPlay.SetActive(false);
                debugPanelCharacter.SetActive(true);
            }

            private void OnExitButtonClicked()
            {
                Application.Quit();
            }

            /*public void OnInicioButtonClicked()
            {
                NetworkManager.Singleton.DisconnectClient(PlayerNetworkConfig.Personaje[0]);
                //FighterMovement.Die();
                debugPlay.SetActive(true);
                HUD.SetActive(false);
                
            }*/

        }
    }
}