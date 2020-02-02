using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class GameManagement : MonoBehaviour
{
    private float _time = 0;
    private int _startingtime = 4;
    private TextMeshProUGUI _tmp;
    private bool _active = false;

    //Objets geres
    Timer _Timer;
    Counter _Counter;

    public UnityEvent OnStart;
    public UnityEvent OnEnd;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void StartTimer()
    {
        _tmp = Instantiate(Resources.Load<GameObject>("Text (TMP)"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<TextMeshProUGUI>();
        _tmp.text = "Get Ready !";
        PopupManager.Activate();

        _active = true;
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
                        GameObject.FindGameObjectWithTag("Canvas").transform.parent.GetComponents<AudioSource>()[1].Play();
                        OnStart?.Invoke();
                        break;
                }
            }
        }
        if (PopupManager._Isactive && _Timer._time < (_Timer._timeMax/4)*3)
            PopupManager.Deactivate();
    }

    public void EndGame()
    {
        _tmp = Instantiate(Resources.Load<GameObject>("Text (TMP)"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<TextMeshProUGUI>();
        _tmp.text = "Fini !!";

        OnEnd?.Invoke();
        if (Resources.Load<HighScore>("HighScore").score[19] <= _Counter._count)
        {
            Instantiate(Resources.Load<GameObject>("InputField (TMP)"), GameObject.FindGameObjectWithTag("Canvas").transform);
        }

        //StartCoroutine(WaitForClose());
        //Application.Quit();

    }

    public void SendHighScore(TMP_InputField name)
    {
        if (name.text == "")
            name.text = "Mr. Default";
        HighScore hs = Resources.Load<HighScore>("HighScore");
        int i = 0;
        while (hs.score[i] > _Counter._count && i<20)
             i++;

        for (int j = 19; j > i ; j--)
        {
            hs.score[j] = hs.score[j-1];
            hs.joueur[j] = hs.joueur[j-1];
        }
        hs.score[i] = _Counter._count;
        hs.joueur[i] = name.text;
        Destroy(name.gameObject);

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
