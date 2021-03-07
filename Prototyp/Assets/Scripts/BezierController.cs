using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
using MidiPlayerTK;
using System;

public class BezierController : MonoBehaviour
{
    //asign in unity
    public Transform parent;
    public GameObject MidiStream;

    public bool hasModulation = false;

    //
    private float scale;
    private bool isVisualized = false;
    private bool isPlaying;

    //VISUALIZATION

    //visualized variables
    private float pitch;
    private float velocity;
    private int playedMidiNote;

    //visualizing variables
    private float velocityMove;
    private float pitchXDifference = 0;
    private float pitchYDifference;
    private float[] melodyDifference = { 0, 0, 0, 0, 0, 0, 0, 0 }; //wave amplitude for each wave


    //spline
    private Transform[] notePoints; //ATTENTION! notePoits[0] is not the first beat but the parent object

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float defaultGap;


    void Start()
    {
        //get all spline points and push the moveable ones into notePoints[]
        var allPoints = GetComponentsInChildren<Transform>();
        notePoints = new Transform[] { allPoints[2], allPoints[4], allPoints[6], allPoints[8], allPoints[10], allPoints[12], allPoints[14], allPoints[16] };

        //calculate how big the gap is between two notePoints
        startPosition = notePoints[0].transform.position;
        endPosition = notePoints[1].transform.position;
        defaultGap = -(startPosition.x - endPosition.x);

        //get the scale from the parent
        scale = parent.transform.localScale.y;

    }


    void Update()
    {
        //get live component activty from MidiStream
        velocity = MidiStream.GetComponent<SimpleMidiStream>().velocity;
        playedMidiNote = MidiStream.GetComponent<SimpleMidiStream>().currentNote;
        pitch = MidiStream.GetComponent<SimpleMidiStream>().pitchChange;
        //know when a MIDI note is played
        isPlaying = MidiStream.GetComponent<SimpleMidiStream>().isPlaying;

        //call the visualization functions for each component
        VelocityToWaves();
        NotesToAmplitute();
        if (hasModulation) //only call the function if the instrument has a modulation component
            PitchToViz();

        SetPosition(); //apply visualization to the spline

        if (isPlaying) //creates a visual note if a MIDI note is played
            CreateVisualNote();
        
        else //Make sure only one note is created
            isVisualized = false;

    }

    void VelocityToWaves() //higher velocity/volume = higher wave amplitudes
    {
        velocityMove = velocity / (3f / scale);

    }

    void NotesToAmplitute()
    {
        var amplitude = 50f * scale; //how much the amplitude of the respective wave should be raised

        for (var i = 0; i < 8; i++)//set every notePoint to default position
        {
            melodyDifference[i] = 0;

            if (NoteIndex() != -1) //NoteIndex returs 0-8 respectively to what note in the octave is played
                melodyDifference[NoteIndex()] = amplitude; //set active notePoint to higher position
        }

    }

    void PitchToViz()
    {
        //translates pitch to deviancy on x-axis from the default position. 
        pitchXDifference = ((pitch - 64f ) * scale);

        if (pitchXDifference != 0)
            pitchYDifference = 20f * scale;
        else
            pitchYDifference = 0;
    }

    void SetPosition() //calculate the position of all note points 
    {
        for (var i = 0; i < notePoints.Length; i++)
        {
            //X takes into account the deviancy created by differences in Pitch
            var x = (startPosition.x + defaultGap * i) + pitchXDifference;

            //Y takes into account the pitch, velocity and melody 
            var y = startPosition.y + velocityMove + pitchYDifference + melodyDifference[i];

            //Z stays the same always
            var z = notePoints[i].transform.position.z;

            notePointsS.transform.position = new Vector3(x, y, z);
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
        if (!isVisualized && NoteIndex() != -1)
        {  //creates a sphere at the tip of the highest wave when all components are active
            var noteRadius = 30f * scale;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = notePoints[NoteIndex()].transform.position;
            sphere.transform.localScale = new Vector3(noteRadius, noteRadius, noteRadius);

            //adds NoteFloater script so it floats upwards
            sphere.AddComponent<NoteFloater>();

            isVisualized = true;
        }

    }
}

