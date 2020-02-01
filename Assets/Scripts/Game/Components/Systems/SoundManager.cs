using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Systems
{
    class SoundManager : MonoBehaviour
    {

        [SerializeField] public static AudioClip _footstep;
        [SerializeField] public static AudioClip _footstepalt;



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
    }
}