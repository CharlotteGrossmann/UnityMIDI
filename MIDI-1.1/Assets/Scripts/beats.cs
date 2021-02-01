using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beats : MonoBehaviour
{
    public Transform[] allBeats;
    private int i = 2;
    private bool beatOn = false;
    private bool first = true;
    private int lastBeat;
    private int lastMeassure;
    public Transform meassure;
    private Vector3 lastBeatPosition;
    private Vector3 spawnPoint;
    private Transform clone;
    public Transform BeatIndicator;
    public Vector3 lastMeassurePosition;
    private float movement;
    private bool newBeat = false;
    void Start()
    {
        allBeats = GetComponentsInChildren<Transform>();
        lastBeat = allBeats.Length - 1;
        lastMeassure = allBeats.Length - 5;
        lastBeatPosition = allBeats[lastBeat].position;
        spawnPoint = new Vector3(lastBeatPosition.x + 80f , lastBeatPosition.y, lastBeatPosition.z);

        lastMeassurePosition = allBeats[11].transform.position;

    }

    void Update()
    {
        allBeats = GetComponentsInChildren<Transform>(); //array length differs so we have to update it
        
        moveBeats();
        if (!beatOn)
        {
            
            Invoke("indicateBeats", 0.48f); //invoke the beat function every 0,49 seconds (in time with beat of the background music)
            beatOn = true;
        }
        /* if (!newBeat)

             Invoke("buildBeats", 1f);
             newBeat = false;
         }*/
        buildBeats();


    }

    void indicateBeats()
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
                i = 2;
            if (i == 1 || i == 6 || i == 11 || i == 16)
                i += 1;
            first = false;
            beatOn = false;
           
        }


    }

    void buildBeats()
    {
        /*if (newBeat)
        {*/
        if (allBeats[lastMeassure].position.x < 700 && allBeats[lastMeassure].position.x > 650 )
        {
            if (newBeat)
            {
                clone = Instantiate(meassure, spawnPoint, Quaternion.identity);
                clone.transform.SetParent(BeatIndicator.transform);
                newBeat = false;
            }
            
        }
        else
            newBeat = true;
           /* newBeat = false;
        }*/
        
    }

    void moveBeats()
    {
        movement -= 0.5f * Time.deltaTime; //times time.deltaTime to apply per second not frame
        for(var j = 1; j < allBeats.Length; j += 5)
        {
            allBeats[j].position = new Vector3(allBeats[j].position.x + movement, allBeats[j].position.y , allBeats[j].position.z); //let all beats float upwards
            if (allBeats[j].transform.position.x < -602)//destory if meassure is out of camera view
                Destroy(allBeats[j].gameObject); //destroy object when it's not in the camera view anymore
        }
        
    }
}


