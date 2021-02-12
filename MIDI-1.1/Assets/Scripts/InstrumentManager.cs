using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;
public class InstrumentManager : MonoBehaviour
{

    public GameObject keyboards;
    public GameObject woodwinds;
    public GameObject strings;
    public GameObject messageProcessor;


    //give the right input to the right midi stream
    void Update()
    {
        keyboards.GetComponent<SimpleMidiStream>().velocity = messageProcessor.GetComponent<MessageProcessor>().keyboardVolume;
        keyboards.GetComponent<SimpleMidiStream>().currentNote = messageProcessor.GetComponent<MessageProcessor>().keyboardNote;
        keyboards.GetComponent<SimpleMidiStream>().sensorPitch = 501;

        woodwinds.GetComponent<SimpleMidiStream>().currentNote = messageProcessor.GetComponent<MessageProcessor>().woodwindNote;
        woodwinds.GetComponent<SimpleMidiStream>().velocity = messageProcessor.GetComponent<MessageProcessor>().woodwindVolume;
        woodwinds.GetComponent<SimpleMidiStream>().sensorPitch = 501;

        strings.GetComponent<SimpleMidiStream>().currentNote = messageProcessor.GetComponent<MessageProcessor>().stringNote;
        strings.GetComponent<SimpleMidiStream>().sensorPitch = messageProcessor.GetComponent<MessageProcessor>().stringPitch;
        strings.GetComponent<SimpleMidiStream>().vibrato = messageProcessor.GetComponent<MessageProcessor>().stringVibrato;
        strings.GetComponent<SimpleMidiStream>().velocity = messageProcessor.GetComponent<MessageProcessor>().stringVolume;
    }
}
