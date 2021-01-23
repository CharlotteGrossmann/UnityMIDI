using UnityEngine;

//summary
// This script does an action based on the message forwarded by BanyanMessageListener (sent by Banyan).
// This script should be included on any object that will be doing an action, based on a message coming from Banyan
//summary

public class MessageProcessor : MonoBehaviour
{
    public int note;
    public void DoAction(string action, string info, int value, string target)
    {
        note = value;
    }    
    
}