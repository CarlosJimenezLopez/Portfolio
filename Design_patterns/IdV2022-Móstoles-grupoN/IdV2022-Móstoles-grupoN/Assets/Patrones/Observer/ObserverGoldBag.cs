using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObserverGoldBag : MonoBehaviour,ISubject<float>
{

    [SerializeField] public float Gold = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        IPickableCoin coin = collision.GetComponent<IPickableCoin>();
        IPickableCoinSinDirty emerald = collision.GetComponent<IPickableCoinSinDirty>();

        if (coin != null)
        {
            Gold += coin.Pick();
            NotifyObservers();

        }

        if (emerald != null)
        {
            Gold += emerald.PickDirty();
            NotifyObservers();

        }
    }

    
    private List<IObserver<float>> _observers = new List<IObserver<float>>();

    public void AddObserver(IObserver<float> observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver<float> observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (IObserver<float> observer in _observers)
        {
            observer?.UpdateObserver(Gold);
        }
    }
   
}
