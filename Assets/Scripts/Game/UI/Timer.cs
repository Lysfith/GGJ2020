using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float _timeMax = 600;
    public float _time = 60;
    public int _timetoadd = 30;
    private bool end = true;

    public Color _startColor, _endColor;
    public Renderer _landscapeRenderer;

    private Color _currentColor;


    private void Start()
    {
        _currentColor = _startColor;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManagement>().RegisterTimer(this);
        this.gameObject.GetComponent<TextMeshProUGUI>().text = System.Math.Truncate(_time).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            _time -= Time.deltaTime;
            this.gameObject.GetComponent<TextMeshProUGUI>().text = System.Math.Truncate(_time).ToString();

            Color targetColor = Color.Lerp(_startColor, _endColor, 1.0f - _time / _timeMax);

            _currentColor = Color.Lerp(_currentColor, targetColor, Time.deltaTime);
            _landscapeRenderer.material.SetColor("_Temperature", _currentColor);

            if (_time <= 0)
            {
                end = true;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManagement>().EndGame();
            }
        }
    }

    public void AddTime()
    {
        _time += _timetoadd;
    }

    public void TimerReset(float t = 60)
    {
        _time = _timeMax;
        end = false;
    }

}
