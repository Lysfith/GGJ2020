using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PopupPopper : MonoBehaviour
{
    private GameObject _tip = null;
    public void ShowToolTip(Vector3 coordinates)
    {
        if (_tip != null)
            Destroy(_tip);
        _tip = Instantiate(Resources.Load<GameObject>("tip"), GameObject.FindGameObjectWithTag("Canvas").transform);
        _tip.transform.position = new Vector3(coordinates.x, _tip.transform.position.y, coordinates.z);


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
