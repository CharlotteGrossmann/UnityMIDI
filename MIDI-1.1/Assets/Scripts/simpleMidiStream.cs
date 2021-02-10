//#define MPTK_PRO
//#define DEBUG_MULTI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;

namespace MidiPlayerTK
{
    //this is the SIMPLE MidiStream, because it it's based on the MidiStreamPlayer.cs that comes with the Midi Tool Kit Free Plugin
    public class SimpleMidiStream : MonoBehaviour
    {
        // MPTK component able to play a stream of midi events
        public MidiStreamPlayer midiStreamPlayer;


        //unimportand midi setup variables
        [Range(0, 16)]
        int streamChannel = 0;

        const float DEFAULT_PITCH = 64;

        private int[] indexGenerator;
        private string[] labelGenerator;
        private float[] valueGenerator;
        private const int nbrGenerator = 4;
        

        //the instrument sound to simulate
        private int instrumentSound = 0; 

        //Volume of the sound
        [Range(0, 127)] 
        public int velocity = 100;

        //the note currently played
        public int currentNote = -1;
        private MPTKEvent notePlaying;
        public bool isActive = false;

        //0 - 127
        public float pitchChange = DEFAULT_PITCH;
        private float currentVelocityPitch;
        private float lastTimePitchChange = 0;
                                                   
        //get banyan info
        public GameObject instrument;
        public int mySustain;

        private int pastVelocity = 0;
        public bool newNote = true;
        

        private void Awake()                                     
        {
            if (midiStreamPlayer != null)
            {
                if (!midiStreamPlayer.OnEventSynthAwake.HasEvent())
                    midiStreamPlayer.OnEventSynthAwake.AddListener(StartLoadingSynth);
            }
            else
                Debug.LogWarning("midiStreamPlayer is not defined. Check in Unity editor inspector of this gameComponent");
        }

        // Use this for initialization
        void Start()
        {

            GenModifier.InitListGenerator();
            indexGenerator = new int[nbrGenerator];
            labelGenerator = new string[nbrGenerator];
            valueGenerator = new float[nbrGenerator];

            for (int i = 0; i < nbrGenerator; i++)
            {
                indexGenerator[i] = GenModifier.RealTimeGenerator[0].Index;
                labelGenerator[i] = GenModifier.RealTimeGenerator[0].Label;
                if (indexGenerator[i] >= 0)
                    valueGenerator[i] = GenModifier.DefaultNormalizedVal((fluid_gen_type)indexGenerator[i]) * 100f;
            }

            pitchChange = DEFAULT_PITCH;
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

            if (pitchChange != DEFAULT_PITCH)
            {
                // If user change the pitch, wait 1/2 second before return to median value
                if (Time.realtimeSinceStartup - lastTimePitchChange > 0.5f)
                {
                    pitchChange = Mathf.SmoothDamp(pitchChange, DEFAULT_PITCH, ref currentVelocityPitch, 0.5f, 100, Time.unscaledDeltaTime);
                    if (Mathf.Abs(pitchChange - DEFAULT_PITCH) < 0.1f)
                        pitchChange = DEFAULT_PITCH;
                    //PitchChange = Mathf.Lerp(PitchChange, DEFAULT_PITCH, Time.deltaTime*10f);
                    //Debug.Log("DEFAULT_PITCH " + DEFAULT_PITCH + " " + PitchChange + " " + currentVelocityPitch);
                      midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent() { Command = MPTKCommand.PitchWheelChange, Value = (int)pitchChange << 7, Channel = streamChannel });
                }
            }            
            


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

                //only play if all components are active
                if (velocity !=0  && currentNote != -1 && (pitchChange !=64 /*|| mySustain != 0*/))
                {
                    isActive = true;
                    if (newNote)//if (pastVelocity != Velocity)
                    {
                        Play(false);
                        newNote = false;
                    }
                }
                else
                    isActive = false;

                currentNote = instrument.GetComponent<MessageProcessor>().note;
              

                //manipulate velocity
                velocity = instrument.GetComponent<MessageProcessor>().pressure;
                if (velocity <= 40 && velocity>0)
                    velocity = 40;
                if (velocity >= 127)
                    velocity = 127;
                if (velocity == 0)
                    newNote = true;
                   
                

                //manipulate pitch
                var sensorPitch = instrument.GetComponent<MessageProcessor>().pitch;
                if (sensorPitch == 500)
                    pitchChange = DEFAULT_PITCH;
                else
                {
                    pitchChange = sensorPitch / 10;
                }
                mySustain = instrument.GetComponent<MessageProcessor>().sustain;
                /*if (sustain == 500)
                    //print("defualt sustain");
                else*/
                      
              
            }

            pastVelocity = velocity;

        }
        
        //has to be called actually play the note
        void Play(bool stopCurrent)
        {        
            if (stopCurrent)
                StopOneNote();
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
                // Stop the note (method to simulate a real human on a keyboard : 
                // duration is not known when note is triggered)
                midiStreamPlayer.MPTK_StopEvent(notePlaying);
                notePlaying = null;
               
            }
        }
    }
}