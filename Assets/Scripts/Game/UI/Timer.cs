using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float _time = 60;
    public int _timetoadd = 30;
    private bool end = true;

    private void Start()
    {
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
        _time = 60;
        end = false;
    }

}
