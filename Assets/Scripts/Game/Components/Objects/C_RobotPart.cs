using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Objects
{

    public class C_RobotPart : MonoBehaviour
    {
        [Tooltip("Number of smashes to repair")]
        [SerializeField] private int _hardness = 10;
        [SerializeField] private int _progress = 0;

        public float Hardness
        {
            get
            {
                return _hardness;
            }
        }

        public int Progress {
            get {
                return _progress;
            }
            set {
                _progress = value;
                if(_progress > _hardness)
                {
                    _progress = _hardness;
                } else
                {
                    transform.localScale += Vector3.one * .5f / _hardness;
                }

            }
        }

    }
}
