using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PopupPopper : MonoBehaviour
{
    private GameObject _tip = null;
    public void ShowToolTip(GameObject origin, int popupnum)
    {
        GameObject popup = Resources.Load<GameObject>("tips/tip" + popupnum.ToString());
        if (_tip != null)
            Destroy(_tip);
        _tip = Instantiate(popup, origin.transform);

        Assert.IsNotNull(_tip);


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
