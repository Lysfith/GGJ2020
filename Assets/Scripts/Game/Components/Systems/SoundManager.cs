using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return player.GetComponent<AudioSource>().clip != null;
    }

    static public AudioClip GetAudioClip(SoundList.Sound sound)
    {
        SoundList sl = Resources.Load<SoundList>("SoundList");
        switch (sound)
        {
            case SoundList.Sound.footstep:return sl.footstep;
            case SoundList.Sound.footstepalt: return sl.footstepalt;
            default: return null;
        }

    }
}
