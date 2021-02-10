using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVisualizer : MonoBehaviour
{
    public GameObject Instrument; //Import MessageProcessor.cs

    private int pitchChange; //Modulation der Tonhöhe -> Joystick
    private int sustainChange; //Modulation des Klangs -> Joystick
    private int noteChange; //Melodie -> Buttons
    private int pressureChange; //Rythmus -> Force Sensitive Resistor

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

    public bool isActiveButton = false;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        //Variablen zuordnen zu Variablen aus MessageProcessor.cs
        pitchChange = Instrument.GetComponent<MessageProcessor>().pitch;
        pressureChange = Instrument.GetComponent<MessageProcessor>().pressure;
        noteChange = Instrument.GetComponent<MessageProcessor>().note;
        sustainChange = Instrument.GetComponent<MessageProcessor>().sustain;

        //ps = particle system
        var main = ps.main;

        if (noteChange == 54 && !isActiveButton)
        {
            isActiveButton = true;
            NewGameObjectCube(); //Aufruf Funktion
        }
        else if (noteChange == 53 && !isActiveButton)
        {
            isActiveButton = true;
            NewGameObjectSphere(); //Aufruf Funktion
        }
        else if (noteChange == 51)
            main.startColor = EColor;

        else if (noteChange == 49)
            main.startColor = FColor;

        else if (noteChange == 47)
            main.startColor = GColor;

        else if (noteChange == 46)
            main.startColor = AColor;

        else if (noteChange == 44)
            main.startColor = BColor;

        else if (noteChange == 42)
            main.startColor = C1Color;

        else if (noteChange == -1) {
            isActiveButton = false;
        }
    }

    void NewGameObjectCube()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(0, 1, 0);

        MeshRenderer gameObjectRenderer = go.AddComponent<MeshRenderer>();

        Material newMaterial = new Material(Shader.Find("Diffuse"));

        Color objectColor = CColor;
        newMaterial.color = objectColor;
        gameObjectRenderer.material = newMaterial;
        return;
    }

    void NewGameObjectSphere()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = new Vector3(0, 1, 0);

        MeshRenderer gameObjectRenderer = go.AddComponent<MeshRenderer>();

        Material newMaterial = new Material(Shader.Find("Diffuse"));

        Color objectColor = CColor;
        newMaterial.color = objectColor;
        gameObjectRenderer.material = newMaterial;
    }
}