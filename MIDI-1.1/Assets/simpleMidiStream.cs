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

        [Range(0.05f, 10f)]
        public float Frequency = 1;

        [Range(-10f, 100f)]
        public float NoteDuration = 0;

        [Range(0f, 10f)]
        public float NoteDelay = 0;


        /*bool RandomPlay = false; //muss vielleicht alles public bleiben, damit es funktioniert?
        bool DrumKit = false;
        bool ChordPlay = false;
        int ArpeggioPlayChord = 0;
        int DelayPlayScale = 200;
        bool ChordLibPlay = false;
        bool RangeLibPlay = false;
        int CurrentChord;*/

        [Range(0, 127)]    //start und endnote müssten eigentlich dieselbe sein?
        public int StartNote = 50;

        [Range(0, 127)]
        public int EndNote = 70;

        [Range(0, 127)] //lautstärke des Tons
        public int Velocity = 100;

        [Range(0, 16)]
        int StreamChannel = 0;

        [Range(0, 127)]
        public int CurrentNote;

        [Range(0, 127)] //definiert welches Instrument simuliert wird und müsste dasselbe sein wie EndPreset? 
        public int StartPreset = 0;

        [Range(0, 127)]
        public int EndPreset = 127;

        [Range(0, 127)]
        public int CurrentPreset;

        [Range(0, 127)]
        public int CurrentPatchDrum;

        [Range(0, 127)]
        public int PanChange;

        [Range(0, 127)]
        public int ModChange;

        const float DEFAULT_PITCH = 64;
        [Range(0, 127)] //Tonhöhe
        public float PitchChange = DEFAULT_PITCH;
        private float currentVelocityPitch;
        private float LastTimePitchChange;

        [Range(0, 127)]
        public int ExpChange;

        public int CountNoteToPlay = 1;
        public int CountNoteChord = 3;
        public int DegreeChord = 1;
        public int CurrentScale = 0;

        /// <summary>
        /// Current note playing
        /// </summary>
        private MPTKEvent NotePlaying;

        private float LastTimeChange;
                                                            //GUI Kram kann alles weg?

        // Popup to select a realtime generator
        private PopupListItem[] PopGenerator;
        private int[] indexGenerator;
        private string[] labelGenerator;
        private float[] valueGenerator;
        private const int nbrGenerator = 4;

        // Manage skin
        /*public CustomStyle myStyle;  
        public bool IsplayingLoopNotes;
        public bool IsplayingLoopPresets;*/

        //get banyan Melody info
        public GameObject myMelody;
        private int myNote;

        private void Awake()                                        //passiert als erstes
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

            myNote = myMelody.GetComponent<simpleMIDIMessageProcessor>().note;
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
            CurrentNote = StartNote;
            PanChange = 64;
            LastTimeChange = -9999999f;
            PitchChange = DEFAULT_PITCH;
            CountNoteToPlay = 1;
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

        public bool testLocalchange = false;


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
            if (midiStreamPlayer != null && myNote!=-1)
            {
                if (myNote==1)
                {
                    //StartNote = 54;  //C3
                    CurrentNote = 53;
                }
                else if (myNote == 2)
                {
                    //StartNote = 56;
                    CurrentNote = 55;
                }
                else if(myNote == 3)
                {
                    //StartNote = 58;
                    CurrentNote = 57;
                }
                else if (myNote == 4)
                {
                    //StartNote = 59;
                    CurrentNote = 58;
                }
                else if (myNote == 5)
                {
                    //StartNote = 61;
                    CurrentNote = 60;
                }
                else if (myNote == 6)
                {
                    //StartNote = 63;
                    CurrentNote = 62;
                }
                else if (myNote == 7)
                {
                    //StartNote = 65;
                    CurrentNote = 64;
                    //EndNote = 65;
                }
                else if (myNote == 8)
                {
                    //StartNote = 66;
                    CurrentNote = 65;
                    //EndNote = 66;
                }
                print("return key pressed");
                midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent()
                {
                    Command = MPTKCommand.PatchChange,
                    Value = CurrentPreset,
                    Channel = StreamChannel,
                });


                
                if (++CurrentNote > EndNote) CurrentNote = StartNote;
                if (CurrentNote < StartNote) CurrentNote = StartNote;
                print("currentNote became startNote");

                // Play note or chrod or scale without stopping the current (useful for performance test)
                Play(false);
                print("Play(false);");
                    
            }
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

        //! [Example MPTK_PlayEvent]                                Die Noten werden hier erzeugt
        /// <summary>
        /// Send the note to the player. Notes are plays in a thread, so call returns immediately.
        /// The note is stopped automatically after the Duration defined.
        /// </summary>
        /// @snippet TestMidiStream.cs Example MPTK_PlayEvent
        private void PlayOneNote()
        {
            print("PlayOneNote");
            //Debug.Log($"{StreamChannel} {midiStreamPlayer.MPTK_ChannelPresetGetName(StreamChannel)}");
            // Start playing a new note
            NotePlaying = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,                                      //wird getriggert wenn alle Instrumentenkompont aktiv sind
                Value = CurrentNote,                                              //Tonhöhe: Wird von der Melodiekomponente festgelegt
                Channel = StreamChannel,                                         //bleibt gleich
                Duration = Convert.ToInt64(NoteDuration * 1000f), // milliseconds, -1 to play undefinitely (Duration = -1) // wird von Rhythmuskomponente beeinflusst
                Velocity = Velocity, // Sound can vary depending on the velocity //wird von Rhythmuskomponente beeinflusst
                Delay = Convert.ToInt64(NoteDelay * 1000f),                         //wird von Extra-Komponente beeinflusst
            };
            midiStreamPlayer.MPTK_PlayEvent(NotePlaying);
            print("one note played");
        }
        //! [Example MPTK_PlayEvent]

        /// <summary>
        /// Play one shot
        /// </summary>
        /// 
          
        private void StopOneNote()                              //muss aufgerufen werden um die Note zu beenden
        {
            print("StopOneNote");
            if (NotePlaying != null)
            {
                Debug.Log("Stop note");
                // Stop the note (method to simulate a real human on a keyboard : 
                // duration is not known when note is triggered)
                midiStreamPlayer.MPTK_StopEvent(NotePlaying);
                print("note is stoped"); //wird nicht ausgegeben?
                NotePlaying = null;
               
            }
        }
    }
}