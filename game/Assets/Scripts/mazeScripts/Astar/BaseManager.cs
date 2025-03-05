using System.ComponentModel;
using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
            Destroy(gameObject);
    }
}