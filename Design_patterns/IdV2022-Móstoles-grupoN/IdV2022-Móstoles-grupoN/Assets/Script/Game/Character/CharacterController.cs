using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float velocidad;
    public float capacidadSalto;



    // Update is called once per frame
    private Rigidbody2D rigidBody;
    //bool jump = false;
    private bool orientacion;
    private BoxCollider2D boxCollider;
    public LayerMask suelo;
    private Animator ani;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
    }
    void Update()
    {
        Movimiento();
        Salto();
       // AnimacionSalto();
    }
    
    bool PegadoAlSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, suelo);
        return raycastHit.collider != null;
    }
    
    void Salto()
    {
        if(Input.GetKeyDown(KeyCode.Space) && PegadoAlSuelo())
        {
            ani.SetTrigger("Salto");
            rigidBody.AddForce(Vector2.up*capacidadSalto, ForceMode2D.Impulse);
        }
        /*
        else
        {
            ani.SetBool("estaSaltando", false);
        }
        */

    }
    /*
    void AnimacionSalto()
    {
        if (PegadoAlSuelo() = true)
        {
            ani.SetBool("estaSaltando", true);
        }
        else
        {
            ani.SetBool("estaSaltando", false);
        }
    }
    */
    void Movimiento()
    {
        float reciveMovimiento = Input.GetAxis("Horizontal");
        // Rigidbody2D rBody = GetComponent<Rigidbody2D>();

        if (reciveMovimiento != 0)
        {
            ani.SetBool("estaCorriendo", true);
        }
        else
        {
            ani.SetBool("estaCorriendo", false);
        }
        rigidBody.velocity = new Vector2(reciveMovimiento * velocidad, rigidBody.velocity.y);
        Direccion(reciveMovimiento);
    }

    void Direccion(float reciveMovimiento)
    {
        if( (orientacion == true && reciveMovimiento > 0 )|| (orientacion == false && reciveMovimiento < 0))
        {
            orientacion = !orientacion;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

    }
}
