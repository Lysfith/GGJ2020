using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Systems
{
    static class SoundManager
    {

        [SerializeField] static AudioClip _footstep;
        [SerializeField] static AudioClip _footstepalt;



        static public void StartFoodStepSound(GameObject player)
        {
            player.GetComponent<AudioSource>().clip = _footstep;
            player.AddComponent<SoundSteps>().SetSoundSteps(_footstep, _footstepalt);
        }

        static public void StopFoodStepSound(GameObject player)
        {
            player.GetComponent<SoundSteps>().DestroySelf();
        }

        static public bool IsSounding(GameObject player)
        {
            return player.GetComponent<AudioSource>().clip != null;
        }
    }
}