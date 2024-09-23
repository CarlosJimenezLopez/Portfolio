using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{


    //public TextMeshProGUI puntos;
    public GameObject[] vidas;
    public void Update()
    {
        //puntos.text = gameManager.Istance.PuntosTotales.ToString();
    }


    public void PierdeVida(int nVida)
    {
        vidas[nVida].SetActive(false);
    }
    public void GanaVida(int nVida)
    {
        vidas[nVida].SetActive(true);
    }
}
