using Assets.Scripts.Global.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CreditSequence : MonoBehaviour
{


    public S_SceneFaderSystem _fader;
    public float _duration = 2f;
    public float _fadeDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(_duration);
        sequence.OnComplete(() => _fader.FadeOut("MainMenu", _fadeDuration));
    }

}
