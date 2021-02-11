using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

public class ActivityIndicator : MonoBehaviour
{
    //assign in Unity
    public MeshRenderer melodyIndicator;
    public MeshRenderer rhythmIndicator;
    public MeshRenderer modulateIndicator;
    public GameObject midiMaker;

    //components
    private float velocity;
    private int playedMidiNote;
    private float pitch;

    void Update()
    {
        //get component activity from MidiStream
        velocity = midiMaker.GetComponent<SimpleMidiStream>().velocity;
        playedMidiNote = midiMaker.GetComponent<SimpleMidiStream>().currentNote;
        pitch = midiMaker.GetComponent<SimpleMidiStream>().pitchChange;

        //logic to activate/deactivate Indicators
        if (playedMidiNote > 0) 
            melodyIndicator.enabled = true;
        else
            melodyIndicator.enabled = false;

        if (velocity != 0)            
            rhythmIndicator.enabled = true;
        else
            rhythmIndicator.enabled = false;

        if (pitch != 64) 
            modulateIndicator.enabled = true;
        else
            modulateIndicator.enabled = false;


    }
}
