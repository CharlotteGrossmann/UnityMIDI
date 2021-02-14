using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when a new note spawns this script gets attached and makes the note float up until its out of the cameras view
public class NoteFloater : MonoBehaviour
{
    private float startPosition;
    void Awake()
    {
        startPosition = this.transform.position.y;
    }
    private float upMovement = 0; 
    void Update()
    {
        upMovement-=0.6f*Time.deltaTime; //times time.deltaTime to apply per second not frame
        var newPosition = new Vector3(this.transform.position.x, this.transform.position.y - upMovement, this.transform.position.z);
        this.transform.position = newPosition; //let sphere float upwards
        if (this.transform.position.y > startPosition+3f)
            Destroy(this.gameObject); //destroy object when it's not in the camera view anymore
    }
}
