using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Others
{
    public class C_SpawnObjectOnGrid : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private int _width;
        [SerializeField] private int _depth;
        [SerializeField] private int _height;
        [SerializeField] private float _size;
        [SerializeField] private GameObject _prefab;

        private void OnEnable()
        {
            SpawnGrid();
        }

        private void SpawnGrid()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        SpawnObject(x, y, z, _prefab);
                    }
                }
            }
        }

        private void SpawnObject(int x, int y, int z, GameObject prefab)
        {
            var go = Instantiate(prefab);
            go.transform.SetParent(_root);
            go.transform.localPosition = new Vector3(x * _size, y * _size, z * _size);

            var rand = UnityEngine.Random.Range(0, 4);
            go.transform.localRotation = Quaternion.Euler(0, 90 * rand, 0);

            var gridPos = go.GetComponent<C_GridPosition>();
            gridPos.X = x;
            gridPos.Y = y;
            gridPos.Z = z;
        }
    }
}
