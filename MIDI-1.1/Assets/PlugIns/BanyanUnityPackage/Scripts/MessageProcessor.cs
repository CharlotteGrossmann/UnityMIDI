using UnityEngine;


public class MessageProcessor : MonoBehaviour
{

    //Keyboards
    public int keyboardNote;
    public int keyboardVolume;

    //Woodwinds
    public int woodwindVolume;
    public int woodwindNote;

    //Strings
    public int stringVolume;
    public int stringNote;
    public int stringPitch = 500;
    public int stringVibrato;


    //components send: (component name, second modulation value, component value, cube)
    public void DoAction(string action, string info, int value, string target)
    {
        switch (action)
        {
            case "KeyboardRhythm":
                keyboardVolume = value;
                break;
            case "KeyboardMelody":
                keyboardNote = value;
                break;

            case "WoodwindRhythm":
                woodwindVolume = value;
                break;
            case "WoodwindMelody":
                woodwindNote = value;
                break;

            case "StringRhythm":
                stringVolume = value;
                break;
            case "StringMelody":
                stringNote = value;
                break;
            
            case "StringPitch":
                stringPitch = value;
                break;
            case "StringVibrato":
                stringVibrato = int.Parse(info);
                break;
            default:
                break;


        }
    }
    void Update()
    {
        
       
    }   
}