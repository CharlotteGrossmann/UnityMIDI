# UnityMIDI

Goal: 

- Play background music

- Send analog inputs from Raspberry Pis via Banyan to Unity
  There are 5 Raspberry Pis running: Keyboard Melody; Keyboard Rhythm; Keyboard Modulation; Flute Melody and Flute Rhythm (see this repository: https://github.com/CharlotteGrossmann/Raspi-Banyan)
  The respective inputs are send via Banyan to the Backplane running on the computer running Unity and further to Unity itself.
  The instrument activity in the form of several variables are now available in C# in Unity
  
- Use analog inputs to play music
  The instrument activty is used to create midi notes (using Midi Tool Kit Free).
  The volume and time to create or stop the notes depend on the rhythm components (using a Force Sensitiy Resistor).
  The note frequency depends on the melody components (using 8 push buttons).
  The modulation component (usind a joystick) influences the pitch and/or sustain of the notes.
  All keyboard Ccmponets have to be active in order to create a note. That goes too for the flute components.
  
- Use analog inputs to change motion graphics
  The prototype outputs three display views.
  One view for each instrument (flute and keyboard) and one for the group alltogether.
  The instrument specific view (group view) displays which components are currently active, visualizes the beat of the background music and vizualises the impact the components have and when a note is played.
  The group view shows the meassure and beat of the background music, and the audible notes that are being played.




In order to start the banyan communication

1. backplane has to run on pc
2. unitylistener.py and unitygateway.py have to run on pc (check if they run on the same IP as backplane)
3. The script sendig messages has to run (unitygateway.py should print this message "Your Unity receiver is not listening on: 127.0.0.1, port: 5000" along with "Could not send message:" + the message your script is sending
4. The Unity scene should be playing (the unitygateway.py messages should stop)



###Used Ressources:

Banyan Unity: https://github.com/NoahMoscovici/banyanunity

Python Banyan: https://github.com/MrYsLab/python_banyan

Midi Tool Kit Free: https://assetstore.unity.com/packages/tools/audio/midi-tool-kit-free-107994

**Works with Unity 2019.3.15f1**
