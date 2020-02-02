using Assets.Scripts.Game.Components.Bots;
using Assets.Scripts.Game.Components.Characters;
using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.ScriptableObjects.Bots;
using Assets.Scripts.Game.ScriptableObjects.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.Components.Systems
{
    public class C_LevelSystem : MonoBehaviour
    {
        private static C_LevelSystem _instance;

        public static C_LevelSystem Instance => _instance;


        [Header("Prefabs")]
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private GameObject _parachutePrefab;
        [SerializeField] private GameObject _boxOpenedPrefab;
        [SerializeField] private GameObject _wastePrefab;
        [SerializeField] private SO_BotDefinitionList _botDefinitionList;
        [SerializeField] private SO_ObjectList _boxList;

        [Header("References")]
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private Transform _charactersRoot;
        [SerializeField] private Transform _botRoot;
        [SerializeField] private Transform _boxRoot;
        [SerializeField] private Transform _objectRoot;
        [SerializeField] private List<C_Character> _characters;

        [Header("Properties")]
        [SerializeField] private float _timeBetweenBotAndParachute;
        [SerializeField] private SO_BotDefinition _currentBotDefinition;
        [SerializeField] private List<GameObject> _boxes;
        [SerializeField] private bool _isEnabled;
        [SerializeField] private bool _isRecyclingStep;


        private void OnEnable()
        {
            _instance = this;

            _characters = new List<C_Character>();
#if UNITY_EDITOR
            foreach (var gamepad in Gamepad.all)
            {
                var randSpawn = UnityEngine.Random.Range(0, _spawnPositions.Count);
                SpawnCharacter(gamepad, _characterPrefab, _spawnPositions.ElementAt(randSpawn).position);
            }
#endif
        }

        private void Update()
        {
            if (_isEnabled && _isRecyclingStep)
            {
                if (!_boxList.Objects.Any())
                {
                    _isRecyclingStep = false;
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

            foreach (var character in _characters)
            {
                character.Control.Disable();
                character.Mover.Disable();
            }
        }


        private void SpawnCharacter(Gamepad gamepad, GameObject prefab, Vector3 position)
        {
            var go = Instantiate(prefab, _charactersRoot);
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
            _boxes.AddRange(_currentBotDefinition.BotParts);
            for (int i = 0; i < 16; i++)
            {
                _boxes.Add(_wastePrefab);
            }
        }

        private void SpawnBot()
        {
            if (!_isEnabled)
            {
                return;
            }

            var randBot = UnityEngine.Random.Range(0, _botDefinitionList.Bots.Count);
            _currentBotDefinition = _botDefinitionList.Bots.ElementAt(randBot);

            var go = Instantiate(_currentBotDefinition.Bot, _botRoot);
            go.transform.localPosition = Vector3.zero;

            var bot = go.GetComponent<C_Bot>();

            bot.OnBotComplete += (s, e) =>
            {
                var layerWaste = LayerMask.NameToLayer("Waste");
                foreach(var box in _boxList.Objects)
                {
                    box.gameObject.layer = layerWaste;
                }

                _isRecyclingStep = true;
            };

            StartCoroutine(SpawnParachute());
        }

        public void SpawnBoxObject(GameObject boxClosed)
        {
            if (_boxes.Count == 0)
            {
                return;
            }

            var box = Instantiate(_boxOpenedPrefab, _objectRoot);
            box.transform.position = boxClosed.transform.position;

            var randPart = UnityEngine.Random.Range(0, _boxes.Count);
            var prefabPart = _boxes.ElementAt(randPart);
            var part = Instantiate(prefabPart, _objectRoot);
            part.transform.position = boxClosed.transform.position + Vector3.up;

            _boxes.RemoveAt(randPart);

            RemoveObjectFromList(boxClosed.GetComponent<C_Object>());
            AddObjectToList(box.GetComponent<C_Object>());
            AddObjectToList(part.GetComponent<C_Object>());

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
