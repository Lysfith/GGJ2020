using Assets.Scripts.Game.Components.Bots;
using Assets.Scripts.Game.Components.Characters;
using Assets.Scripts.Game.ScriptableObjects.Bots;
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
        [Header("Prefabs")]
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private GameObject _botPrefab;
        [SerializeField] private GameObject _parachutePrefab;
        [SerializeField] private GameObject _boxOpenedPrefab;
        [SerializeField] private GameObject _wastePrefab;
        [SerializeField] private SO_BotDefinitionList _botDefinitionList;

        [Header("References")]
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private Transform _charactersRoot;
        [SerializeField] private Transform _botRoot;
        [SerializeField] private Transform _boxRoot;
        [SerializeField] private Transform _objectRoot;

        [Header("Properties")]
        [SerializeField] private float _timeBetweenBotAndParachute;
        [SerializeField] private SO_BotDefinition _currentBotDefinition;
        [SerializeField] private List<GameObject> _boxes;


        private void OnEnable()
        {
#if UNITY_EDITOR
            foreach (var gamepad in Gamepad.all)
            {
                var randSpawn = UnityEngine.Random.Range(0, _spawnPositions.Count);
                SpawnCharacter(gamepad, _characterPrefab, _spawnPositions.ElementAt(randSpawn).position);
            }
#endif

           
        }

        public void StartGame()
        {
            SpawnBot();
        }


        private void SpawnCharacter(Gamepad gamepad, GameObject prefab, Vector3 position)
        {
            var go = Instantiate(prefab, _charactersRoot);
            go.transform.position = position;

            var character = go.GetComponent<C_Character>();
            character.Control.SetGamepad(gamepad);
        }

        private IEnumerator SpawnParachute()
        {
            yield return new WaitForSeconds(_timeBetweenBotAndParachute);

            var go = Instantiate(_parachutePrefab, _boxRoot);
            go.transform.localPosition = Vector3.zero;

            _boxes = new List<GameObject>();
            _boxes.AddRange(_currentBotDefinition.BotParts);
            for(int i = 0; i < 16; i++)
            {
                _boxes.Add(_wastePrefab);
            }
        }

        private void SpawnBot()
        {
            var randBot = UnityEngine.Random.Range(0, _botDefinitionList.Bots.Count);
            _currentBotDefinition = _botDefinitionList.Bots.ElementAt(randBot);

            var go = Instantiate(_currentBotDefinition.Bot, _botRoot);
            go.transform.localPosition = Vector3.zero;

            var bot = go.GetComponent<C_Bot>();
            bot.OnBotComplete += (s, e) =>
            {
                SpawnBot();
            };

            StartCoroutine(SpawnParachute());
        }

        public void SpawnBoxObject(Vector3 position)
        {
            if(_boxes.Count == 0)
            {
                return;
            }

            var box = Instantiate(_boxOpenedPrefab, _objectRoot);
            box.transform.position = position;

            var randPart = UnityEngine.Random.Range(0, _boxes.Count);
            var prefabPart = _boxes.ElementAt(randPart);
            var part = Instantiate(prefabPart, _objectRoot);
            part.transform.position = position + Vector3.up;

            _boxes.RemoveAt(randPart);
        }
    }
}
