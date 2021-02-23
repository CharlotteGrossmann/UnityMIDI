using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beats : MonoBehaviour
{
    //define in Unity
    public Transform oneBeat; //the prefab for spawning new beats

    public GameObject Messure; //Main Visualizer

    public Transform parent;


    //all beats in an array
    private Transform[] allBeats; //ATTENTION! allBeats[0] is not the first beat but the parent object

    private int i = 1; //to use as index in allBeats



    //spawning and destroying beats
    public Transform spawnBeat;
    public Transform destroyBeat;
    public Transform beatIndicator;


    //for beat movement X-Axis
    private float bpm; //Beats per Minute
    private float mps; //Meassure per Second
    private float movement; 

    public Vector3 meassureLength;
    private Vector3 lastBeatInOneMeassure;
    private Vector3 lastBeatInAnotherMeassure;
    

    //for beat movement Y-Axis
    private bool isBeatIndicated = false;


    private float activeBeat; //yposition of the active beat
    private float yDifference = 30f; //how much the active beat is lifted up

    private Vector3 inactiveBeat; //position of inactive beats

    private float scale; //scaling of the group 

    void Start()
    {
        allBeats = GetComponentsInChildren<Transform>(); //push all beats into the array

        scale = parent.transform.localScale.y; //get the scale from the parents transform
        activeBeat = allBeats[1].position.y + yDifference*scale;   //calculate the position for active beats
        inactiveBeat = allBeats[1].position; //save position of inactive beats

        //calculate the length of a meassure
        lastBeatInOneMeassure = allBeats[4].position;         
        lastBeatInAnotherMeassure = allBeats[8].position;
        meassureLength.x = lastBeatInOneMeassure.x - lastBeatInAnotherMeassure.x;

        bpm = Messure.GetComponent<MeassureRotator>().bpm; //get bpm from messure rotator in main view
        mps = (bpm / 60f) / 4f; //Beats per Minute / 60 Seconds = Beats per 1 Second / 4 = Meassure per 1 Second
    }

    void Update()
    {
        if (!isBeatIndicated)
        {
            Invoke("IndicateBeats", (bpm / 60f)); //invoke the beat function in time with the beat of the background music (second parameter is time in seconds)
            isBeatIndicated = true;
        }

        MoveBeats();
    }

    void IndicateBeats()
    {
        if (isBeatIndicated)
        {
           for (var k = 0; k<allBeats.Length; k++) //set all beats to inactive position
           {
               allBeats[k].position = new Vector3(allBeats[k].position.x, inactiveBeat.y, inactiveBeat.z); //since all beats move to the left, x has to be a variable
           }
           allBeats[i].position = new Vector3(allBeats[i].position.x, activeBeat, inactiveBeat.z); //set active beat to active position
            
            //make i loop trough the array
            if (i<allBeats.Length) 
                i++;
            if (i == allBeats.Length)
                i = 1;
            isBeatIndicated = false; //ensure the function is called just once
            
        }

    }


    void MoveBeats()
    {
        movement = meassureLength.x * (Time.deltaTime*mps); //let it move in time with the background music. time.deltaTime to apply per second not frame
        for(var j = 1; j < allBeats.Length; j ++) //let all beats float to the left
        {
            allBeats[j].position = new Vector3(allBeats[j].position.x + movement, allBeats[j].position.y , allBeats[j].position.z); 

            if (allBeats[j].position.x < destroyBeat.position.x) // if meassure is out of camera view
                allBeats[j].position = new Vector3 (spawnBeat.position.x, allBeats[j].position.y, allBeats[j].position.z); //teleport it to the spawn point
         
        }
        
    }

}


