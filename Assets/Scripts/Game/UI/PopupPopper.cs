using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPopper : MonoBehaviour
{
    private GameObject _tip = null;
    public void ShowToolTip()
    {
        if (_tip != null)
            Destroy(_tip);
        Instantiate(Resources.Load<GameObject>("tip"), GameObject.FindGameObjectWithTag("Canvas").transform);



    }

    public void HideToolTip()
    {
        if (_tip != null)
            Destroy(_tip);
    }



    private void OnDestroy()
    {
        HideToolTip();
    }
}
