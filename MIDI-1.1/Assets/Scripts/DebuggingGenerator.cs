using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*public static class ExtensionMethods
{

    public static Int Remap(note, 21, 108, 0, 359)
    {
       
    }

}*/
public class DebuggingGenerator : MonoBehaviour
{
    public GameObject gem;
    GameObject element;
    List<NoteInfo> noteInfoList;

    public int note = 47;
    public int velocity = 122;

    void Awake()
    {
        noteInfoList = new List<NoteInfo>();
        

    }

    

    void Update(){
        if (Input.GetKeyDown(KeyCode.Return))
        {
         

            foreach (var ni in noteInfoList)
            {
                if (ni.note == note)
                    return;
            }

            // int numElements = Mathf.Max(8 * velocity / 128, 1);
            int numElements = 1;
            var noteInfo = new NoteInfo(note);
            for (var i = 0; i < numElements; i++)
            {
                int randomisedRotation = Random.Range(0, 360);
                /*int noteRotation = MapValue(21, 108, 0, 359, note){                                                                    {
                    return (0 + (359 - 0) * ((note - 21) / (108 - 21)));
                }*/
                Vector3 spawnPoint = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-5.0f, 5.0f), 0);
                element = Instantiate(gem, spawnPoint, Quaternion.Euler(note, -90, randomisedRotation)) as GameObject;
                noteInfo.elements.Add(element);
               
              
                //element.GetComponent<Element>().ApplyMouse();
            }

            noteInfoList.Add(noteInfo);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {

            
            NoteInfo niFound = null;
            foreach (var ni in noteInfoList)
            {
                if (ni.note == note)
                {
                    niFound = ni;
                    break;
                }
            }
            
            if (niFound != null)
            {
                foreach (var e in niFound.elements)
                {
                    
                }
                noteInfoList.Remove(niFound);
            }
        }
    }
    void destroy()
    {
        Destroy(element);
    }

}
