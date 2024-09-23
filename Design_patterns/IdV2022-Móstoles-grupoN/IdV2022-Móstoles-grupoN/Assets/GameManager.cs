using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int PuntosTotales { get { return puntos; }}
    private int puntos;
    public HUD hud;
    private int vidas = 3;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Varios GM");
        }
    }
    public void SumaDePuntos(int cantidad)
    {
        puntos += cantidad;
    }
    public void PerderVida()
    {
        vidas -= 1;

        if (vidas == 0)
        {
            SceneManager.LoadScene(3);
        }

        hud.PierdeVida(vidas);
    }

    public bool RecuperarVida()
    {
        if (vidas == 3)
        {
            return false;
        }

        hud.GanaVida(vidas);
        vidas += 1;
        return true;
    }
}
