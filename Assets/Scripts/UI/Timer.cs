using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float _time = 60;
    public int _timetoadd = 30;
    private bool end = false;

    private void Start()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = System.Math.Truncate(_time).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = System.Math.Truncate(_time).ToString();
        if (!end && _time <= 0)
        {
            end = true; //Fun to end
        }
    }

    public void AddTime()
    {
        _time += _timetoadd;
    }

    public void TimerReset(float t = 60)
    {
        _time = 60;
        end = false;
    }

}
