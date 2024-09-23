using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Vector3 pos;
    public Vector3 vel;
    public Vector3 force;
    public int id;


    public float mass;
    public bool isFixed;

    public Node(Vector3 position, float mass, int id)
    {
        pos = position;
        vel = Vector3.zero;
        this.mass = mass;
        this.id = id;

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = pos;
    }

    public void ComputeForces()
    {
        //force += mass * transform.parent.GetComponent<MassSpringCloth>().Gravity;
    }
}
