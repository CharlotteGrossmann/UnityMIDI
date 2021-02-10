using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
using MidiPlayerTK;
using System;

public class bezierController : MonoBehaviour
{
    private Transform[] notePoints; //only the points that are affected by melody component

    //imported variables
    public GameObject MidiMaker;
    private float velocity;
    private int playedMidiNote;
    private float pitch;
    private bool isPlaying;

    
    private float velocityMove;
    private Vector3 defaultPosition;
    private Vector3 defaultPosition1;
    public float defaultGap;
    public float[] melodyDifference = { 0, 0, 0, 0, 0, 0, 0, 0 }; //wave amplitude for each wave
    private bool isVisualized = false;
    private float pitchDifference;
    private float pitchMove;


    void Start()
    {
       

        var allPoints = GetComponentsInChildren<Transform>(); //get all spline points
        notePoints = new Transform[] { allPoints[2], allPoints[4], allPoints[6], allPoints[8], allPoints[10], allPoints[12], allPoints[14], allPoints[16]};
        
        defaultPosition = notePoints[0].transform.position; //position of the first point that corresponds to a note
        defaultPosition1 = notePoints[1].transform.position; //position of the second point that corresponds to a note
        defaultGap = -(defaultPosition.x - defaultPosition1.x); //calculate how big the gap is between notePoints


    }


    void Update()
    {
        velocity = MidiMaker.GetComponent<simpleMidiStream>().Velocity;
        playedMidiNote = MidiMaker.GetComponent<simpleMidiStream>().CurrentNote;
        pitch = MidiMaker.GetComponent<simpleMidiStream>().PitchChange;
        isPlaying = MidiMaker.GetComponent<simpleMidiStream>().isActive;

        velocityToWaves();
        notesToAmplitute();
        pitchToViz();
        setPosition();
       
        if (isPlaying)
            createVisualNote();
        else
            isVisualized = false;
        
    }

    void velocityToWaves()
    {
        velocityMove = velocity / 3f;

    }

    void notesToAmplitute()
    {
        var amplitude = 50f; //how much the amplitude of the respective wave should be risen

        for(var i = 0; i<8; i++)
        {
            melodyDifference[i] = 0; //sets everything wave amplitude to 0
            if(noteIndex()!=-1)
                melodyDifference[noteIndex()] = amplitude;
        }

    }

    void pitchToViz()
    {
            pitchDifference = -((64 - pitch) / 100)*40f; //translates pitch to deviancy on x-axis. Times 40 to scale it up to a visible difference
            if (pitchDifference != 0)
                pitchMove = 20f;
            else
                pitchMove = 0;
    }
    
    void setPosition() //calculate the position of all note points 
    {
        for (var i = 0; i<notePoints.Length; i++)
        {
            var x = (defaultPosition.x + defaultGap * i) + pitchDifference; 
            var y = defaultPosition.y + velocityMove + pitchMove + melodyDifference[i];
            var z = notePoints[i].transform.position.z;
            notePoints[i].transform.position = new Vector3(x, y, z);
        }
    }

    private int noteIndex()
    {
        switch (playedMidiNote) //sets the respective wave higher
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

    void createVisualNote()
    {
        if (!isVisualized&&noteIndex()!=-1) {  //creates a sphere at the tip of the highest wave when all components are active
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = notePoints[noteIndex()].transform.position;
            sphere.transform.localScale = new Vector3(30, 30, 30);
            sphere.AddComponent<noteFloater>();
            isVisualized = true;
        }

    }
}

