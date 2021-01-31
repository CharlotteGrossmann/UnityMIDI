//#define MPTK_PRO
//#define DEBUG_MULTI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;

namespace MidiPlayerTK
{
    public class simpleMidiStream : MonoBehaviour
    {

        // MPTK component able to play a stream of midi events
        // Add a MidiStreamPlayer Prefab to your game object and defined midiStreamPlayer in the inspector with this prefab.
        public MidiStreamPlayer midiStreamPlayer;

        [Range(-10f, 100f)]
        public float NoteDuration = 0.7f;



        [Range(0, 127)] //lautstärke des Tons
        public int Velocity = 100;

        [Range(0, 16)]
        int StreamChannel = 0;

        [Range(0, 127)]
        public int CurrentNote;

        [Range(0, 127)]
        public int InstrumentSound; //sollte 0 sein für Klavier
   
        [Range(0, 127)]
        public int PanChange;
      
        const float DEFAULT_PITCH = 64;

        [Range(0, 127)] 
        public float PitchChange = DEFAULT_PITCH;
        private float currentVelocityPitch;
        private float LastTimePitchChange = 0;

   

        /// <summary>
        /// Current note playing
        /// </summary>
        private MPTKEvent NotePlaying;
        public bool isPlaying = false;

        private float LastTimeChange;
                                                    

        // Popup to select a realtime generator
        private PopupListItem[] PopGenerator;
        private int[] indexGenerator;
        private string[] labelGenerator;
        private float[] valueGenerator;
        private const int nbrGenerator = 4;

        //get banyan  info
        public GameObject Instrument;
        public int mySustain;

        //keyboard mode
        public bool banyan_melody = true;
        public bool banyan_rhythm = true;
        public bool banyan_modulate = true;

        private int pastVelocity = 0;
        private int keyNote = -1;

        

        private void Awake()                                     
        {
            if (midiStreamPlayer != null)
            {
                // The call of this method can also be defined in the prefab MidiStreamPlayer
                // from the Unity editor inspector. See "On Event Synth Awake". 
                // StartLoadingSynth will be called just before the initialization of the synthesizer.
                // Warning: depending on the starting order of the GameObjects, 
                //          this call may be missed if MidiStreamPlayer is started before TestMidiStream, 
                //          so it is preferable to define this event in the inspector.
                if (!midiStreamPlayer.OnEventSynthAwake.HasEvent())
                    midiStreamPlayer.OnEventSynthAwake.AddListener(StartLoadingSynth);

                // The call of this method can also be defined in the prefab MidiStreamPlayer 
                // from the Unity editor inspector. See "On Event Synth Started.
                // EndLoadingSynth will be called when the synthesizer is ready.
                if (!midiStreamPlayer.OnEventSynthStarted.HasEvent())
                    midiStreamPlayer.OnEventSynthStarted.AddListener(EndLoadingSynth);
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
            PopGenerator = new PopupListItem[nbrGenerator];
            for (int i = 0; i < nbrGenerator; i++)
            {
                indexGenerator[i] = GenModifier.RealTimeGenerator[0].Index;
                labelGenerator[i] = GenModifier.RealTimeGenerator[0].Label;
                if (indexGenerator[i] >= 0)
                    valueGenerator[i] = GenModifier.DefaultNormalizedVal((fluid_gen_type)indexGenerator[i]) * 100f;
                //PopGenerator[i] = new PopupListItem() { Title = "Select A Generator", OnSelect = GeneratorChanged, Tag = i, ColCount = 3, ColWidth = 250, };
            }
            LastTimeChange = Time.realtimeSinceStartup;
            //CurrentNote = StartNote;
            PanChange = 64;
            LastTimeChange = -9999999f;
            PitchChange = DEFAULT_PITCH;
        }

        /// <summary>
        /// The call of this method is defined in MidiPlayerGlobal (it's a son of the prefab MidiStreamPlayer) from the Unity editor inspector. 
        /// The method is called when SoundFont is loaded. We use it only to statistics purpose.
        /// </summary>
        public void EndLoadingSF()
        {
            Debug.Log("End loading SoundFont. Statistics: ");

            Debug.Log("List of presets available");
            foreach (MPTKListItem preset in MidiPlayerGlobal.MPTK_ListPreset)
                Debug.Log($"   [{preset.Index,3:000}] - {preset.Label}");

            Debug.Log("   Time To Load SoundFont: " + Math.Round(MidiPlayerGlobal.MPTK_TimeToLoadSoundFont.TotalSeconds, 3).ToString() + " second");
            Debug.Log("   Time To Load Samples: " + Math.Round(MidiPlayerGlobal.MPTK_TimeToLoadWave.TotalSeconds, 3).ToString() + " second");
            Debug.Log("   Presets Loaded: " + MidiPlayerGlobal.MPTK_CountPresetLoaded);
            Debug.Log("   Samples Loaded: " + MidiPlayerGlobal.MPTK_CountWaveLoaded);

        }

        public void StartLoadingSynth(string name)
        {
            Debug.LogFormat($"Start loading Synth {name}");
        }

        /// <summary>
        /// This methods is run when the synthesizer is ready if you defined OnEventSynthStarted or set event from Inspector in Unity.
        /// It's a good place to set some synth parameter's as defined preset by channel 
        /// </summary>
        /// <param name="name"></param>
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

        //public bool testLocalchange = false;


        // Update is called once per frame
        void Update()
        {
           


            // Check that SoundFont is loaded and add a little wait (0.5 s by default) because Unity AudioSource need some time to be started
            if (!MidiPlayerGlobal.MPTK_IsReady())
                return;

            if (PitchChange != DEFAULT_PITCH)
            {
                // If user change the pitch, wait 1/2 second before return to median value
                if (Time.realtimeSinceStartup - LastTimePitchChange > 0.5f)
                {
                    PitchChange = Mathf.SmoothDamp(PitchChange, DEFAULT_PITCH, ref currentVelocityPitch, 0.5f, 100, Time.unscaledDeltaTime);
                    if (Mathf.Abs(PitchChange - DEFAULT_PITCH) < 0.1f)
                        PitchChange = DEFAULT_PITCH;
                    //PitchChange = Mathf.Lerp(PitchChange, DEFAULT_PITCH, Time.deltaTime*10f);
                    //Debug.Log("DEFAULT_PITCH " + DEFAULT_PITCH + " " + PitchChange + " " + currentVelocityPitch);
                      midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent() { Command = MPTKCommand.PitchWheelChange, Value = (int)PitchChange << 7, Channel = StreamChannel });
                }
            }            
            


            //Noten erzeugung!
            if (midiStreamPlayer != null)
            {
                //update midi variables if necessary
                midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent()
                {
                    Command = MPTKCommand.PatchChange,
                    Value = InstrumentSound,
                    Channel = StreamChannel,
                });


                //melody simulation
                if (banyan_melody)
                    CurrentNote = Instrument.GetComponent<MessageProcessor>().note;
                else if (!banyan_melody)
                    CurrentNote = getKeys();

                //rhythm simulation
                if (banyan_rhythm)
                {
                    Velocity = Instrument.GetComponent<MessageProcessor>().pressure;
                    mySustain = Instrument.GetComponent<MessageProcessor>().sustain;
                    if (Velocity <= 40 && Velocity>0)
                        Velocity = 40;
                    if (Velocity >= 127)
                        Velocity = 127;
                    if (pastVelocity != Velocity)
                        Play(false);
                }
                else if (!banyan_rhythm)
                {
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                        Play(false);

                    if (Input.GetKeyUp(KeyCode.Return))
                        StopOneNote();
                }

                //modulation simulation
                if (banyan_modulate)
                {
                    var sensorPitch = Instrument.GetComponent<MessageProcessor>().pitch;
                    if (sensorPitch == 500)
                        PitchChange = DEFAULT_PITCH;
                    else
                    {
                        PitchChange = sensorPitch / 10;
                    }
                    var sustain = Instrument.GetComponent<MessageProcessor>().sustain;
                    /*if (sustain == 500)
                        //print("defualt sustain");
                    else*/
                      
                }
                else if (!banyan_modulate)
                    PitchChange = DEFAULT_PITCH;
            }

