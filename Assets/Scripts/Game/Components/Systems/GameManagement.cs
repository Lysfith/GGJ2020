using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagement : MonoBehaviour
{
    private float _time = 0;
    private int _startingtime = 3;
    private TextMeshProUGUI _tmp;
    private bool _active = true;

    //Objets geres
    Timer _Timer;

    // Start is called before the first frame update
    void Start()
    {
        _tmp = Instantiate(Resources.Load<GameObject>("Text (TMP)"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<TextMeshProUGUI>();
        _tmp.text = "Get Ready !";
        _tmp.fontSize = 80;
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
                _tmp.text = (_startingtime--).ToString();
                _time = 0;
                if (_startingtime == -1)
                    _tmp.text = "GO";
                if (_startingtime == -2)
                {
                    Destroy(_tmp.gameObject);
                    _active = false;
                    _Timer.TimerReset();
                }
            }
        }
    }

    public void EndGame()
    {
        _tmp = Instantiate(Resources.Load<GameObject>("Text (TMP)"), GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<TextMeshProUGUI>();
        _tmp.text = "Fini !!";

        StartCoroutine(WaitForClose());
        _tmp.text = "";
        //Application.Quit();

    }
    IEnumerator WaitForClose()
    {
        yield return new WaitForSeconds(5);
    }



    public void RegisterTimer(Timer t)
    {
        _Timer = t;
    }
}
