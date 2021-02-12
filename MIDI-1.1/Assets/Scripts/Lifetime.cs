using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

//when a new note spawns in the main view this script gets attached 
//it animate the note and 
public class Lifetime : MonoBehaviour
{
    public GameObject midiStream;
    private bool isActive;
    // Start is called before the first frame update
    void Awake()
    {
        isActive = midiStream.GetComponent<SimpleMidiStream>().isActive;
    }

    // Update is called once per frame
    void Update()
    {
        /*for(var i = -30; i<30&&i>-30; i++)
        {

            this.transform.rotation += new Quaternion(-i, -i, -0,-0);
        }*/
        if (!isActive)
            Destroy(this.gameObject);
        
    }
}
