using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Components.Objects;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Repairing
{
    public class C_Workbench : MonoBehaviour
    {

        private const float MAX_ANGLE_DIFF = Mathf.PI / 4;

        private float _lastAngle;
        [SerializeField] private ObjectType _workbenchType;
        [SerializeField] private C_Object _currentObject;
        [SerializeField] private Transform _anchor;

        public C_Object CurrentObject {
            get {
                return _currentObject;
            }
            set {
                if(!_currentObject || !value)
                {
                    _currentObject = value;
                }
            }
        }

        public ObjectType ObjectType {
            get {
                return _workbenchType;
            }
        }

        private void Start()
        {
            Assert.IsFalse(transform.childCount > 0);
            _anchor = transform.GetChild(0);
        }

        public void OnRepair()
        {
            if(_currentObject && _currentObject is C_RobotPart)
            {
                if (_currentObject.ObjectType == _workbenchType)
                {
                    ((C_RobotPart)_currentObject).Progress++;
                }
            }
        }

        public void UpdateAngle(float angle)
        {
            if (_currentObject && _currentObject is C_RobotArm)
            {
                float angleDelta = angle - _lastAngle;
                // Wrap angle diff
                if (angleDelta > Mathf.PI) angleDelta -= 2 * Mathf.PI;
                if (angleDelta < -Mathf.PI) angleDelta += 2 * Mathf.PI;

                if (Mathf.Abs(angleDelta) < MAX_ANGLE_DIFF)
                {
                    if (_currentObject.ObjectType == ObjectType.LEFT_ARM)
                    {
                        ((C_RobotArm)_currentObject).Progress += Mathf.Max(0, angleDelta) / (2 * Mathf.PI * ((C_RobotArm)_currentObject).Hardness);
                    }
                    else
                    {
                        ((C_RobotArm)_currentObject).Progress += Mathf.Max(0, -angleDelta) / (2 * Mathf.PI * ((C_RobotArm)_currentObject).Hardness);
                    }
                }
                _lastAngle = angle;
            }

        }
    }
}
