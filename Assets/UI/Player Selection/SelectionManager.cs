using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Game.ScriptableObjects.Characters;
using Assets.Scripts;
using UnityEngine.Assertions;

public class SelectionManager : MonoBehaviour
{

    public RectTransform[] _playerUISlots;
    public PlayerSlot[] _playerSlots;
    public PlayerModels _playerModels;

    public Transform[] _playerTransforms;
    public Transform _startTransform;

    public Dictionary<Gamepad, int> _gamepadSlots;
    public Color[] _playerColors;
    public int _firstEmpty;

    private void Start()
    {
        _gamepadSlots = new Dictionary<Gamepad, int>();
        foreach (var playerSlot in _playerSlots)
        {
            playerSlot._active = false;
            playerSlot._ready = false;
            playerSlot._gamepad = null;
        }
    }

    private void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    PlayerReady(gamepadSlot);
                }
                else if (_firstEmpty < _playerUISlots.Length)
                {
                    PlayerJoin(gamepad);
                }
            }
            else if (gamepad.buttonEast.wasPressedThisFrame)
            {
                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    PlayerLeave(gamepadSlot);
                }

            }

            if (gamepad.leftShoulder.wasPressedThisFrame || gamepad.leftTrigger.wasPressedThisFrame || gamepad.dpad.left.wasPressedThisFrame || gamepad.leftStick.left.wasPressedThisFrame)
            {
                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    _playerUISlots[gamepadSlot].Find("Info").Find("Left").GetComponent<BounceEffect>().Play();
                    ChangeModel(gamepadSlot, ((int) _playerSlots[gamepadSlot]._type) - 1);
                }
            } else if (gamepad.rightShoulder.wasPressedThisFrame || gamepad.rightTrigger.wasPressedThisFrame || gamepad.dpad.right.wasPressedThisFrame || gamepad.leftStick.right.wasPressedThisFrame)
            {
                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    _playerUISlots[gamepadSlot].Find("Info").Find("Right").GetComponent<BounceEffect>().Play();
                    ChangeModel(gamepadSlot, ((int)_playerSlots[gamepadSlot]._type) + 1);
                }
            }

        }
    }

    private void ChangeModel(int slot, int modelIndex)
    {

        if (_playerSlots[slot]._ready) return;

        var anchor = _playerTransforms[slot].Find("Anchor");

        GameObject.Destroy(_playerTransforms[slot].Find("Model").gameObject);
        int l = _playerModels.prefabs.Length;
        int model = (modelIndex % l + l) % l;
        var go = GameObject.Instantiate(_playerModels.prefabs[model], _playerTransforms[slot]);
        go.name = "Model";
        go.transform.position = anchor.position;
        go.transform.rotation = anchor.rotation;
        var rend = go.GetComponent<Renderer>();
        Assert.IsNotNull(rend);
        if(rend)
        {
            rend.material.SetColor("_BaseColor", _playerColors[slot]);
        }
        _playerSlots[slot]._type = (PlayerType) model;
    }

    private void PlayerJoin(Gamepad gamepad)
    {
        _gamepadSlots.Add(gamepad, _firstEmpty);
        _playerSlots[_firstEmpty]._active = true;
        _playerSlots[_firstEmpty]._gamepad = gamepad;
        _playerUISlots[_firstEmpty].Find("Info").gameObject.SetActive(true);
        _playerUISlots[_firstEmpty].Find("JoinText").gameObject.SetActive(false);
        while (_playerSlots[_firstEmpty]._active && _firstEmpty < _playerUISlots.Length) _firstEmpty++;
        /*if (_firstEmpty < _playerUISlots.Length)
        {
            _playerUISlots[_firstEmpty].Find("JoinText").gameObject.SetActive(true);
        }*/
    }

    private void PlayerReady(int slot)
    {
        if (!_playerSlots[slot]._ready)
        {
            _playerSlots[slot]._ready = true;
            _playerUISlots[slot].Find("Ready").gameObject.SetActive(true);
            _playerUISlots[slot].Find("Ready").GetComponent<BounceEffect>()._callBack.AddListener(OnReady);
            _playerUISlots[slot].Find("Ready").GetComponent<BounceEffect>().Play();
        }
    }

    private void OnReady()
    {
        bool allReady = true;
        foreach (var playerSlot in _playerSlots)
        {
            allReady = allReady && (playerSlot._ready || !playerSlot._active);
        }

        if (allReady)
        {
            _startTransform.gameObject.SetActive(true);
            _startTransform.GetComponent<BounceEffect>().Play();
        }
    }

    private void PlayerLeave(int slot)
    {
        if(_playerSlots[slot]._ready)
        {
            _playerUISlots[slot].Find("Ready").gameObject.SetActive(false);
            _playerSlots[slot]._ready = false;

            if(_startTransform.gameObject.activeSelf)
            {
                _startTransform.gameObject.SetActive(false);
            }
        }
        else
        {
            _playerSlots[slot]._active = false;
            _gamepadSlots.Remove(_playerSlots[slot]._gamepad);
            _playerSlots[slot]._gamepad = null;
            _playerUISlots[slot].Find("Info").gameObject.SetActive(false);
            _playerUISlots[slot].Find("JoinText").gameObject.SetActive(true);
            if (slot < _firstEmpty) _firstEmpty = slot;
        }

    }


}
