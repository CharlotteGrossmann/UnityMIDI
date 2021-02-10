using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
using MidiPlayerTK;
using System;

public class BezierController : MonoBehaviour
{
   //imported variables
    public GameObject midiMaker;

    private float pitch;
    private float velocity;

    private int playedMidiNote;
    
    private bool isPlaying;

    //only the points from the spline that are affected by melody component
    private Transform[] notePoints; //keep in mind! notePoits[0] is not the first beat but the parent object



    private Vector3 defaultPosition;
    private Vector3 defaultPosition1;

    private float[] melodyDifference = { 0, 0, 0, 0, 0, 0, 0, 0 }; //wave amplitude for each wave
    public float defaultGap;
    private float velocityMove;
    private float pitchDifference;
    private float pitchMove;

    private bool isVisualized = false;
    


    void Start()
    {

        //get all spline points and push the moveable ones into notePoints[]
        var allPoints = GetComponentsInChildren<Transform>(); 
        notePoints = new Transform[] { allPoints[2], allPoints[4], allPoints[6], allPoints[8], allPoints[10], allPoints[12], allPoints[14], allPoints[16]};

        //calculate how big the gap is between two notePoints
        defaultPosition = notePoints[0].transform.position; 
        defaultPosition1 = notePoints[1].transform.position; 
        defaultGap = -(defaultPosition.x - defaultPosition1.x); 


    }


    void Update()
    {
        //get component activty from MidiStream
        velocity = midiMaker.GetComponent<SimpleMidiStream>().velocity;
        playedMidiNote = midiMaker.GetComponent<SimpleMidiStream>().currentNote;
        pitch = midiMaker.GetComponent<SimpleMidiStream>().pitchChange;
        isPlaying = midiMaker.GetComponent<SimpleMidiStream>().isActive;

        VelocityToWaves();
        NotesToAmplitute();
        PitchToViz();
        SetPosition();
       
        if (isPlaying)
            CreateVisualNote();
        else //Make sure only one note is created
            isVisualized = false; 
        
    }

    void VelocityToWaves() //velocity directly translates to the amplitutes
    {
        velocityMove = velocity / 3f;

    }

    void NotesToAmplitute()
    {
        var amplitude = 50f; //how much the amplitude of the respective wave should be risen

        for(var i = 0; i<8; i++)//set every notePoint to default position
        {
            melodyDifference[i] = 0; 

            if(NoteIndex()!=-1)
                melodyDifference[NoteIndex()] = amplitude; //set active notePoint to higher position
        }

    }

    void PitchToViz()
    {
            //translates pitch to deviancy on x-axis from the default position. 
            //Times 40 to scale it up to a visible difference
            pitchDifference = -((64 - pitch) / 100)*40f; 
            
            if (pitchDifference != 0)
                pitchMove = 20f;
            else
                pitchMove = 0;
    }
    
    void SetPosition() //calculate the position of all note points 
    {
        for (var i = 0; i<notePoints.Length; i++)
        {
            //X takes into account the deviancy created by differences in Pitch
            var x = (defaultPosition.x + defaultGap * i) + pitchDifference; 

            //Y takes into account the pitch, velocity and melody 
            var y = defaultPosition.y + velocityMove + pitchMove + melodyDifference[i];

            //Z stays the same always
            var z = notePoints[i].transform.position.z;

            notePoints[i].transform.position = new Vector3(x, y, z);
        }
    }

    private int NoteIndex()
    {
        switch (playedMidiNote) //translate Midi Note to notePoint Index
        {
            case 42:
                return 0;
            case 44:
                return 1;
            case 46:
                return 2;
            case 47:
                return 3;
            case 49:
                return 4;
            case 51:
                return 5;
            case 53:
                return 6;
            case 54:
                return 7;
            default:
                return -1;
        }
    }

    void CreateVisualNote()
    {
        if (!isVisualized&&NoteIndex()!=-1) {  //creates a sphere at the tip of the highest wave when all components are active
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = notePoints[NoteIndex()].transform.position;
            sphere.transform.localScale = new Vector3(30, 30, 30);

            //adds NoteFloater script so it floats upwards
            sphere.AddComponent<NoteFloater>();
            isVisualized = true;
        }

    }
}

