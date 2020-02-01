using Assets.Scripts.Game.Components.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PopupManager : MonoBehaviour
{
    private static GameObject _canvas;
    private static bool _Isactive = false;
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
    }


    public static void Activate() { 
        _Isactive = true;
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Player"))
            e.AddComponent<PopupPopper>();

    }
    public static void Deactivate() { 
        _Isactive = false;
        foreach (PopupPopper e in FindObjectsOfType<PopupPopper>())
            Destroy(e);
    }

    public static void ShowTipOnPlayer(GameObject player, ObjectType objecttype)
    {
        if (!_Isactive)
            return;
        Assert.IsNotNull(player.GetComponent<PopupPopper>());
        player.GetComponent<PopupPopper>().ShowToolTip();
    }
    public static void RemoveTipOnPlayer(GameObject player )
    {
        if (!_Isactive )
            return;
        Assert.IsNotNull (player.GetComponent<PopupPopper>());
        player.GetComponent<PopupPopper>().HideToolTip();
    }
}
