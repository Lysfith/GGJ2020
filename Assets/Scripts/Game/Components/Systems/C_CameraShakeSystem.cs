using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Systems
{
    public class C_CameraShakeSystem : MonoBehaviour
    {
        private static C_CameraShakeSystem _instance;

        public static C_CameraShakeSystem Instance => _instance;


        [SerializeField] Transform _camera;
        [SerializeField] Vector3 _shakeForce;

        private void OnEnable()
        {
            _instance = this;

            Assert.IsNotNull(_camera);
        }

        public void Shake()
        {
            _camera.DOShakePosition(10f, _shakeForce, 5, 10f, false, true);
        }
    }
}
