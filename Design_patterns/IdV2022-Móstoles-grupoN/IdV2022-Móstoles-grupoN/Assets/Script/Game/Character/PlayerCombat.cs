using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Transform AtackPoint;
    public float attackRange = 0.5f;
    public float attackDamage = 34;
    public LayerMask Enemies;
    public Animator ani;
    public float attackRate = 1f;
    public float nextAttackTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
       
    }



    void Attack()
    {
        ani.SetTrigger("Ataque");
        
      Collider2D[] hit= Physics2D.OverlapCircleAll(AtackPoint.position, attackRange, Enemies);

        foreach(Collider2D enemy in hit)
           {
            enemy.GetComponent<Enemi>().enemigoPierdeVida(33);
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
    
}
