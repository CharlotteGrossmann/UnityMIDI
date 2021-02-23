using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;
public class InstrumentManager : MonoBehaviour
{
    //define in unity
    public GameObject keyboards;
    public GameObject woodwinds;
    public GameObject strings;
    public GameObject cube;


    //give the right input to the right midi stream
    void Update()
    {
        var distributor = cube.GetComponent<MessageProcessor>();
        var keyboardVar = keyboards.GetComponent<SimpleMidiStream>();
        var woodwindVar = woodwinds.GetComponent<SimpleMidiStream>();
        var stringVar = strings.GetComponent<SimpleMidiStream>();

        keyboardVar.velocity = distributor.keyboardVolume;
        keyboardVar.currentNote = distributor.keyboardNote;
        keyboardVar.sensorPitch = 501;

        woodwindVar.currentNote = distributor.woodwindNote;
        woodwindVar.velocity = distributor.woodwindVolume;
        woodwindVar.sensorPitch = 501;

        stringVar.currentNote = distributor.stringNote;
        stringVar.sensorPitch = distributor.stringPitch;
        stringVar.velocity = distributor.stringVolume;
    }
}
