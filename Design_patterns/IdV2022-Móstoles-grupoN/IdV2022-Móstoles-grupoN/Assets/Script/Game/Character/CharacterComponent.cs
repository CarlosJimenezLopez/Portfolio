using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    /*
     * Este codigo se ha ayudado de StackOverFlow y el canal de youtube de Brackeys.
     */
    public float velocidad;
    public float capacidadSalto;
    public int saltosMaximos;
    public AudioManager audiomanager;
    public AudioClip sonidoGiro;
    public AudioClip sonidoSalto;


    // Update is called once per frame
    private Rigidbody2D rigidBody;
    //bool jump = false;
    private bool orientacion;
    private BoxCollider2D boxCollider;
    public LayerMask suelo;
    private Animator ani;
    public Transform AtackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 34;
    public LayerMask Enemies;
    private int saltosRestantes;
    //public Animator ani;
    public float attackRate = 1f;
    public float nextAttackTime = 0f;
    int nVidas = 3;



    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        saltosRestantes = saltosMaximos;
        ani = GetComponent<Animator>();
    }
    void Update()
    {
        Movimiento();
        Salto();
        // AnimacionSalto();

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    bool PegadoAlSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, suelo);
        return raycastHit.collider != null;
    }

    void Salto()
    {
        if (PegadoAlSuelo())
        {
            saltosRestantes = saltosMaximos;
        }
        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            saltosRestantes--;
            ani.SetTrigger("Salto");
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            rigidBody.AddForce(Vector2.up * capacidadSalto, ForceMode2D.Impulse);
            audiomanager.ReproducirSonido(sonidoSalto);
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
        if ((orientacion == true && reciveMovimiento > 0) || (orientacion == false && reciveMovimiento < 0))
        {
            orientacion = !orientacion;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            audiomanager.ReproducirSonido(sonidoGiro);
        }

    }
    void Attack()
    {
        ani.SetTrigger("Ataque");

        Collider2D[] hit = Physics2D.OverlapCircleAll(AtackPoint.position, attackRange, Enemies);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemi>().enemigoPierdeVida(attackDamage);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (AtackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(AtackPoint.position, attackRange);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (nVidas > 0)
        {

            if (other.gameObject.CompareTag("Enemies"))
            {
                ani.SetTrigger("PersonajeGolpeado");
                GameManager.Instance.PerderVida();
                nVidas--;
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Enemies"))
            {
                ani.SetBool("PersonajeMuere",true);
                GameManager.Instance.PerderVida();
                nVidas--;
            }
        }
        
    }
}
