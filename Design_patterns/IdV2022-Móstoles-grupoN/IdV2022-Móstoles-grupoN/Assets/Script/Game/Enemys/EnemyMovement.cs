using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] private float velocidad;
    [SerializeField] private float distancia;
    [SerializeField] private LayerMask suelo;
    public Animator ani;
   
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
      

    }
    private void Update()
    {
        if (ani.GetBool("Muerto") == false)
        {
            rigidBody.velocity = new Vector2(velocidad * transform.right.x, rigidBody.velocity.y);

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.right, distancia, suelo);

            if (raycastHit)
            {
                ani.SetBool("Suelo", true);
                Girar();
            }
        }
        else
        {
            rigidBody.velocity = new Vector2(0, 0) ;
        }
    }

    private void Girar()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * distancia);
    }


    
}
