using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameManagement : MonoBehaviour
{
    private float _time = 0;
    private int _startingtime = 4;
    private TextMeshProUGUI _tmp;
    private bool _active = true;

    //Objets geres
    Timer _Timer;
    Counter _Counter;

    public UnityEvent OnStart;
    public UnityEvent OnEnd;

    // Start is called before the first frame update
    void Start()
    {
        _tmp = Instantiate(Resources.Load<GameObject>("Text (TMP)"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<TextMeshProUGUI>();
        _tmp.text = "Get Ready !";
        PopupManager.Activate();

    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            _time += Time.deltaTime;
            if (_time >= 1)
            {
                _time = 0;
                _startingtime--;
                switch (_startingtime)
                {
                    case 3:
                        _tmp.text = _startingtime.ToString();
                        SoundManager.PlaySound(SoundList.Sound.three);
                        break;
                    case 2:
                        _tmp.text = _startingtime.ToString();
                        SoundManager.PlaySound(SoundList.Sound.two);
                        break;
                    case 1:
                        _tmp.text = _startingtime.ToString();
                        SoundManager.PlaySound(SoundList.Sound.one);
                        break;
                    case 0:
                        _tmp.text = "GO";
                        SoundManager.PlaySound(SoundList.Sound.play);
                        break;
                    case -1:
                        Destroy(_tmp.gameObject);
                        _active = false;
                        _Timer.TimerReset();

                        OnStart?.Invoke();
                        break;
                }
            }
        }
    }

    public void EndGame()
    {
        _tmp = Instantiate(Resources.Load<GameObject>("Text (TMP)"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<TextMeshProUGUI>();
        _tmp.text = "Fini !!";

        OnEnd?.Invoke();

        StartCoroutine(WaitForClose());
        //Application.Quit();

    }
    IEnumerator WaitForClose()
    {
        yield return new WaitForSeconds(5);
    }

    public void AddOneToCount()
    {
        _Counter.Add();
        _Timer.AddTime();
    }

    public void RegisterTimer(Timer t)
    {
        _Timer = t;
    }

    public void RegisterCounter(Counter c)
    {
        _Counter = c;
    }
}
