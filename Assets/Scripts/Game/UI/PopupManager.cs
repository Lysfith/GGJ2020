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

        Vector3 coordinates = new Vector3(0,0,0);
        GameObject[] workbenches = GameObject.FindGameObjectsWithTag("Workbench");
        foreach (GameObject wb in workbenches)
        {
            if(wb.GetComponent<C_Workbench>().GetWorkbenchType() == objecttype)
            {
                coordinates = wb.transform.position;
            }
        }
        Assert.IsNotNull(player.GetComponent<PopupPopper>());
        player.GetComponent<PopupPopper>().ShowToolTip(coordinates);
    }
    public static void RemoveTipOnPlayer(GameObject player )
    {
        if (!_Isactive )
            return;
        if(player.GetComponent<PopupPopper>() != null)
            player.GetComponent<PopupPopper>().HideToolTip();
    }
}
