using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beats : MonoBehaviour
{
    //define in Unity
    public Transform oneBeat; //the prefab for spawning new beats

    public GameObject MainViz; //Main Visualizer
    
    private Transform[] allBeats; //keep in mind! allBeats[0] is not the first beat but the parent object

    private int i = 1; //to use as index in allBeats


    //spawning and destroying beats
    public Transform spawnBeat;
    public Transform destroyBeat;
    public Transform beatIndicator;

    private Transform clone;


    //for beat movement X-Axis
    private float bpm;
    private float mps; //Meassure per Second
    private float movement;

    public Vector3 meassureLength;
    private Vector3 lastBeatInOneMeassure;
    private Vector3 lastBeatInAnotherMeassure;
    

    //for beat movement Y-Axis
    private bool isBeatOn = false;


    private float activeBeat;
    private float yDifference = 0.3f;

    private Vector3 inactiveBeat;



    void Start()
    {
        allBeats = GetComponentsInChildren<Transform>(); //push all beats into the array


        activeBeat = allBeats[1].transform.position.y + yDifference;   //determine what positions inactive and active beats should have
        inactiveBeat = allBeats[1].transform.position;

        lastBeatInOneMeassure = allBeats[4].transform.position;         //calculate the length of a meassure
        lastBeatInAnotherMeassure = allBeats[8].transform.position;
        meassureLength.x = lastBeatInOneMeassure.x - lastBeatInAnotherMeassure.x;

        bpm = MainViz.GetComponent<MeassureRotator>().bpm;
        mps = (bpm / 60f) / 4f; //Beats per Minute / 60 Seconds = Beats per 1 Second / 4 = Meassure per 1 Second

    }

    void Update()
    {

        UpdateArray();
        
        


    }

    void lateUpdate()
    {
        if (!isBeatOn && i <= 9)
        {
            Invoke("IndicateBeats", (bpm / 60f)); //invoke the beat function in time with the beat of the background music)
            isBeatOn = true;
        }

        MoveBeats();
    }

    void IndicateBeats()
    {
       
        if (isBeatOn)
        {
            for (var k = 0; k<allBeats.Length; k++) //set all beats to inactive position
            {
                allBeats[k].transform.position = new Vector3(allBeats[k].transform.position.x, inactiveBeat.y, inactiveBeat.z);
            }
            allBeats[i].transform.position = new Vector3(allBeats[i].transform.position.x, activeBeat, inactiveBeat.z); //set active beat to active position

            
            if(i<9) //as long as i<9 make i bigger, after that i will always be 9 which is roughly the beat in the middle of the screen
                i++;
            
            isBeatOn = false; //ensure the function is called just once
            
           
        }


    }


    void MoveBeats()
    {
        movement = meassureLength.x * (Time.deltaTime*mps); //let it move in time with the background music. time.deltaTime to apply per second not frame
        for(var j = 1; j < allBeats.Length; j ++) //let all beats float to the left
        {
            allBeats[j].position = new Vector3(allBeats[j].position.x + movement, allBeats[j].position.y , allBeats[j].position.z); 
            if (allBeats[j].position.x < destroyBeat.position.x) // if meassure is out of camera view
            {
                clone = Instantiate(oneBeat,spawnBeat.position, Quaternion.identity); //create new beat
                clone.transform.SetParent(beatIndicator.transform);

                Destroy(allBeats[j].gameObject); //destroy object when it's not in the camera view anymore
                
            }
        }
        
    }

    void UpdateArray()
    {
        allBeats = GetComponentsInChildren<Transform>(); //array length increases in the begining so we have to update it
        
    }

}


