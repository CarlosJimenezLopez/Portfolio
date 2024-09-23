using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using System;

/// <summary>
/// Basic physics manager capable of simulating a given ISimulable
/// implementation using diverse integration methods: explicit,
/// implicit, Verlet and semi-implicit.
/// </summary>
public class MassSpringCloth : MonoBehaviour
{
    /// <summary>
    /// Default constructor. Zero all. 
    /// </summary>
    public MassSpringCloth()
    {
        this.Paused = true;
        this.TimeStep = 0.01f;
        this.Gravity = new Vector3(0.0f, -9.81f, 0.0f);
        this.IntegrationMethod = Integration.Symplectic;
    }

    /// <summary>
    /// Integration method.
    /// </summary>
    public enum Integration
    {
        Explicit = 0,
        Symplectic = 1,
    };

    #region InEditorVariables

    public bool Paused;
    public float TimeStep;
    public Vector3 Gravity;
    public Integration IntegrationMethod;
    public List<Node> nodes;
    public List<Spring> springs;
    public float mass;
    public float stiffness;
    public float flexiStiffness;

    // Variables usadas para amortiguamiento
    public float nodeDamping;
    public float springDamping;

    public  Vector3[] vertices;
    public int[] triangles;


    public List<GameObject> cubes;


    private Vector3 pos;


    public EdgeEqualityComparer edgeEqualityComparer = new EdgeEqualityComparer();
    public Dictionary<Edge, Edge> edgeDictionary;


    #endregion

    #region OtherVariables
    #endregion

    #region MonoBehaviour

    public void Start()
    {
        // Asigna a mesh la maya pintada en la escena, de ella obtiene los vertices y los triangulos.
           Mesh mesh = this.GetComponent<MeshFilter>().mesh;
            vertices = mesh.vertices;
            triangles = mesh.triangles;
           
        nodes = new List<Node>(vertices.Length);
        
        springs = new List<Spring>(triangles.Length);

        
        Dictionary<Edge, Edge> edgeDictionary = new Dictionary<Edge, Edge>(edgeEqualityComparer);


        //Se rellena la lista de nodos, creando uno por cada vertice
        // Se pasan a coordenadas locales del objeto que posee este script.
        int i = 0;
        foreach (Vector3 vertex in vertices)
        {
            Vector3 pos = transform.TransformPoint(vertices[i]);
            Node node = new Node(pos, 1.0f, i);
            nodes.Add(node);
            i++;
        }
        

        // Se recorre la lista de triangulos de tres en tres, obteniendo los inidices de los vertices que forman triangulos
        for (int j = 0; j < triangles.Length; j += 3)
        {
            //Se crean las tres aristas que forman cada triangulo, se crea otra variable otherEdge que almacenara la arista ya introducida y corresponde con la peticion
            Edge otherEdge;
            Edge edge = new Edge(nodes[triangles[j]], nodes[triangles[j+1]], nodes[triangles[j+2]]);
            Edge edge2 = new Edge(nodes[triangles[j]], nodes[triangles[j+2]], nodes[triangles[j+1]]);
            Edge edge3 = new Edge(nodes[triangles[j+1]], nodes[triangles[j+2]], nodes[triangles[j]]);


            //En cada if else, se le pasa una de las aristas al diccionario, si ya esta introducida, nos devuelve la arista que estaba dentro, 
            // a partir de ella, podemos obtener con que tercer vertice se formaba ese triangulo, y podemos trazar el muelle de flexion, desde el tercer vertice de esa arista y hasta el tercer vertice de la 
            // que pretendiamos introducir.
            if (edgeDictionary.TryGetValue(edge, out otherEdge))
            {
                //La arista está en el diccionario
                Spring flex = new Spring(edge.nodeC, otherEdge.nodeC, flexiStiffness);
                springs.Add(flex);

            }
            else
            {
                //La arista no está en el diccionario
                edgeDictionary.Add(edge, edge);
                Spring spring = new Spring(edge.nodeA, edge.nodeB, stiffness);
                springs.Add(spring);
            }

            

            if (edgeDictionary.TryGetValue(edge2, out otherEdge))
            {
                //La arista está en el diccionario
                Spring flex = new Spring(edge2.nodeC, otherEdge.nodeC, flexiStiffness);
                springs.Add(flex);
            }
            else
            {
                //La arista no está en el diccionario
                edgeDictionary.Add(edge2, edge2);
                Spring spring = new Spring(edge2.nodeA, edge2.nodeB, stiffness);
                springs.Add(spring);
            }
            


            if (edgeDictionary.TryGetValue(edge3, out otherEdge))
            {
                //La arista está en el diccionario
                Spring flex = new Spring(edge3.nodeC, otherEdge.nodeC, flexiStiffness);
                springs.Add(flex);
            }
            else
            {
                //La arista no está en el diccionario
                edgeDictionary.Add(edge3, edge3);
                Spring spring = new Spring(edge3.nodeA, edge3.nodeB, stiffness);
                springs.Add(spring);
            }
        }
      

        // Proceso para añadir muelles hasta el requisito 3, no distingue entre duplicdas ni crea muelles de felxion. 
        /*
        
        for (int cont = 0; cont < triangles.Length; cont += 3)
        {

            Spring spring1 = new Spring(nodes[triangles[cont]], nodes[triangles[cont + 1]],stiffness);
            Spring spring2 = new Spring(nodes[triangles[cont + 1]], nodes[triangles[cont + 2]],stiffness);
            Spring spring3 = new Spring(nodes[triangles[cont + 2]], nodes[triangles[cont]],stiffness);
            springs.Add(spring1);
            springs.Add(spring2);
            springs.Add(spring3);
        }
        
        */

        
    }

