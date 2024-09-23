using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge 
{
    public Node nodeA, nodeB, nodeC;

    //Se contruye la arista con tres nodos, los dos que forman la propia arista y el nodo que forma el triangulo completo. Se ordenan los dos primeros nodos para que el diccionario pueda identificar 
    // duplicados.
    public Edge(Node nodeA, Node nodeB, Node nodeC)
    {
        if (nodeA.id < nodeB.id)
        {
            this.nodeA = nodeA;
            this.nodeB = nodeB;
        }
        else
        {
            this.nodeA = nodeB;
            this.nodeB = nodeA;
        }

        this.nodeC = nodeC;

    }
    // Start is called before the first frame update



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }






}
