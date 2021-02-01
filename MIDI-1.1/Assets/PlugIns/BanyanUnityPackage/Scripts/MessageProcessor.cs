using UnityEngine;

//summary
// This script does an action based on the message forwarded by BanyanMessageListener (sent by Banyan).
// This script should be included on any object that will be doing an action, based on a message coming from Banyan
//summary

public class MessageProcessor : MonoBehaviour
{
    private int sensorValue;
    private string component;
    private string secondaryValue;
    private string lastValue;
    public int pressure;
    public int note;
    public int flutePressure;
    public int fluteNote;
    public int pitch = 64;
    public int sustain;
    public bool JoyStickButton;

    public void DoAction(string action, string info, int value, string target)
    {
        sensorValue = value;
        component = action;
        secondaryValue = info;
        lastValue = target;

    }
    void Update()
    {
        switch (component)
        {
            case "KeyboardRhythm":
                pressure = sensorValue;
                break;
            case "KeyboardMelody":
                note = sensorValue;
                break;
            case "FluteRhythm":
                flutePressure = sensorValue;
                break;
            case "FluteMelody":
                fluteNote = sensorValue;
                break;
            case "modulate":
                pitch = sensorValue;
                sustain = int.Parse(secondaryValue);
                // JoyStickButton = bool.Parse(lastValue);
                break;
            case "sustain":
                break;
            default:
                pitch = 500; //default sensor value for joystick translates to defualt pitch of 64 in simpleMidiStream.cs
                break;


        }
       
    }   
}