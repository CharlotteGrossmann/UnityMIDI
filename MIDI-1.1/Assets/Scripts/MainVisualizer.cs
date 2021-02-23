using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVisualizer : MonoBehaviour
{

    //assign in Unity
    public Transform visualNote; //note prefab
    public Transform self; //parent object

    //note spawning variables
    private Transform clone;    //cloned object
    private Vector3 spawnPosition;
    

    //if note stopped playing
    public bool isStopped;

    //color variables
    private float H;
    private float S;
    private float V;
    private float oppacity = 1;


    void Start()
    {
        //get color of prefab and convert to HSV
        var prefabSprite = visualNote.GetComponent<SpriteRenderer>();
        var thisColor = prefabSprite.color;
        Color.RGBToHSV(thisColor, out H, out S, out V);

        //get position of prefab (because it's in the middle of the circle)
        spawnPosition = visualNote.position;        

        //disable sprite of the prefab so it's not visible in the game
        prefabSprite.enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        //Delete the visible note when it's stopped playing
        if (isStopped && this.transform.childCount != 0)
            Destroy(this.transform.GetChild(0).gameObject);
        
        //randomise the spawn position
        spawnPosition.x = visualNote.position.x + Random.Range(-2f, 2.5f);
        spawnPosition.y = visualNote.position.y + Random.Range(-1.2f, 1.2f);

    }

  

    public void NewNote(float velocity, int note, float pitch)
    {       
        clone = Instantiate(visualNote, spawnPosition, Quaternion.identity); //copy the note prefab and spawn it at the spawnPosition

        //change oppacity according to volume
        oppacity = velocity / 127;//velocity is 0-127, oppacity is 0-1
        var sprite = clone.GetComponent<SpriteRenderer>();
        sprite.enabled = true; //we have to enable the sprite because in the prefab it's disabled
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, oppacity); //turn the sprite to customised color


        //change difference in brightness according to pitch and note
        //the higher the note, the brighter the color
        var difference = 5;
        switch (note)
        {   
            case 42:
                difference *= -4;
                break;
            case 44:
                difference *= 3;
                break;
            case 46:
                difference *= -2;
                break;
            case 47:
                difference *= -1;
                break;
            case 49:
                difference *= 0;
                break;
            case 51:
                difference *= 1;
                break;
            case 53:
                difference *= 2;
                break;
            case 54:
                difference *= 3;
                break;
            default:
                break;
        }
        
        //apply difference in brightness to the sprite color
        sprite.color = Color.HSVToRGB(H, S - difference -((64 - pitch) / 100) * 40f , V);

        clone.transform.SetParent(self.transform); //set note as child
        


    }
}