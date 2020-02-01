using Assets.Scripts.Game.Components.Characters;
using Assets.Scripts.Game.ScriptableObjects.Objects;
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
        [SerializeField] private SO_ObjectList _objectListPrefab;

        [Header("References")]
        [SerializeField] private Transform _objectsRoot;

        [Header("Properties")]
        [SerializeField] private float _nbObjects;
        [SerializeField] private float _radius;

        private void OnEnable()
        {

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                //for(int i = 0; i < _nbObjects; i++)
                //{
                //    var position = transform.position + UnityEngine.Random.onUnitSphere * 5;
                //    SpawnBox(position);
                //}

                SpawnObjectList();
            }
        }

        public void SpawnObjectList()
        {
            foreach (var prefab in _objectListPrefab.Objects)
            {
                var position = transform.position + UnityEngine.Random.onUnitSphere * _radius;
                var go = Instantiate(prefab, _objectsRoot);
                go.transform.position = position;
            }
        }


        private void SpawnBox(Vector3 position)
        {
            var go = Instantiate(_boxPrefab, _objectsRoot);
            go.transform.position = position;
        }
    }
}
