using Assets.Scripts.Global.Components;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MainMenu : MonoBehaviour
{

    public RectTransform _logo;
    public Vector2 _floorPosition;
    public float _duration;
    public float _bounceDelay;
    public float _bounceDuration;
    public Vector2 _magnitude;

    public RectTransform _anyButtonText;
    private bool _isEnabled;

    public S_SceneFaderSystem _sceneFader;
    

    void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.Insert(0, _logo.DOAnchorPos(_floorPosition, _duration).SetEase(Ease.OutBounce));
        sequence.Insert(_bounceDelay, _logo.DOScaleY(_magnitude.y, _bounceDuration / 2).SetEase(Ease.OutQuad));
        sequence.Insert(_bounceDelay + _bounceDuration / 2, _logo.DOScaleY(1f, _bounceDuration / 2).SetEase(Ease.OutQuad));
        sequence.Insert(_bounceDelay, _logo.DOScaleX(_magnitude.x, _bounceDuration / 2).SetEase(Ease.OutQuad));
        sequence.Insert(_bounceDelay + _bounceDuration / 2, _logo.DOScaleX(1f, _bounceDuration / 2).SetEase(Ease.OutQuad));
        sequence.AppendInterval(.5f);
        sequence.OnComplete(EnableMenu);
    }

    private void Update()
    {
        if(_isEnabled)
        {
            bool anyPress = false;
            foreach (var gamepad in Gamepad.all)
            {
                if (gamepad.allControls.Any(x => x is ButtonControl button && ((ButtonControl) x).wasPressedThisFrame && !x.synthetic))
                {
                    anyPress = true;
                }
            }
            if(anyPress)
            {
                _sceneFader.FadeOut("PlayerSelectionMenu");
            }
        }
    }

    void EnableMenu()
    {
        _isEnabled = true;
        _anyButtonText.gameObject.SetActive(true);
    }
}
