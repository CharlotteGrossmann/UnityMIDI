using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beats : MonoBehaviour
{
    private Transform[] allBeats;
    public int i = 1;
    private bool beatOn = false;
    private bool first = true;
    void Start()
    {
        allBeats = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        
        if (!beatOn)
        {
            Invoke("BeatBeats", 1f);
            beatOn = true;
        }
      
       
    }

    void BeatBeats()
    {
        if (beatOn)
        {
            if (i != 1)
                allBeats[i - 1].transform.position = new Vector3(allBeats[i - 1].transform.position.x, allBeats[i-1].transform.position.y - 30f, allBeats[i - 1].transform.position.z);
            if(i==1 && !first)
                allBeats[12].transform.position = new Vector3(allBeats[12].transform.position.x, allBeats[12].transform.position.y - 30f, allBeats[12].transform.position.z);

            allBeats[i].transform.position = new Vector3(allBeats[i].transform.position.x, allBeats[i].transform.position.y+30f , allBeats[i].transform.position.z);
            
            if(i<allBeats.Length)
                i+=1;
            if (i == allBeats.Length)
                i = 1;
            first = false;
            beatOn = false;
            
        }
        

    }
}


