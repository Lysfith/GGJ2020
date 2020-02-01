using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObject/SoundList", order = 0)]
public class SoundList : ScriptableObject
{
    public enum Sound
    {
        footstep,
        footstepalt
    }

    public AudioClip footstep;
    public AudioClip footstepalt;

}
