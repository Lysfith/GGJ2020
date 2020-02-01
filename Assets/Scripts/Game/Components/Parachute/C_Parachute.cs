using Assets.Scripts.Game.Components.Box;
using Assets.Scripts.Game.Components.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Parachute
{
    public class C_Parachute : MonoBehaviour
    {
        [SerializeField] private GameObject _boxRoot;

        private void OnEnable()
        {
            Assert.IsNotNull(_boxRoot);
        }

        private void Explosion()
        {
            for (var i = 0; i < _boxRoot.transform.childCount; i++)
            {
                var boxTransform = _boxRoot.transform.GetChild(i);
                var pos = boxTransform.GetComponent<C_GridPosition>();

                if (pos.X == 3)
                {
                    var box = boxTransform.GetComponent<C_Box>();
                    box.transform.SetParent(null);
                    box.EnablePhysic();
                    box.ApplyForce(Vector3.right * 500);
                }
            }
        }

        private void End()
        {
            Destroy(gameObject);
        }
    }
}
