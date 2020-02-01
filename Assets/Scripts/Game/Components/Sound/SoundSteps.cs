using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSteps : MonoBehaviour
{

    AudioClip _footstep;
    AudioClip _footstepalt;
    AudioSource _source;
    int _time = 0;

    public void SetSoundSteps(AudioClip FS1, AudioClip FS2)
    {
        _footstep = FS1;
        _footstepalt = FS2;
    }



    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_source.isPlaying)
        {
            if (_time == 0)
                _source.clip = _footstepalt;
            if (_time == 4)
            {
                _time = -1;
                _source.clip = _footstepalt;
            }
            _source.Play();
            _time++;

        }
    }

    public void DestroySelf()
    {
        _source.clip = null;
        Destroy(this);
    }
}
