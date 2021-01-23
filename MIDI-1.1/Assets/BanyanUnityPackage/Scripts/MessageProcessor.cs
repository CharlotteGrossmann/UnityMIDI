using UnityEngine;

//summary
// This script does an action based on the message forwarded by BanyanMessageListener (sent by Banyan).
// This script should be included on any object that will be doing an action, based on a message coming from Banyan
//summary

public class MessageProcessor : MonoBehaviour
{
    public int sensorValue;
    public string component;
    public int pressure;
    public int note;
    public void DoAction(string action, string info, int value, string target)
    {
        sensorValue = value;
        component = action;
    }
    void Update()
    {
        if (component == "rhythm")
            pressure = sensorValue;
        else if (component == "melody")
            note = sensorValue;
    }   
}