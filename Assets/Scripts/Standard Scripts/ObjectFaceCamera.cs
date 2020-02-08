using Assets.Scripts.Game.Components.Characters.Others;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFaceCamera : MonoBehaviour
{
    private Transform camera;
    private bool _inverted = false;
    private C_CharacterStats _coord;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _coord = transform.parent.GetComponent<C_CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera);

        if((_inverted && _coord.Direction.x> 0) || (!_inverted && _coord.Direction.x < 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _inverted = !_inverted;
        }


    }
}
