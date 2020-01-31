using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFunctionOnClick : MonoBehaviour
{
    public GameObject Objet;
    public class maclasse { };
    void OnMouseDown()
    {
        
        Objet.GetComponent<maclasse>();
    }
}
