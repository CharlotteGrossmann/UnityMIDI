from setuptools import setup
setup(
    name='ecs',
    version='1.0',
    packages=[
        'ecs',
    ],
    install_requires=[
        'pyzmq',
        'u-msgpack-python',
        'msgpack-python',
        'numpy>=1.9',
        'msgpack-numpy',
        'psutil'
    ],

    entry_points={
        'console_scripts': [
            'gateway = ecs.unitygateway:unity_gateway',
            'listener = ecs.unitylistener:unity_listener',
            'stringModulate = ecs.string_modulate_simulator_unity:unity_test',
            'stringRhythm = ecs.string_rhythm_simulator_unity:unity_test',
            'stringMelody = ecs.string_melody_simulator_unity:unity_test',
            'woodwindRhythm = ecs.woodwind_rhythm_simulator_unity:unity_test',
            'woodwindMelody = ecs.woodwind_melody_simulator_unity:unity_test',
            'keyboardRhythm = ecs.keyboard_rhythm_simulator_unity:unity_test',
            'keyboardMelody = ecs.keyboard_melody_simulator_unity:unity_test'
        ]
    },
)