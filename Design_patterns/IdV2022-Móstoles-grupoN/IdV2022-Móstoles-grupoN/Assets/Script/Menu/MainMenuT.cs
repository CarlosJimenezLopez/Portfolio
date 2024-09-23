using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuT : MonoBehaviour
{
    /*
     * Este codigo se ha ayudado del canal de Youtube Rocket Jam.
     */
    public void PlayThisGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitThisGame()
    {
        Application.Quit();
    }
}
