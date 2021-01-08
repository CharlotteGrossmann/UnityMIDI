using UnityEngine;
using System.Collections;

public class MidiMessageDistributor : MonoBehaviour
{
    public GameObject[] targets;
    MidiReceiver receiver;

    void Start ()
    {
        receiver = FindObjectOfType (typeof(MidiReceiver)) as MidiReceiver;
    }

    void Update ()
    {
        while (!receiver.IsEmpty) {
            var message = receiver.PopMessage ();
            if (message.status == 0x90) { //message.status überprüft den ersten Byte der MIDI Nachricht. 0x90 heíßt Standartmäßig Note ON
                foreach (var go in targets) {
                    go.SendMessage ("OnNoteOn", message);
                }
            } else if (message.status == 0x80) { //0x80 ist im MIDI Standard der Code für Note OFF
                foreach (var go in targets) {
                    go.SendMessage ("OnNoteOff", message);
                }
            }
        }
    }
}
