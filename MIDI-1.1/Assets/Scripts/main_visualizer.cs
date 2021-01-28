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

    private ParticleSystem ps;
    //Jeder Note eine Farbe zuordnen (R,G,B,Alpha):
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
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        //Variablen zuordnen zu Variablen aus MessageProcessor.cs
        PitchChange = Instrument.GetComponent<MessageProcessor>().pitch;
        PressureChange = Instrument.GetComponent<MessageProcessor>().pressure;
        NoteChange = Instrument.GetComponent<MessageProcessor>().note;
        SustainChange = Instrument.GetComponent<MessageProcessor>().sustain;

        //ps = particle system
        var main = ps.main;

        if (NoteChange == 54)
        {
            //main.startColor = CColor;
            // GameObject go1 = new GameObject("go1", typeof(Rigidbody), typeof(BoxCollider));
            GameObject go = new GameObject("test");

            go.transform.position = new Vector3(0, 1, 0);

            Color objectColor = new Color(1, 0, 0, 1);

            MeshRenderer gameObjectRenderer = go.AddComponent<MeshRenderer>();

            Material newMaterial = new Material(Shader.Find("Diffuse"));

            newMaterial.color = objectColor;
            gameObjectRenderer.material = newMaterial;
        }
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
    }
}