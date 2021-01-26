using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_visualizer : MonoBehaviour
{
    public GameObject Instrument; //Import MessageProcessor.cs
    //public GameObject Note;

    private int PitchChange; //Modulation der Tonhöhe -> Joystick
    private int SustainChange; //Modulation des Klangs -> Joystick
    private int NoteChange; //Melodie -> Buttons
    private int PressureChange; //Rythmus -> Force Sensitive Resistor
    public Color col;

    // Start is called before the first frame update
    void Start()
    {
        //Variablen zuordnen zu Variablen aus MessageProcessor.cs
        PitchChange = Instrument.GetComponent<MessageProcessor>().pitch;
        PressureChange = Instrument.GetComponent<MessageProcessor>().pressure;
        NoteChange = Instrument.GetComponent<MessageProcessor>().note;
        SustainChange = Instrument.GetComponent<MessageProcessor>().sustain;

        ParticleSystem Note = GetComponent<ParticleSystem>();
        col = Note.colorOverLifetime.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(NoteChange == 54)
        {

            col.color = Color.red;
        }
            
    }
}