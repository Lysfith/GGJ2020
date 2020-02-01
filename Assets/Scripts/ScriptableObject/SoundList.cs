using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObject/SoundList", order = 0)]
public class SoundList : ScriptableObject
{
    public enum Sound
    {
        footstep,
        footstepalt,
        workbenchtap,
        workbenchturn,
        workbenchok,
        droprobot,
        dropsmall,
        addpart,
        
    }

    public AudioClip footstep;
    public AudioClip footstepalt;
    public AudioClip workbenchtap;
    public AudioClip workbenchturn;
    public AudioClip workbenchok;
    public AudioClip droprobot;
    public AudioClip dropsmall;
    public AudioClip addpart;

}
