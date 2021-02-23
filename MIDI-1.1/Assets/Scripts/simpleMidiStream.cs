//#define MPTK_PRO
//#define DEBUG_MULTI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;

namespace MidiPlayerTK
{
    //this script is based on the TestMidiStream.cs that comes with the Midi Tool Kit plugin
    public class SimpleMidiStream : MonoBehaviour
    {
        // MPTK component able to play a stream of midi events
        public MidiStreamPlayer midiStreamPlayer;


        //unimportand midi setup variables
        const float DEFAULT_PITCH = 64;

        private int[] indexGenerator;
        private string[] labelGenerator;
        private float[] valueGenerator;
        private const int nbrGenerator = 4;
        

        //the instrument sound to simulate
        public int instrumentSound = 0; //used in this Prototype: 0 = Grand Stereo Piano; 24 = Nylon Guitar; 73 = Flute;

        //the channel the midi data is streamed to
        [Range(0, 16)]
        public int streamChannel = 0;

        //Volume of the sound
        [Range(0, 127)] 
        public int velocity = 100;

        //the note currently played
        public int currentNote = -1; 
        private MPTKEvent notePlaying;
        public bool isPlaying = false; //if note is playing

        //0 - 127
        public float pitchChange = DEFAULT_PITCH;
        //0-1000 (roughly)
        public float sensorPitch;
                           
        //
        public bool newNote = true;

        //main visualizer
        public GameObject mainInstrumentVisualizer;

        private void Start()                                  
        {
            if (midiStreamPlayer != null) //start up midi stream player
            {
                if (!midiStreamPlayer.OnEventSynthAwake.HasEvent())
                    midiStreamPlayer.OnEventSynthAwake.AddListener(StartLoadingSynth);
            }
            else
                Debug.LogWarning("midiStreamPlayer is not defined. Check in Unity editor inspector of this gameComponent");
        }

        public void StartLoadingSynth(string name)
        {
            Debug.LogFormat($"Start loading Synth {name}");
        }

    
        public void EndLoadingSynth(string name)
        {
            Debug.LogFormat($"Synth {name} is loaded");

            // Set piano (preset 0) to channel 0. Could be different for another SoundFont.
            midiStreamPlayer.MPTK_ChannelPresetChange(0, 0);
            Debug.LogFormat($"Preset {midiStreamPlayer.MPTK_ChannelPresetGetName(0)} defined on channel 0");

            // Set reed organ (preset 20) to channel 1. Could be different for another SoundFont.
            midiStreamPlayer.MPTK_ChannelPresetChange(1, 20);
            Debug.LogFormat($"Preset {midiStreamPlayer.MPTK_ChannelPresetGetName(1)} defined on channel 1");
        }



        // Update is called once per frame
        void Update()
        {
            // Check that SoundFont is loaded and add a little wait (0.5 s by default) because Unity AudioSource need some time to be started
            if (!MidiPlayerGlobal.MPTK_IsReady())
                return;

            //create notes here!!!
            if (midiStreamPlayer != null)
            {

                //update midi variables if necessary
                midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent()
                {
                    Command = MPTKCommand.PatchChange,
                    Value = instrumentSound,
                    Channel = streamChannel,
                });


                //velocity handeling
                if (velocity <= 40 && velocity>0)
                    velocity = 40;

                if (velocity >= 127)
                    velocity = 127;

                if (velocity == 0)
                {
                    StopOneNote(); //note stops when velocity stops
                    newNote = true;
                }
                   
                //pitch handeling
                if (sensorPitch == 500)
                    pitchChange = DEFAULT_PITCH;
                else
                    pitchChange = sensorPitch / 7.8f; //translate 0-1000 roughly to 0-127            
                               
            }

            //only play if all components are active
            if (velocity != 0 && currentNote != -1 && pitchChange != 64)
            {
                isPlaying = true;
                if (newNote)
                {
                    Play(false);
                    //visualizes in main view
                    mainInstrumentVisualizer.GetComponent<MainVisualizer>().NewNote(velocity, currentNote, pitchChange);
                    mainInstrumentVisualizer.GetComponent<MainVisualizer>().isStopped = false;
                    newNote = false;
                }
            }
            else
            {
                isPlaying = false;
                mainInstrumentVisualizer.GetComponent<MainVisualizer>().isStopped = true;
            }

        }
        
        //is called this way so the computer knows which note to stop
        void Play(bool stopCurrent)
        {        
            if (stopCurrent)
            {
                StopOneNote();
            }
            PlayOneNote();

        }

        //has to be called to compose the note
        private void PlayOneNote()
        {
            //Debug.Log($"{StreamChannel} {midiStreamPlayer.MPTK_ChannelPresetGetName(StreamChannel)}");
            // Start playing a new note
            notePlaying = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,          //gets triggerd when all components are active
                Value = currentNote,                   //tone frequency
                Channel = streamChannel,               //stays the same
                Duration = -1,                         //plays until stoped or fades away
                Velocity = velocity,                   //sound volume
                                     
            };
            midiStreamPlayer.MPTK_PlayEvent(notePlaying);
           
        }
      
      
        //has to be called to stop the note after playing
        private void StopOneNote()                             
        {
            if (notePlaying != null)
            {
                //Debug.Log("Stop note");
                midiStreamPlayer.MPTK_StopEvent(notePlaying);
                notePlaying = null;
               
            }
        }
    }
}