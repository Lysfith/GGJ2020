using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Counter : MonoBehaviour
{

    public int _count = 0;

    // Start is called before the first frame update
    public void Start()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = _count.ToString();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManagement>().RegisterCounter(this);
    }
    public void Add()
    {
        _count++;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = _count.ToString();
    }
}
