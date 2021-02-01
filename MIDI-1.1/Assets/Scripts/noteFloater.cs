using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when a new note spawns this script gets attached and makes the note float up until its out of the cameras view
public class noteFloater : MonoBehaviour
{

    private float i = 0;
    void Update()
    {
        i-=1f*Time.deltaTime; //times time.deltaTime to apply per second not frame
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - i, this.transform.position.z); //let sphere float upwards
        if (this.transform.position.y > 513)
            Destroy(this.gameObject); //destroy object when it's not in the camera view anymore
    }
}
