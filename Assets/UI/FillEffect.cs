using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FillEffect : MonoBehaviour
{

    public AnimationCurve _curve;
    public float _duration = 1.0f;
    public UnityEvent _callBack;

    public void Play()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(FillCoroutine());
        }
    }

    public IEnumerator FillCoroutine()
    {
        Image image = GetComponent<Image>();
        float progress = .0f;
        while(progress <= _duration)
        {
            progress += Time.deltaTime;
            image.fillAmount = progress / _duration;
            yield return null;
        }
        _callBack.Invoke();
    }

}
