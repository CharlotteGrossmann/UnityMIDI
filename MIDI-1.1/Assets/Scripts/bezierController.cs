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
    public float velocity;
    public int playedNote;
    private float sustain;
    private float pitch;

    private float velocityMove;
    private Vector3 defaultPosition;
    public float[] melodyDifference = { 0, 0, 0, 0, 0, 0, 0, 0 }; //wave amplitude for each wave

    void Start()
    {
        var allPoints = GetComponentsInChildren<Transform>(); //get all spline points
        notePoints = new Transform[] { allPoints[2], allPoints[4], allPoints[6], allPoints[8], allPoints[10], allPoints[12], allPoints[14], allPoints[16]};
        
        defaultPosition = allPoints[2].transform.position; //the y position for each point is the same, so we can get it from allPoints[2] and use it for every one 
    }
    
 
    void Update()
    {
        velocity = MidiMaker.GetComponent<simpleMidiStream>().Velocity;
        playedNote = MidiMaker.GetComponent<simpleMidiStream>().CurrentNote;
        sustain = MidiMaker.GetComponent<simpleMidiStream>().mySustain;
        pitch = MidiMaker.GetComponent<simpleMidiStream>().PitchChange;

        velocityToWaves();
        notesToAmplitute();
        sustainToViz();
        pitchToViz();
        setPosition();

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

            switch (playedNote) //sets the respective wave higher
            {
                case 42:
                    melodyDifference[0] = amplitude;
                    break;
                case 44:
                    melodyDifference[1] = amplitude;
                    break;
                case 46:
                    melodyDifference[2] = amplitude;
                    break;
                case 47:
                    melodyDifference[3] = amplitude;
                    break;
                case 49:
                    melodyDifference[4] = amplitude;
                    break;
                case 51:
                    melodyDifference[5] = amplitude;
                    break;
                case 53:
                    melodyDifference[6] = amplitude;
                    break;
                case 54:
                    melodyDifference[7] = amplitude;
                    break;
                default:
                    break;
            }
        }

    }

    void sustainToViz()
    {
        //sustain;
    }
    void pitchToViz()
    {
        //pitch;
    }
    
    // schlauer lösen
    void setPosition() //calculate the position of all note points 
    {
        for (var i = 0; i<notePoints.Length; i++)
        {
            notePoints[i].transform.position = new Vector3(notePoints[i].transform.position.x, defaultPosition.y + velocityMove + melodyDifference[i], notePoints[i].transform.position.z);

        }
    }

}

