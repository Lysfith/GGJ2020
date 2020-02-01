using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Objects
{

    public class C_RobotArm : MonoBehaviour
    {
        [Tooltip("Number of circles to repair")]
        [SerializeField] private float _hardness = 10f;
        [SerializeField] private float _progress = 0.0f;

        public float Hardness {
            get {
                return _hardness;
            }
        }

        public float Progress {
            get {
                return _progress;
            }
            set {
                float diff = value - _progress;
                _progress = value;
                if (_progress > 1.0f)
                {
                    _progress = 1.0f;
                }
                else
                {
                    transform.localScale += Vector3.one * .5f * diff;
                }
            }
        }

    }
}
