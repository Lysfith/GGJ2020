using Assets.Scripts.Game.Components.Bots;
using Assets.Scripts.Game.Components.Characters;
using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.ScriptableObjects;
using Assets.Scripts.Game.ScriptableObjects.Bots;
using Assets.Scripts.Game.ScriptableObjects.Characters;
using Assets.Scripts.Game.ScriptableObjects.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.Components.Systems
{
    public class C_LevelSystem : MonoBehaviour
    {
        private static C_LevelSystem _instance;

        public static C_LevelSystem Instance => _instance;


        [Header("Prefabs")]
        [SerializeField] private PlayerModels _playerModels;
        [SerializeField] private GameObject _parachutePrefab;
        [SerializeField] private GameObject _boxOpenedPrefab;
        [SerializeField] private GameObject _wastePrefab;
        [SerializeField] private GameObject _botBasePrefab;
        [SerializeField] private SO_ObjectList _boxList;
        [SerializeField] private SO_ObjectList _headList;
        [SerializeField] private SO_ObjectList _chestList;
        [SerializeField] private SO_ObjectList _leftArmList;
        [SerializeField] private SO_ObjectList _rightArmList;
        [SerializeField] private SO_ObjectList _legList;
        [SerializeField] private SO_ColorList _colorList;
        [SerializeField] private Color _currentColor;
        [SerializeField] private List<GameObject> _partsInvalidPrefab;


        [Header("References")]
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private List<Transform> _endPositions;
        [SerializeField] private Transform _charactersRoot;
        [SerializeField] private Transform _botRoot;
        [SerializeField] private Transform _boxRoot;
        [SerializeField] private Transform _objectRoot;
        [SerializeField] private List<C_Character> _characters;
        [SerializeField] private PlayerSlot[] _playerSlots;
        [SerializeField] private GameObject _recyclingText;

        [Header("Properties")]
        [SerializeField] private float _timeBetweenBotAndParachute;
        [SerializeField] private List<GameObject> _boxes;
        [SerializeField] private bool _isEnabled;
        [SerializeField] private bool _isRecyclingStep;
        [SerializeField] private Material _botMaterialV1;
        [SerializeField] private Material _botMaterialV2;
        [SerializeField] private C_Bot _bot;


        private void OnEnable()
        {
            _instance = this;

            _characters = new List<C_Character>();
            _boxList.Objects.Clear();

            //if (!_playerSlots.Where(s => s._active).Any())
            //{
            //    for (int i = 0; i < Gamepad.all.Count; i++)
            //    {
            //        var gamepad = Gamepad.all[i];
            //        var randSpawn = UnityEngine.Random.Range(0, _spawnPositions.Count);
            //        SpawnCharacter(gamepad, i, _spawnPositions.ElementAt(randSpawn).position);
            //    }
            //}
            //else
            //{
                for (int i = 0; i < _playerSlots.Count(); i++)
                {
                    var slot = _playerSlots[i];
                    if (!slot._active)
                    {
                        continue;
                    }

                    SpawnCharacter(slot._gamepad, slot, _spawnPositions.ElementAt(i).position);
                }
            //}
        }

        private void Update()
        {
            if (_isEnabled && _isRecyclingStep)
            {
                if (!_boxList.Objects.Any())
                {
                    _isRecyclingStep = false;
                    _recyclingText.SetActive(false);
                    SpawnBot();
                }
            }
        }

        public void StartGame()
        {
            _isEnabled = true;
            SpawnBot();
        }

        public void StopGame()
        {
            _isEnabled = false;

            for (int i = 0; i < _characters.Count; i++)
            {
                var character = _characters[i];
                character.Control.Disable();
                character.Mover.Disable();
                character.Mover.MoveTo(_endPositions[i].position);
            }

            StartCoroutine(ShowParachutes());
        }

        private IEnumerator ShowParachutes()
        {
            yield return new WaitForSeconds(3);

            for (int i = 0; i < _characters.Count; i++)
            {
                var character = _characters[i];
                character.ShowParachute();
            }
        }


        private void SpawnCharacter(Gamepad gamepad, PlayerSlot slot, Vector3 position)
        {

            var go = Instantiate(_playerModels.prefabs[(int)slot._type], _charactersRoot);
            var renderer = go.transform.Find("Graphic").GetChild(0).Find("Body").GetComponent<Renderer>();
            renderer.materials[1].SetColor("_BaseColor", slot._color);
            go.transform.GetComponent<NavMeshAgent>().enabled = true;
            go.transform.position = position;

            var character = go.GetComponent<C_Character>();
            character.Control.SetGamepad(gamepad);

            _characters.Add(character);
        }

        private IEnumerator SpawnParachute()
        {
            yield return new WaitForSeconds(_timeBetweenBotAndParachute);

            var go = Instantiate(_parachutePrefab, _boxRoot);
            go.transform.localPosition = Vector3.zero;

            _boxes = new List<GameObject>();
            for (int i = 0; i < 12; i++)
            {
                _boxes.Add(GetWastePart());
            }

            _boxes.Add(GetGOFromTypeAndVersion(ObjectType.HEAD, _bot.Parts[ObjectType.HEAD]));
            _boxes.Add(GetGOFromTypeAndVersion(ObjectType.CHEST, _bot.Parts[ObjectType.CHEST]));
            _boxes.Add(GetGOFromTypeAndVersion(ObjectType.LEFT_ARM, _bot.Parts[ObjectType.LEFT_ARM]));
            _boxes.Add(GetGOFromTypeAndVersion(ObjectType.RIGHT_ARM, _bot.Parts[ObjectType.RIGHT_ARM]));
        }

        private GameObject GetWastePart()
        {
            var randType = (ObjectType)UnityEngine.Random.Range(0, 3);
            var version = _bot.Parts[randType] == PartVersion.V1 ? PartVersion.V2 : PartVersion.V1;

            var go = GetGOFromTypeAndVersion(randType, version);
            return go;
        }

        public GameObject GetGOFromTypeAndVersion(ObjectType type, PartVersion version)
        {
            var intVersion = (int)version;
            switch (type)
            {
                case ObjectType.HEAD: return _headList.Objects.ElementAt(intVersion).gameObject;
                case ObjectType.CHEST: return _chestList.Objects.ElementAt(intVersion).gameObject;
                case ObjectType.LEFT_ARM: return _leftArmList.Objects.ElementAt(intVersion).gameObject;
                case ObjectType.RIGHT_ARM: return _rightArmList.Objects.ElementAt(intVersion).gameObject;
            }

            return null;
        }

        public Color GetOtherColorRandom()
        {
            var rand = UnityEngine.Random.Range(0, _colorList.Colors.Count-2);

            return _colorList.Colors.Except(new List<Color>() { _currentColor }).ElementAt(rand);
        }

        public Color GetColorRandom()
        {
            var rand = UnityEngine.Random.Range(0, _colorList.Colors.Count - 1);

            return _colorList.Colors.ElementAt(rand);
        }

        private void SpawnBot()
        {
            if (!_isEnabled)
            {
                return;
            }

            var go = Instantiate(_botBasePrefab, _botRoot);
            go.transform.localPosition = Vector3.zero;

            var bot = go.GetComponent<C_Bot>();
            var partsValid = new Dictionary<ObjectType, PartVersion>()
            {
                { ObjectType.HEAD, (PartVersion)UnityEngine.Random.Range(0, 2) },
                { ObjectType.CHEST, (PartVersion)UnityEngine.Random.Range(0, 2) },
                { ObjectType.LEFT_ARM, (PartVersion)UnityEngine.Random.Range(0, 2) },
                { ObjectType.RIGHT_ARM, (PartVersion)UnityEngine.Random.Range(0, 2) },
                { ObjectType.LEG, (PartVersion)UnityEngine.Random.Range(0, 2) },
            };

            _currentColor = GetColorRandom();
            bot.Init(partsValid, _currentColor);
            _bot = bot;

            bot.OnBotComplete += (s, e) =>
            {
                var layerWaste = LayerMask.NameToLayer("Waste");
                foreach (var box in _boxList.Objects)
                {
                    box.gameObject.layer = layerWaste;
                }

                _isRecyclingStep = true;
                _recyclingText.SetActive(true);
            };

            StartCoroutine(SpawnParachute());
        }

        public void SpawnBoxObject(GameObject boxClosed)
        {
            if (_boxes.Count == 0)
            {
                return;
            }

            //var box = Instantiate(_boxOpenedPrefab, _objectRoot);
            //box.transform.position = boxClosed.transform.position;

            var randPart = UnityEngine.Random.Range(0, _boxes.Count);
            var prefabPart = _boxes.ElementAt(randPart);
            var part = Instantiate(prefabPart, _objectRoot);
            part.transform.position = boxClosed.transform.position + Vector3.up;

            var partObject = part.GetComponent<C_Object>();

            if(!_bot.Parts.ContainsKey(partObject.ObjectType))
            {
                Debug.Log(partObject.ObjectType);
            }

            var isValid = _bot.Parts[partObject.ObjectType] == partObject.Version;

            var renderer = part.GetComponentInChildren<Renderer>(true);

            //Material newMaterial = null;

            //if (partObject.Version == PartVersion.V1)
            //{
            //    newMaterial = new Material(_botMaterialV1);
            //}
            //else
            //{
            //    newMaterial = new Material(_botMaterialV2);
            //}

            renderer.materials[0].SetColor("Color_F5B15491", isValid ? _currentColor : GetOtherColorRandom());
            renderer.materials[0].SetFloat("FresnelOpacity_", isValid ? 1 : 0);

            

            //var renderers = part.GetComponentsInChildren<Renderer>(true);
            

            //foreach (var renderer in renderers)
            //{
            //    var material = renderer.materials.Where(i => i.name == "Color2").FirstOrDefault();

            //    if (material == null)
            //    {
            //        continue;
            //    }

            //    var index = Array.IndexOf(renderer.materials, material);
            //    renderer.materials[index] = newMaterial;
            //}

            _boxes.RemoveAt(randPart);

            RemoveObjectFromList(boxClosed.GetComponent<C_Object>());
            //AddObjectToList(box.GetComponent<C_Object>());
            AddObjectToList(partObject);

            if(_isRecyclingStep)
            {
                var layerWaste = LayerMask.NameToLayer("Waste");
                //box.layer = layerWaste;
                part.layer = layerWaste;
            }

            Destroy(boxClosed);
        }

        public void RemoveObjectFromList(C_Object obj)
        {
            if (_boxList.Objects.Contains(obj))
            {
                _boxList.Objects.Remove(obj);
            }
        }

        public void AddObjectToList(C_Object obj)
        {
            if (!_boxList.Objects.Contains(obj))
            {
                _boxList.Objects.Add(obj);
            }
        }
    }
}
