using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comportamiento_Personaje : MonoBehaviour
{
    public float velocidadMovimiento = 100.0f;
    public float velocidadGiro = 100.0f;
    private float x, y;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if((Mathf.Abs(x)==00) && (Mathf.Abs(y)==0))
                anim.SetTrigger("Estirando");
        else
            anim.SetTrigger("Andando");

        if (Input.GetKeyDown(KeyCode.Space))
            anim.SetTrigger("Chutando");
        transform.Rotate(0, (x * velocidadGiro * Time.deltaTime),0);
        transform.Translate(0, 0, (-y * velocidadMovimiento * Time.deltaTime));
    }
}
