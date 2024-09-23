using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard: MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        //Revisar porque ùede que solo sea para el 3d
        transform.LookAt(Camera.main.transform);
    }
}
