using Assets.Scripts;
using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.Components.Repairing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PopupManager : MonoBehaviour
{
    private static GameObject _canvas;
    public static bool _Isactive = false;
    // Start is called before the first frame update
    void Start()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
    }


    public static void Activate() { 
        _Isactive = true;
        //foreach (GameObject e in GameObject.FindGameObjectsWithTag("Player"))
        //    e.AddComponent<PopupPopper>();

    }
    public static void Deactivate() { 
        _Isactive = false;
        //foreach (PopupPopper e in FindObjectsOfType<PopupPopper>())
        //    Destroy(e);
    }

    public static void ShowTipOnPlayer(GameObject player, ObjectType objecttype)
    {
        //if (!_Isactive)
        //    return;

        //GameObject[] workbenches = GameObject.FindGameObjectsWithTag("Workbench");
        //foreach (GameObject wb in workbenches)
        //{
        //    if(wb.GetComponent<C_Workbench>().GetWorkbenchType() == objecttype)
        //    {
        //        Assert.IsNotNull(player.GetComponent<PopupPopper>());
        //        player.GetComponent<PopupPopper>().ShowToolTip(wb,1);
        //    }
        //}
        //if(objecttype == ObjectType.PACK)
        //    player.GetComponent<PopupPopper>().ShowToolTip(player,3);
        //if (objecttype == ObjectType.WASTE)
        //    player.GetComponent<PopupPopper>().ShowToolTip(player, 2);
        //if (objecttype == ObjectType.COMPLETED)
        //    player.GetComponent<PopupPopper>().ShowToolTip(GameObject.FindGameObjectWithTag("Bot"), 1);



    }
    public static void RemoveTipOnPlayer(GameObject player )
    {
        //if (!_Isactive )
        //    return;
        //if(player.GetComponent<PopupPopper>() != null)
        //    player.GetComponent<PopupPopper>().HideToolTip();
    }
}
