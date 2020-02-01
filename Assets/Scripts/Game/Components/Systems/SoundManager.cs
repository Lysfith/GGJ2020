using System.Collections;
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

    static public void PlaySound(Sound sound, GameObject player = null, bool priority = false)
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("MainCamera");
        if (IsSounding(player) && !priority)
            return;
        AudioSource source = player.GetComponent<AudioSource>();
        source.clip = GetAudioClip(sound);
        source.Play();

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

            default: Debug.LogError("Attention. Son non implemente");
                return null;
        }

    }
}
