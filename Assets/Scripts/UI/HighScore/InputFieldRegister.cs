using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldRegister : MonoBehaviour
{
    public void Validate()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManagement>().SendHighScore(GetComponent<TMP_InputField>());
    }
}
