using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beats : MonoBehaviour
{
    private Transform[] allBeats;
    private int i = 1;
    private bool beatOn = false;
    private bool first = true;
    private int lastBeat;
    void Start()
    {
        allBeats = GetComponentsInChildren<Transform>();
        lastBeat = allBeats.Length - 1;

        
    }

    void Update()
    {

        if (!beatOn)
        {
            Invoke("BeatBeats", 0.48f); //invoke the beat function every 0,49 seconds (in time with beat of the background music)
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
                allBeats[lastBeat].transform.position = new Vector3(allBeats[lastBeat].transform.position.x, allBeats[lastBeat].transform.position.y - 30f, allBeats[lastBeat].transform.position.z);

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


