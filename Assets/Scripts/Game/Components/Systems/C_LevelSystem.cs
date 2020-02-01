using Assets.Scripts.Game.Components.Bots;
using Assets.Scripts.Game.Components.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.Components.Systems
{
    public class C_LevelSystem : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private GameObject _botPrefab;

        [Header("References")]
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private Transform _charactersRoot;
        [SerializeField] private Transform _botRoot;
        [SerializeField] private C_ObjectSpawnerSystem _objectSpawnerSystem;

        private void OnEnable()
        {
            _objectSpawnerSystem = FindObjectOfType<C_ObjectSpawnerSystem>();

#if UNITY_EDITOR
            foreach (var gamepad in Gamepad.all)
            {
                var randSpawn = UnityEngine.Random.Range(0, _spawnPositions.Count);
                SpawnCharacter(gamepad, _characterPrefab, _spawnPositions.ElementAt(randSpawn).position);
            }
#endif

            SpawnBot(_botPrefab);
            
        }


        private void SpawnCharacter(Gamepad gamepad, GameObject prefab, Vector3 position)
        {
            var go = Instantiate(prefab, _charactersRoot);
            go.transform.position = position;

            var character = go.GetComponent<C_Character>();
            character.Control.SetGamepad(gamepad);
        }

        private void SpawnBot(GameObject prefab)
        {
            var go = Instantiate(prefab, _botRoot);
            go.transform.localPosition = Vector3.zero;

            var bot = go.GetComponent<C_Bot>();
            bot.OnBotComplete += (s, e) =>
            {
                SpawnBot(_botPrefab);
            };

            _objectSpawnerSystem.SpawnObjectList();
        }
    }
}
