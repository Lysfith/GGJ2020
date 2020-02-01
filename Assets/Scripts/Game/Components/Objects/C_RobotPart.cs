using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Objects
{

    public class C_RobotPart : C_Object
    {
        [Tooltip("Number of smashes to repair")]
        [SerializeField] private int _hardness = 10;
        [SerializeField] private int _progress = 0;

        private Vector3 _referenceScale;

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
                    transform.localScale = _referenceScale * (1 +  .5f * _progress / _hardness);
                }

            }
        }
        
        private void Start()
        {
            _referenceScale = transform.localScale;
        }

    }
}
