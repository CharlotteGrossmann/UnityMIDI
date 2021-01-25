# UnityMIDI

Goal: 

- Get analog inputs from Raspberry Pi via Banyan
- Use analog inputs to change midi sounds
- Use analog inputs to change motion graphics

Work in progress is found in the "SimpleMIDICreator" scene in the "SelfMade" directory within the unity assets.

In order to start the banyan communication

1. backplane has to run on pc
2. unitylistener.py and unitygateway.py have to run on pc
3. The Unity scene should be playing

In order to work with user input, you can...<br>
... use a-k keys to simulate the melody component, activate the melody_simulator.py script on the pc or start the respective raspberry script<br>
... use the return key to simulate the rhythm component, activate the rythm_simulator.py script on the pc or start the respective raspberry script<br>
... activate the modulate_simulator.py python_banyan directory<br>


###Used Plugins:

Banyan Unity

Midi Tool Kit Free

**Works with Unity 2019.3.15f1**
