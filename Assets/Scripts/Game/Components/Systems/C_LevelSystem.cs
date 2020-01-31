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

        [Header("References")]
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private Transform _charactersRoot;

        private void OnEnable()
        {

            var randSpawn = UnityEngine.Random.Range(0, _spawnPositions.Count);

#if UNITY_EDITOR
            foreach (var gamepad in Gamepad.all)
            {
                SpawnCharacter(gamepad, _characterPrefab, _spawnPositions.ElementAt(randSpawn).position);
            }
#endif

        }


        private void SpawnCharacter(Gamepad gamepad, GameObject prefab, Vector3 position)
        {
            var go = Instantiate(prefab, _charactersRoot);
            go.transform.position = position;

            var character = go.GetComponent<C_Character>();
            character.Control.SetGamepad(gamepad);
        }
    }
}
