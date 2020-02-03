using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadThis : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void DestroyNow()
    {
        Destroy(this.gameObject);
    }
 
}
