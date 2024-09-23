using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObserverGoldInfo : MonoBehaviour, IObserver<float>
{
   
    

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        ObserverGoldBag observerGoldBag = player.GetComponent<ObserverGoldBag>();
        observerGoldBag.AddObserver(this);
        
    }

    public void UpdateObserver(float data)
    {
       
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = $"{data}";
        
    }
}
