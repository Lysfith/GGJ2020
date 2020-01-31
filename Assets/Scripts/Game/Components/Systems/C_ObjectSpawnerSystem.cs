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
    public class C_ObjectSpawnerSystem : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _boxPrefab;

        [Header("References")]
        [SerializeField] private Transform _objectsRoot;

        [Header("Properties")]
        [SerializeField] private float _nbObjects;

        private void OnEnable()
        {

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                for(int i = 0; i < _nbObjects; i++)
                {
                    var position = transform.position + UnityEngine.Random.onUnitSphere * 5;
                    SpawnBox(position);
                }
            }
        }


        private void SpawnBox(Vector3 position)
        {
            var go = Instantiate(_boxPrefab, _objectsRoot);
            go.transform.position = position;
        }
    }
}
