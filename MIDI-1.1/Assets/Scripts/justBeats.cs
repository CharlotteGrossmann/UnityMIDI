using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class justBeats : MonoBehaviour
{
    public Transform[] allBeats;
    public int i = 1;
    private bool beatOn = false;
    private bool first = true;
    private int lastBeat;
    private int lastMeassure;
    public Transform meassure;
    private Transform clone;
    public Transform BeatIndicator;
    private float movement;
    public Transform SpawnBeat;
    public Transform DestroyBeat;
    private float activeBeat;
    private Vector3 inactiveBeat;
    private float yDifference = 30;
    void Start()
    {
        allBeats = GetComponentsInChildren<Transform>();
        lastBeat = allBeats.Length - 1;


        activeBeat = allBeats[1].transform.position.y + yDifference;
        inactiveBeat = allBeats[1].transform.position;
    }

    void Update()
    {

        updateArray();
        // indicate Beats funktioniert noch nicht richtig
        if (!beatOn&&i<9)
        {
            Invoke("indicateBeats", 0.48f); //invoke the beat function every 0,49 seconds (in time with beat of the background music)
            beatOn = true;
        } 
        else if (beatOn&&i==9)
            Invoke("indicateBeats", 0.48f); //invoke the beat function every 0,49 seconds (in time with beat of the background music)

        moveBeats();



    }

    void indicateBeats()
    {
        if (beatOn)
        {
            for (var i = 0; i<allBeats.Length; i++)
            {
                allBeats[i].transform.position = new Vector3(allBeats[i].transform.position.x, inactiveBeat.y, inactiveBeat.z);
            }
            /*if (i != 1)
                allBeats[i - 1].transform.position = new Vector3(allBeats[i - 1].transform.position.x, inactiveBeat, allBeats[i - 1].transform.position.z);
            if(i==1 && !first)
                allBeats[lastBeat].transform.position = new Vector3(allBeats[lastBeat].transform.position.x, inactiveBeat, allBeats[lastBeat].transform.position.z);
            */
            allBeats[i].transform.position = new Vector3(allBeats[i].transform.position.x, activeBeat, inactiveBeat.z);

            
            if(i<9)
                i++;
            
            first = false;
            beatOn = false;
           
        }


    }


    void moveBeats()
    {
        movement = -140f * Time.deltaTime; //times time.deltaTime to apply per second not frame
        for(var j = 1; j < allBeats.Length; j ++)
        {
            allBeats[j].position = new Vector3(allBeats[j].position.x + movement, allBeats[j].position.y , allBeats[j].position.z); //let all beats float to the left
            if (allBeats[j].position.x < DestroyBeat.position.x) // if meassure is out of camera view
            {
                clone = Instantiate(meassure,SpawnBeat.position, Quaternion.identity); //create new
                clone.transform.SetParent(BeatIndicator.transform);

                Destroy(allBeats[j].gameObject); //destroy object when it's not in the camera view anymore
                
            }
        }
        
    }

    void updateArray()
    {
        allBeats = GetComponentsInChildren<Transform>(); //array length differs so we have to update it
        lastBeat = allBeats.Length - 1;
    }

}


