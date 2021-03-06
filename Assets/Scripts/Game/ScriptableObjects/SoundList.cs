﻿using System.Collections;
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
        parachute,
        robotclonc,
        boxopen,
        one,
        two,
        three,
        play,
        error,
        login,
        select,
        ready,
        start,
        glou,
        cucaracha,
        accel,
        decel,
        elec
    }

    public AudioClip footstep;
    public AudioClip footstepalt;
    public AudioClip workbenchtap;
    public AudioClip workbenchturn;
    public AudioClip workbenchok;
    public AudioClip droprobot;
    public AudioClip dropsmall;
    public AudioClip addpart;
    public AudioClip parachute;
    public AudioClip robotclonc;
    public AudioClip boxopen;
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    public AudioClip play;
    public AudioClip error;
    public AudioClip login;
    public AudioClip select;
    public AudioClip ready;
    public AudioClip start;
    public AudioClip glou;
    public AudioClip cucaracha;
    public AudioClip accel;
    public AudioClip decel;
    public AudioClip elec;


}
