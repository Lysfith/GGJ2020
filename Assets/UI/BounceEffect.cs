using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BounceEffect : MonoBehaviour
{

    public AnimationCurve _curve;
    public float _duration = 1.0f;
    public UnityEvent _callBack;
    private Vector3 _referenceScale;

    private void OnEnable()
    {
        _referenceScale = transform.localScale;
    }

    public void Play()
    {
        if(gameObject.activeSelf)
        {
            StartCoroutine(BounceRoutine());
        }

    }

    public IEnumerator BounceRoutine()
    {
        
        float progress = .0f;
        while(progress <= _duration)
        {
            progress += Time.deltaTime;
            transform.localScale = _referenceScale * _curve.Evaluate(progress / _duration);
            yield return null;
        }
        _callBack.Invoke();
    }

}
