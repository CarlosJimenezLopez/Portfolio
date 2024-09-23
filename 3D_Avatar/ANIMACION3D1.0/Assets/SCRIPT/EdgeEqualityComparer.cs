using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeEqualityComparer : IEqualityComparer<Edge>
{
    public bool Equals(Edge a, Edge b)
    {
        if((a.nodeA.id == b.nodeA.id) && (a.nodeB.id == b.nodeB.id)) 
        //Si a y b son iguales
        return true;
        //Si a y b no son iguales
        else
        return false;
    }
    
    public int GetHashCode(Edge e)
    {
        //int hcode = e.nodeA.id+e.nodeB.id+e.nodeC.id;//Se calcula un número entero asociado a la arista
        int hcode = e.nodeA.id+e.nodeB.id; // Debería la suma de los tres, sin embargo, al obligar a que sean los mismos por el equals, nunca va a repetirse.
        return hcode.GetHashCode();
    }
    
}