            pastVelocity = Velocity;

        }

        /// <summary>
        /// Play music depending the parameters set
        /// </summary>
        /// <param name="stopCurrent">stop current note playing</param>
        void Play(bool stopCurrent)
        {        
            if (stopCurrent)
                StopOneNote();
            PlayOneNote();
            
        }

       
        private void PlayOneNote()
        {
            //Debug.Log($"{StreamChannel} {midiStreamPlayer.MPTK_ChannelPresetGetName(StreamChannel)}");
            // Start playing a new note
            isPlaying = true;
            NotePlaying = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,                                      //wird getriggert wenn alle Instrumentenkompont aktiv sind
                Value = CurrentNote,                                              //Tonhöhe: Wird von der Melodiekomponente festgelegt
                Channel = StreamChannel,                                         //bleibt gleich
                Duration = -1,//Convert.ToInt64(NoteDuration * 1000f), // milliseconds, -1 to play undefinitely (Duration = -1) // wird von Rhythmuskomponente beeinflusst
                Velocity = Velocity, // Sound can vary depending on the velocity //wird von Rhythmuskomponente beeinflusst
                                     
            };
            midiStreamPlayer.MPTK_PlayEvent(NotePlaying);
           
        }
      
      
          
        private void StopOneNote()                              //muss aufgerufen werden um die Note zu beenden
        {
            if (NotePlaying != null)
            {
                isPlaying = false;
                //Debug.Log("Stop note");
                // Stop the note (method to simulate a real human on a keyboard : 
                // duration is not known when note is triggered)
                midiStreamPlayer.MPTK_StopEvent(NotePlaying);
                NotePlaying = null;
               
            }
        }

        private int getKeys()
        {
            if (Input.GetKeyDown(KeyCode.A))
                keyNote = 42;
            else if (Input.GetKeyDown(KeyCode.S))
                keyNote = 44;
            else if (Input.GetKeyDown(KeyCode.D))
                keyNote = 46;
            else if (Input.GetKeyDown(KeyCode.F))
                keyNote = 47;
            else if (Input.GetKeyDown(KeyCode.G))
                keyNote = 49;
            else if (Input.GetKeyDown(KeyCode.H))
                keyNote = 51;
            else if (Input.GetKeyDown(KeyCode.J))
                keyNote = 53;
            else if (Input.GetKeyDown(KeyCode.K))
                keyNote = 54;
            else if(Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.S)|| Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.G) || Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.K))
                keyNote = -1;
            return keyNote;
        }
    }
}