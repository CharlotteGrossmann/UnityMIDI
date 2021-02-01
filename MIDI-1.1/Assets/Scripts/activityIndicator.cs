using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activityIndicator : MonoBehaviour
{
    public GameObject Instrument; //Import MessageProcessor.cs
    public MeshRenderer MelodyIndicator;
    public MeshRenderer RhythmIndicator;
    public MeshRenderer ModulateIndicator;

    public int PitchChange; //Modulation der Tonhöhe -> Joystick
    public int SustainChange; //Modulation des Klangs -> Joystick
    public int NoteChange; //Melodie -> Buttons
    private int PressureChange; //Rythmus -> Force Sensitive Resistor
   
    void Update()
    {
        PitchChange = Instrument.GetComponent<MessageProcessor>().pitch;
        PressureChange = Instrument.GetComponent<MessageProcessor>().pressure;
        NoteChange = Instrument.GetComponent<MessageProcessor>().note;
        SustainChange = Instrument.GetComponent<MessageProcessor>().sustain;

        if (NoteChange != -1) 
            MelodyIndicator.enabled = true;
        else
            MelodyIndicator.enabled = false;

        if (PressureChange != 0)            
            RhythmIndicator.enabled = true;
        else
            RhythmIndicator.enabled = false;

        if (/*SustainChange != 500||*/PitchChange != 500) 
            ModulateIndicator.enabled = true;
        else
            ModulateIndicator.enabled = false;


    }
}
