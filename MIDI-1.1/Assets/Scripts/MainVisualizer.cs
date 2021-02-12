using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVisualizer : MonoBehaviour
{
    //assign in Unity
    public string instrument;
    public GameObject midiStream;

    //note spawning variables
    public Transform visualNote; //note prefab
    public Transform self; //parent object

    private Transform clone;    //cloned object
    private Vector3 spawnPosition;
    

    //when note isn't playing anymore
    public bool isStopped;

    //color variables
    public float H;
    public float S;
    public float V;
    public float oppacity = 1;


    void Start()
    {
        //get color of prefab and convert to HSV
        var prefabSprite = visualNote.GetComponent<SpriteRenderer>();
        var thisColor = prefabSprite.color;
        Color.RGBToHSV(thisColor, out H, out S, out V);

        //get position of prefab (because it's in the middle of the circle)
        spawnPosition = visualNote.position;        //roughly (591, 0, 527);

        //disable sprite of the prefab so it's not visible in the game anyomer
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
        clone = Instantiate(visualNote, spawnPosition, Quaternion.identity); //create new note

        //change oppacity according to volume
        oppacity = velocity / 127;//velocity is 0-127, oppacity smaller than 128 is not visible, so min of opaccity is 128 max is 255
        var sprite = clone.GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        sprite.color = new Color(sprite.color.a, sprite.color.g, sprite.color.b, oppacity);

        var difference = 5;

        //change difference in brightness according to pitch and note
        //the higher the note, the brighter the color
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
        
        //aplly difference in brightness to the sprite color
        sprite.color = Color.HSVToRGB(H, S - difference -((64 - pitch) / 100) * 40f , V);

        clone.transform.SetParent(self.transform); //set note as child
        


    }
}