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
        if (component == "rhythm")
            pressure = sensorValue;
        else if (component == "melody")
            note = sensorValue;
        else if (component == "modulate")
        {
            pitch = sensorValue;
            sustain = int.Parse(secondaryValue);
            JoyStickButton = bool.Parse(lastValue);
        }
            
            

    }   
}