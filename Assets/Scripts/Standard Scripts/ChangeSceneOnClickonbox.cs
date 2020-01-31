using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClickonbox : MonoBehaviour
{

    public string _nextScene;
    

        void OnMouseDown ()
        {
            SceneManager.LoadScene(_nextScene);
        }

 }
