using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Game.ScriptableObjects.Characters;
using Assets.Scripts;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.Global.Components;
using UnityEngine.AI;

public class SelectionManager : MonoBehaviour
{

    private const float BACK_PRESS_TIME = 2f;

    public RectTransform[] _playerUISlots;
    public PlayerSlot[] _playerSlots;
    public PlayerModels _playerModels;

    public Transform[] _playerTransforms;
    public Transform _startTransform;

    public Image _backImage;

    public Dictionary<Gamepad, int> _gamepadSlots;
    public int _firstEmpty;

    public UnityEvent OnStart;
    public UnityEvent OnBack;

    public S_SceneFaderSystem _sceneFader;

    private float _backTimePressed;
    private bool _readyToStart;
    private bool _locked;

    private void Start()
    {
        _gamepadSlots = new Dictionary<Gamepad, int>();
        _readyToStart = false;
        _locked = false;
        int index = 0;
        foreach (var playerSlot in _playerSlots)
        {
            playerSlot._active = false;
            playerSlot._ready = false;
            playerSlot._gamepad = null;

            int l = _playerModels.prefabs.Length;
            int modelIndex = (index % l + l) % l;
            var model = GameObject.Instantiate(_playerModels.prefabs[index], _playerTransforms[index]);
            var anchor = _playerTransforms[index].Find("Anchor");
            model.transform.position = anchor.position;
            model.transform.rotation = anchor.rotation;
            model.transform.localScale = anchor.localScale;
            var rend = model.transform.Find("Graphic").GetChild(0).Find("Body").GetComponent<Renderer>();
            Assert.IsNotNull(rend);
            if (rend)
            {
                rend.materials[1].SetColor("_BaseColor", playerSlot._color);
            }
            playerSlot._type = (PlayerType)index;
            index++;

        }
    }

    private void Update()
    {

        bool backPressed = false;

        if (_locked) return;

        foreach (var gamepad in Gamepad.all)
        {

            if(gamepad.buttonEast.isPressed)
            {
                if(!_gamepadSlots.ContainsKey(gamepad))
                {
                    backPressed = true;
                }
            }

            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                if(_readyToStart)
                {
                    _locked = true;
                    Destroy(GameObject.FindGameObjectWithTag("Music"));
                    OnStart?.Invoke();
                    return;
                }

                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    PlayerReady(gamepadSlot);
                }
            }
            else if (gamepad.buttonEast.wasPressedThisFrame)
            {
                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    PlayerLeave(gamepadSlot);
                }
            }
            else if (gamepad.startButton.wasPressedThisFrame && _firstEmpty < _playerUISlots.Length) {
                if(!_gamepadSlots.ContainsKey(gamepad))
                {
                    PlayerJoin(gamepad);
                }

            }
            else if (gamepad.leftShoulder.wasPressedThisFrame || gamepad.leftTrigger.wasPressedThisFrame || gamepad.dpad.left.wasPressedThisFrame || gamepad.leftStick.left.wasPressedThisFrame)
            {
                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    _playerUISlots[gamepadSlot].Find("Info").Find("Left").GetComponent<BounceEffect>().Play();
                    ChangeModel(gamepadSlot, ((int) _playerSlots[gamepadSlot]._type) - 1);
                }
            }
            else if (gamepad.rightShoulder.wasPressedThisFrame || gamepad.rightTrigger.wasPressedThisFrame || gamepad.dpad.right.wasPressedThisFrame || gamepad.leftStick.right.wasPressedThisFrame)
            {
                if (_gamepadSlots.TryGetValue(gamepad, out int gamepadSlot))
                {
                    _playerUISlots[gamepadSlot].Find("Info").Find("Right").GetComponent<BounceEffect>().Play();
                    ChangeModel(gamepadSlot, ((int)_playerSlots[gamepadSlot]._type) + 1);
                }
            }

        }

        if(backPressed)
        {
            _backTimePressed += Time.deltaTime;
            if(_backTimePressed > BACK_PRESS_TIME)
            {
                Back();
            }
        } else
        {
            _backTimePressed = 0.0f;
        }

        _backImage.fillAmount = _backTimePressed / BACK_PRESS_TIME;
    }

    public void ReadyToStart()
    {
        _readyToStart = true;
    }

    private void Back()
    {
        OnBack?.Invoke();
        _sceneFader.FadeOut("MainMenu", .2f);
    }

    private void ChangeModel(int slot, int modelIndex)
    {

        if (_playerSlots[slot]._ready) return;

        SoundManager.PlaySound(SoundList.Sound.select);

        var anchor = _playerTransforms[slot].Find("Anchor");

        GameObject.Destroy(_playerTransforms[slot].GetChild(1).gameObject);
        int l = _playerModels.prefabs.Length;
        int model = (modelIndex % l + l) % l;
        var go = GameObject.Instantiate(_playerModels.prefabs[model], _playerTransforms[slot]);
        go.name = "Model";
        go.transform.position = anchor.position;
        go.transform.rotation = anchor.rotation;
        go.transform.localScale = anchor.localScale;
        var rend = go.transform.Find("Graphic").GetChild(0).Find("Body").GetComponent<Renderer>();
        Assert.IsNotNull(rend);
        if(rend)
        {
            rend.materials[1].SetColor("_BaseColor", _playerSlots[slot]._color);
        }
        _playerSlots[slot]._type = (PlayerType) model;
    }

    private void PlayerJoin(Gamepad gamepad)
    {

        SoundManager.PlaySound(SoundList.Sound.login);
        _gamepadSlots.Add(gamepad, _firstEmpty);
        _playerSlots[_firstEmpty]._active = true;
        _playerSlots[_firstEmpty]._gamepad = gamepad;
        _playerUISlots[_firstEmpty].Find("Info").gameObject.SetActive(true);
        _playerUISlots[_firstEmpty].Find("JoinText").gameObject.SetActive(false);
        while (_playerSlots[_firstEmpty]._active && _firstEmpty < _playerUISlots.Length) _firstEmpty++;

        if (_startTransform.gameObject.activeSelf)
        {
            _startTransform.gameObject.SetActive(false);
            foreach (var img in _startTransform.GetComponentsInChildren<Image>())
            {
                img.fillAmount = 0.0f;
            }
            _startTransform.Find("GO").gameObject.SetActive(false);
            _readyToStart = false;
        }
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
            SoundManager.PlaySound(SoundList.Sound.ready);
            _playerUISlots[slot].Find("Info").Find("Left").gameObject.SetActive(false);
            _playerUISlots[slot].Find("Info").Find("Right").gameObject.SetActive(false);
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
            _startTransform.GetComponent<FillEffect>().Play();
        }
    }

    private void PlayerLeave(int slot)
    {
        if(_playerSlots[slot]._ready)
        {
            _playerUISlots[slot].Find("Ready").gameObject.SetActive(false);
            _playerUISlots[slot].Find("Info").Find("Left").gameObject.SetActive(true);
            _playerUISlots[slot].Find("Info").Find("Right").gameObject.SetActive(true);
            _playerSlots[slot]._ready = false;

            if(_startTransform.gameObject.activeSelf)
            {
                _startTransform.gameObject.SetActive(false);
                foreach (var img in _startTransform.GetComponentsInChildren<Image>())
                {
                    img.fillAmount = 0.0f;
                }
                _startTransform.Find("GO").gameObject.SetActive(false);
                _readyToStart = false;
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