    public void Update()
    {
        // Para cada nodo, se obtiene su posicion, y por cada objeto que se haya añadido a la lista de fixers llamada cubes, se calcula que nodos estan dentro de estos cubos, aquellos que cumplan 
        // esta condición pasa a estar fijados.
        for(int cont = 0; cont < vertices.Length; cont++)
        {
            //

            //pos = transform.InverseTransformPoint(nodes[i].pos);
            pos = nodes[cont].pos;
            foreach (GameObject cube in cubes)
            {
                //if (cube.GetComponent<Collider>().bounds.Contains(transform.TransformPoint(pos))) nodes[i].isFixed = true;
                if (cube.GetComponent<Collider>().bounds.Contains(pos)) nodes[cont].isFixed = true;

            }
        }
        if (Input.GetKeyUp(KeyCode.P))
            this.Paused = !this.Paused;

        //Procedure to update vertex positions

        // Se accede a la maya de la escena, para luego crear un array de posiciones aux, a este se le pasan las posiciones de nuestros nodos en este momento, la cual se actualiza mas adelante
        // en el codigo en el fixedUpdate, la nueva posicion se calcula a nivel de nodo pero la maya recibe vertices, por tanto pasamos a cada valor del array de vertices las nueva posiciones.
        // Estas las pasamos coordenadas locales de la maya y se las asignamos en la linea 222
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] verticesAux = new Vector3[mesh.vertexCount];

        for (int i = 0; i < vertices.Length; i++)
        {
            verticesAux[i] = nodes[i].pos;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            //verticesAux[0].x = 5;
            Vector3 pos = verticesAux[i];
            //Debug.Log("Vertices[" + i + "]: " + verticesAux[i]);
            vertices[i] = transform.InverseTransformPoint(pos);

        }


        mesh.vertices = vertices;
    }

    public void FixedUpdate()
    {
        if (this.Paused)
            return; // Not simulating

        // Select integration method
        switch (this.IntegrationMethod)
        {
            case Integration.Explicit: this.stepExplicit(); break;
            case Integration.Symplectic: this.stepSymplectic(); break;
            default:
                throw new System.Exception("[ERROR] Should never happen!");
        }

    }

    #endregion

    /// <summary>
    /// Performs a simulation step in 1D using Explicit integration.
    /// </summary>
    private void stepExplicit()
    {
        foreach (Node node in nodes)
        {
            node.force = Vector3.zero;
            node.force += mass * Gravity;
            node.force -= nodeDamping * node.vel;
        }
        foreach (Spring spring in springs)
        {
            spring.ComputeForces(springDamping);
        }

        foreach (Node node in nodes)
        {
            if (!node.isFixed)
            {
                node.pos += TimeStep * node.vel;
                node.vel += TimeStep / node.mass * (node.force);
                
            }
        }

        foreach (Spring spring in springs)
        {
            spring.UpdateLength();
        }
    }


    /// <summary>
    /// Performs a simulation step in 1D using Symplectic integration.
    /// </summary>
    private void stepSymplectic()
    {
        // Integra las fuerzas de los nodos, incluye amortiguamiento.
        foreach (Node node in nodes)
        {
            node.force = Vector3.zero;
            node.force += mass * Gravity;
            node.force -= nodeDamping * node.vel;
        }
        foreach (Spring spring in springs)
        {
            spring.ComputeForces(springDamping);
        }
        // Solo suma las fuerzas integradas de aquellos nodos que no estan fijados por el collider.
        foreach (Node node in nodes)
        {
            if (!node.isFixed)
            {
                node.vel += TimeStep / node.mass * (node.force);
                node.pos += TimeStep * node.vel;
            }
        }

        foreach (Spring spring in springs)
        {
            spring.UpdateLength();
        }
    }

}