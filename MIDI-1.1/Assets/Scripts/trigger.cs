using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public bool isTriggered;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }
}
