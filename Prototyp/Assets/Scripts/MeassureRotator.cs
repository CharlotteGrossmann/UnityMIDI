using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeassureRotator : MonoBehaviour
{
    //define in Unity
    public float bpm; //beats per minute


    //for meassure rotation
    private float meassureRotation; //rotation of one meassure
    private float mps; //Meassure per second



    void Start()
    {
        mps = (bpm / 60) / 4; //calculate meassures per second
        meassureRotation = 360 / 24; //360° of the circle divided by total meassures = the how much toation is needed for 1 meassure

    }

    void Update()
    {
        transform.Rotate(0, 0, meassureRotation * mps * Time.deltaTime); //rotate messure in time with music
        
    }




}


