using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVisualizer : MonoBehaviour
{
    //assign in Unity
    public string instrument;




    public bool isStopped;

    public Transform visualNote; //note prefab
    private Transform clone;    //cloned object
    private Color noteColor;
    public Vector3 spawnPosition;
    public GameObject midiStream;

    public Transform self; //parent object

    public int r;
    public int g;
    public int b;
    private float oppacity = 1;

    //rgba
    public Color instrumentColor;

    // Start is called before the first frame update
    void Start()
    {
        noteColor = visualNote.GetComponent<SpriteRenderer>().color;
        spawnPosition = visualNote.position;

    }

    // Update is called once per frame
    void Update()
    {
        instrumentColor = new Color (r,g,b, oppacity);
        if (isStopped)
            Destroy(clone.gameObject);
        spawnPosition.y = visualNote.position.y + Random.Range(-3, 3f);
        spawnPosition.z = visualNote.position.z + Random.Range(-6, 6f);



    }

  

    public void NewNote(float velocity, string instrument, int note, float pitch)
    {
        //define look
        //color = instrument
        

        //brightness = note and pitch


        //oppacity = volume
        oppacity = velocity/127;//velocity is 0-127, oppacity smaller than 128 is not visible, so min of opaccity is 128 max is 255

        clone = Instantiate(visualNote, spawnPosition, Quaternion.identity); //create new note
        clone.transform.SetParent(self.transform);
        //adds Lifetime script so it floats upwards
        noteColor = instrumentColor;

        //clone.gameObject.AddComponent<Lifetime>();
        //clone.GetComponent<Lifetime>().midiStream = midiStream;


    }
}