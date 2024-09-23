using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patrones.FlyWeight;
using Patrones.FlyWeight.Interfaces;
using Patrones.FlyWeight.States;
using IState = Patrones.FlyWeight.Interfaces.IState;
using UnityEngine.SceneManagement;

public class Enemi : MonoBehaviour, IEnemi
{
    /*
     * Este codigo esta basado en el ejercicio de clase del State, las funciones get y set de los estdos, y se ha ayudado del codigo del canal de youtube rocket Jam para el movimiento. 
     */

    [SerializeField] public FlyWeightEnemi enemi;


    private Rigidbody2D rigidBody;
    public Animator ani;
    int vidaActual;
    private IState currentState;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        vidaActual = enemi.vidaMaxima;

    }

    private void Awake()
    {
        if (enemi.Jefe == true)
        {
            SetState(new MaxLife(this));
        }
    }

    private void Update()
    {
        if (ani.GetBool("Muerto") == false)
        {
            rigidBody.velocity = new Vector2(enemi.velocidad * transform.right.x, rigidBody.velocity.y);

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.right, enemi.distancia, enemi.suelo);

            if (raycastHit)
            {
                ani.SetBool("Suelo", true);
                Girar();
            }
        }
        else
        {
            rigidBody.velocity = new Vector2(0, 0);
        }

        if (enemi.Jefe == true)
        {
            currentState.Update();
        }
        
    }

    public void Girar()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * enemi.distancia);
    }



    public void OnCollisionEnter2D(Collision2D other)
    {/*
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PerderVida();
        }
        */
    }

    public void enemigoPierdeVida(int daño)
    {
        ani.SetTrigger("Daño");
        vidaActual -= daño;


        if (vidaActual <= 0)
        {
            enemigoMuere();
        }
    }

    public void enemigoMuere()
    {
        rigidBody.gravityScale = 0f;
        ani.SetBool("Muerto", true);
        rigidBody.velocity = new Vector2(0,0);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        if (enemi.Jefe == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }



    }

    public int getLife()
    {
        return vidaActual;
    }

    public IState GetState()
    {
        return currentState;
    }

   public int getMaxLife()
    {
        return enemi.vidaMaxima;
    }

    public void setSpeed(int speed)
    {
        enemi.velocidad = speed;
    }


    public void SetState(IState state)
    {
        // Exit old state
        if (currentState != null)
        {
            currentState.Exit();
        }

        // Set current state and enter
        currentState = state;
        currentState.Enter();
    }

}
