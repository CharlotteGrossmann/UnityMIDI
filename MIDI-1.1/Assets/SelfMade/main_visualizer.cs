using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_visualizer : MonoBehaviour
{
    public GameObject Instrument; //Import MessageProcessor.cs

    private int PitchChange; //Modulation der Tonhöhe -> Joystick
    private int SustainChange; //Modulation des Klangs -> Joystick
    private int NoteChange; //Melodie -> Buttons
    private int PressureChange; //Rythmus -> Force Sensitive Resistor

    //color the notes
    private ParticleSystem ps;
    public Color CColor = new Color(0, 1, 1, 1);
    public Color DColor = new Color(0, 0, 1, 1);
    public Color EColor = new Color(1, 0, 0, 1);
    public Color FColor = new Color(1, 1, 0, 1);
    public Color GColor = new Color(1, 0, 1, 1);
    public Color AColor = new Color(0, 0, 0, 1);
    public Color BColor = new Color(1, 1, 1, 1);
    public Color C1Color = new Color(0.5f, 1, 1, 1);


    // Start is called before the first frame update
    void Start()
    {
        //Variablen zuordnen zu Variablen aus MessageProcessor.cs
        PitchChange = Instrument.GetComponent<MessageProcessor>().pitch;
        PressureChange = Instrument.GetComponent<MessageProcessor>().pressure;
        NoteChange = Instrument.GetComponent<MessageProcessor>().note;
        SustainChange = Instrument.GetComponent<MessageProcessor>().sustain;

        ps = GetComponent<ParticleSystem>();
        


    }

    // Update is called once per frame
    void Update()
    {
        var main = ps.main;

        if (NoteChange == 54)
            main.startColor = CColor;

        else if (NoteChange == 52)
            main.startColor = DColor;

        else if (NoteChange == 51)
            main.startColor = EColor;

        else if (NoteChange == 49)
            main.startColor = FColor;

        else if (NoteChange == 47)
            main.startColor = GColor;

        else if (NoteChange == 46)
            main.startColor = AColor;

        else if (NoteChange == 44)
            main.startColor = BColor;

        else if (NoteChange == 42)
            main.startColor = C1Color;
        else
            main.startColor = new Color(0, 0, Random.Range(0.5f, 1f), 1);
    }
    /*void OnGUI()
    {
        GUI.Label(new Rect(25, 40, 100, 30), "Red");
        GUI.Label(new Rect(25, 70, 100, 30), "Green");
        GUI.Label(new Rect(25, 100, 100, 30), "Blue");
        GUI.Label(new Rect(25, 130, 100, 30), "Alpha");

        hSliderValueR = GUI.HorizontalSlider(new Rect(95, 45, 100, 30), hSliderValueR, 0.0F, 1.0F);
        hSliderValueG = GUI.HorizontalSlider(new Rect(95, 75, 100, 30), hSliderValueG, 0.0F, 1.0F);
        hSliderValueB = GUI.HorizontalSlider(new Rect(95, 105, 100, 30), hSliderValueB, 0.0F, 1.0F);
        hSliderValueA = GUI.HorizontalSlider(new Rect(95, 135, 100, 30), hSliderValueA, 0.0F, 1.0F);
    }*/
}