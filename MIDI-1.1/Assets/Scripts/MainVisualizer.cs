using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVisualizer : MonoBehaviour
{

    public Transform visualNote; //note prefab
    private Transform clone;    //cloned object
    private Color startColor;
    public Vector3 spawnPosition;
    public GameObject midiStream;

    public Transform mainView; //parent object

    private ParticleSystem ps;
    

    private float oppacity = 1;

    //rgba
    public Color woodwindColor;
    public Color stringsColor;
    public Color keyboardColor;


    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        startColor = visualNote.GetComponent<SpriteRenderer>().color;
        spawnPosition = visualNote.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        woodwindColor = new Color (191, 245, 186, oppacity);
        stringsColor = new Color(8, 255, 82, oppacity);
        keyboardColor = new Color(188, 219, 238, oppacity);

        spawnPosition.x += Random.Range(100, 101);

        
    }

   

    public void NewNote(float velocity, string instrument)
    { 
        //define look
        switch (instrument)   //color = instrument
        {
            case "Woodwind":
                startColor = woodwindColor;
                break;
            case "String":
                startColor = stringsColor;
                break;
            case "Keyboard":
                startColor = keyboardColor;
                break;
            default:
                break;
        }
        //brightness = frequenc

        //oppacity = volume
        oppacity = velocity/127;//velocity is 0-127, oppacity smaller than 128 is not visible, so min of opaccity is 128 max is 255

        clone = Instantiate(visualNote, spawnPosition, Quaternion.identity); //create new note
        clone.transform.SetParent(mainView.transform);
        //adds Lifetime script so it floats upwards
        
        clone.gameObject.AddComponent<Lifetime>();
        //clone.GetComponent<Lifetime>().midiStream = midiStream;

    }
}