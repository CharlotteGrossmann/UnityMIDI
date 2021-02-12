//#define MPTK_PRO
//#define DEBUG_MULTI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;

namespace MidiPlayerTK
{
    //this is the SIMPLE MidiStream, because it it's based on the MidiStream.cs that comes with the Midi Tool Kit Free Plugin
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
        public int instrumentSound = 0;

        [Range(0, 16)]
        public int streamChannel = 0;

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
        public float sensorPitch;
                                                   
        //get banyan info
        public GameObject instrument;
        public int vibrato;

        private int pastVelocity = 0;
        public bool newNote = true;



        //
        public GameObject mainView;
        public string instrumentName;
        public int midiID = 0;

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


                //update the note according to banyan message
                //currentNote = instrument.GetComponent<MessageProcessor>().stringNote;
              

                //manipulate velocity
                //velocity = instrument.GetComponent<MessageProcessor>().stringVolume;

                if (velocity <= 40 && velocity>0)
                    velocity = 40;

                if (velocity >= 127)
                    velocity = 127;

                if (velocity == 0)
                {
                    StopOneNote(midiID); //not sure if this is stopping the rights notes
                    newNote = true;
                }
                   
                

                //manipulate pitch
                //var sensorPitch = instrument.GetComponent<MessageProcessor>().stringPitch;
                if (sensorPitch == 500)
                    pitchChange = DEFAULT_PITCH;
                else
                    pitchChange = sensorPitch / 7.8f;
                
                vibrato = instrument.GetComponent<MessageProcessor>().stringVibrato;
                
                               
            }

            //only play if all components are active
            if (velocity != 0 && currentNote != -1 && (pitchChange != 64))
            {
                isActive = true;
                if (newNote)
                {
                    midiID += 1;
                    Play(false);
                    //visualizes in main view
                    mainView.GetComponent<MainVisualizer>().NewNote(velocity, currentNote, pitchChange);
                    newNote = false;
                    mainView.GetComponent<MainVisualizer>().isStopped = false;
                }
            }
            else
            {
                isActive = false;
                mainView.GetComponent<MainVisualizer>().isStopped = true;
            }


            pastVelocity = velocity;

        }
        
        //is called this way so the computer knows which note to stop
        void Play(bool stopCurrent)
        {        
            if (stopCurrent)
            {
                StopOneNote(midiID);
            }
            PlayOneNote(midiID);

        }

        //has to be called to compose the note
        private void PlayOneNote(int midiID)
        {

            //tell lifetime that note is playing

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
        private void StopOneNote(int midiID)                             
        {
            if (notePlaying != null)
            {
                //tell lifetime that the midi note is stoped

                //Debug.Log("Stop note");
                // Stop the note (method to simulate a real human on a keyboard : 
                // duration is not known when note is triggered)
                midiStreamPlayer.MPTK_StopEvent(notePlaying);
                notePlaying = null;
               
            }
        }
    }
}