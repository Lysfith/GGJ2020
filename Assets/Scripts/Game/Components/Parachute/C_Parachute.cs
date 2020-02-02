using Assets.Scripts.Game.Components.Box;
using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.Components.Others;
using Assets.Scripts.Game.Components.Systems;
using Assets.Scripts.Game.ScriptableObjects.Objects;
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
            SoundManager.PlaySound(SoundList.Sound.parachute);
        }

        private void Explosion()
        {
            C_CameraShakeSystem.Instance.Shake();

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

                    C_LevelSystem.Instance.AddObjectToList(boxTransform.GetComponent<C_Object>());
                    SoundManager.PlaySound(SoundList.Sound.droprobot, pitch: 0.9f, delay:2);
                }
            }
        }

        private void End()
        {
            Destroy(gameObject);
        }
    }
}
