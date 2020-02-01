using Assets.Scripts.Game.Components.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSteps : MonoBehaviour
{
    AudioSource _source;
    int _time = 0;


    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = SoundManager.GetAudioClip(SoundList.Sound.footstep);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_source.isPlaying)
        {
            if (_time == -1)
            {
                _source.clip = SoundManager.GetAudioClip(SoundList.Sound.footstep);
                _time = Random.Range(0, 6);
            }
            if (_time == 0)
            {
                _source.clip = SoundManager.GetAudioClip(SoundList.Sound.footstepalt);
            }
            _source.Play();
            _time--;

        }
    }

    public void DestroySelf()
    {
        _source.clip = null;
        Destroy(this);
    }
}
