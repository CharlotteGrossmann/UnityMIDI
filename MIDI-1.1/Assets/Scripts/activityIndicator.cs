using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

public class activityIndicator : MonoBehaviour
{
    public MeshRenderer MelodyIndicator;
    public MeshRenderer RhythmIndicator;
    public MeshRenderer ModulateIndicator;

    public GameObject MidiMaker;
    private float velocity;
    private int playedMidiNote;
    private float sustain;
    private float pitch;
    private bool isPlaying;

    void Update()
    {
        velocity = MidiMaker.GetComponent<simpleMidiStream>().Velocity;
        playedMidiNote = MidiMaker.GetComponent<simpleMidiStream>().CurrentNote;
        sustain = MidiMaker.GetComponent<simpleMidiStream>().mySustain;
        pitch = MidiMaker.GetComponent<simpleMidiStream>().PitchChange;
        isPlaying = MidiMaker.GetComponent<simpleMidiStream>().isActive;

        if (playedMidiNote > 0) 
            MelodyIndicator.enabled = true;
        else
            MelodyIndicator.enabled = false;

        if (velocity != 0)            
            RhythmIndicator.enabled = true;
        else
            RhythmIndicator.enabled = false;

        if (/*sustain != 500||*/pitch != 64) 
            ModulateIndicator.enabled = true;
        else
            ModulateIndicator.enabled = false;


    }
}
