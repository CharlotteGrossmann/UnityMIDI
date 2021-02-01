using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
using MidiPlayerTK;
using System;
using System.Linq;

public class bezierController : MonoBehaviour
{
    private Transform[] notePoints; //only the points that are affected by melody component

    public GameObject MidiMaker;
    private float velocity;
    private int playedMidiNote;
    private float sustain;
    private float pitch;
    private bool isPlaying;

    private float velocityMove;
    private Vector3 defaultPosition;
    private float[] melodyDifference = { 0, 0, 0, 0, 0, 0, 0, 0 }; //wave amplitude for each wave
    private bool isVisualized = false;
    private float pitchDifference = 0;

    void Start()
    {
        var allPoints = GetComponentsInChildren<Transform>(); //get all spline points
        notePoints = new Transform[] { allPoints[2], allPoints[4], allPoints[6], allPoints[8], allPoints[10], allPoints[12], allPoints[14], allPoints[16]};
        
        defaultPosition = allPoints[2].transform.position; //the y position for each point is the same, so we can get it from allPoints[2] and use it for every one 
    }
    
 
    void Update()
    {
        velocity = MidiMaker.GetComponent<simpleMidiStream>().Velocity;
        playedMidiNote = MidiMaker.GetComponent<simpleMidiStream>().CurrentNote;
        sustain = MidiMaker.GetComponent<simpleMidiStream>().mySustain;
        pitch = MidiMaker.GetComponent<simpleMidiStream>().PitchChange;
        isPlaying = MidiMaker.GetComponent<simpleMidiStream>().isActive;

        velocityToWaves();
        notesToAmplitute();
        sustainToViz();
        pitchToViz();
        setPosition();
        /*if ((pitch != 64||sustain != 0) && playedMidiNote > 0 && velocity > 0)
            createVisualNote();
        
        else
            isVisualized = false;*/
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
            melodyDifference[noteIndex()] = amplitude;
        }

    }

    void sustainToViz()
    {
        //sustain;
    }
    void pitchToViz()
    {
        /*
        if (pitch < 64)
            pitchDifference = -(pitch / 10f);
        else if (pitch > 64)
            pitchDifference = pitch / 10;
        else
            pitchDifference = 0;*/
    }
    
    // schlauer lösen
    void setPosition() //calculate the position of all note points 
    {
        for (var i = 0; i<notePoints.Length; i++)
        {
            notePoints[i].transform.position = new Vector3(notePoints[i].transform.position.x + pitchDifference, defaultPosition.y + velocityMove + melodyDifference[i], notePoints[i].transform.position.z);

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
        if (!isVisualized) {  //creates a sphere at the tip of the highest wave when all components are active
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = notePoints[noteIndex()].transform.position;
            sphere.transform.localScale = new Vector3(30, 30, 30);
            sphere.AddComponent<noteFloater>();
            isVisualized = true;
        }

    }
}

