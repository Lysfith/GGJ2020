using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class FadeEffect : MonoBehaviour
{

    public float _offset = .5f;
    public float _magnitude = .5f;
    public float _pulsation = 1;
    private TextMeshProUGUI _ugui;

    private void Start()
    {
        _ugui = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _ugui.color = new Color(_ugui.color.r, _ugui.color.g, _ugui.color.b, _offset + _magnitude * Mathf.Sin(_pulsation * Time.time));
        }

    }


}
