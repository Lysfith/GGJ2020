﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundList;
public static class SoundManager
{
    static public void StartFoodStepSound(GameObject player)
    {
        player.AddComponent<SoundSteps>();
    }

    static public void StopFoodStepSound(GameObject player)
    {
        player.GetComponent<SoundSteps>().DestroySelf();
    }
    static public bool IsSounding(GameObject player)
    {

        return player.GetComponent<AudioSource>().isPlaying;
    }
    static public AudioSource AvailableSource(GameObject player )
    {
        AudioSource[] sources = player.GetComponents<AudioSource>();
        if (sources.Length > 1 && sources[1] != null && !sources[1].isPlaying)
            return sources[1];
        if (sources[0]!= null && !sources[0].isPlaying)
            return sources[0];

        return null;
    }


    static public void PlaySound(Sound sound, GameObject player = null, bool priority = true, float pitch = 1, ulong delay = 0)
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("MainCamera");
        if ((IsSounding(player) && !priority))
            return;

        AudioSource source = AvailableSource(player);
        if(source != null)
        {
            source.clip = GetAudioClip(sound);
            source.pitch = pitch;
            source.Play(delay);
            return;
        }

        if (player.GetComponents<AudioSource>()[0].clip == GetAudioClip(sound) || player.GetComponents<AudioSource>()[1].clip == GetAudioClip(sound))
            return;
        source = player.GetComponents<AudioSource>()[1];
        source.clip = GetAudioClip(sound);
        source.pitch = pitch;
        source.Play(delay);


    }




    static public AudioClip GetAudioClip(Sound sound)
    {
        SoundList sl = Resources.Load<SoundList>("SoundList");
        switch (sound)
        {
            case Sound.footstep:return sl.footstep;
            case Sound.footstepalt: return sl.footstepalt;
            case Sound.workbenchtap: return sl.workbenchtap;
            case Sound.workbenchturn: return sl.workbenchturn;
            case Sound.workbenchok:return sl.workbenchok;
            case Sound.droprobot:return sl.droprobot;
            case Sound.dropsmall: return sl.dropsmall;
            case Sound.addpart: return sl.addpart;
            case Sound.parachute: return sl.parachute;
            case Sound.robotclonc: return sl.robotclonc;
            case Sound.boxopen: return sl.boxopen;
            case Sound.play: return sl.play;
            case Sound.one: return sl.one;
            case Sound.two: return sl.two;
            case Sound.three: return sl.three;
            case Sound.error: return sl.error;
            case Sound.login: return sl.login;
            case Sound.select: return sl.select;
            case Sound.ready: return sl.ready;
            case Sound.start: return sl.start;
            case Sound.glou: return sl.glou;
            case Sound.cucaracha: return sl.cucaracha;
            case Sound.accel: return sl.accel;
            case Sound.decel: return sl.decel;
            case Sound.elec: return sl.elec;

            default: Debug.LogError("Attention. Son non implemente");
                return null;
        }

    }
}
