using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Components.Objects;

namespace Assets.Scripts.Game.Components.Repairing
{
    public class C_Workbench : MonoBehaviour
    {

        private const float MAX_ANGLE_DIFF = Mathf.PI / 4;

        private float _lastAngle;
        [SerializeField] private ObjectType _workbenchType;

        private C_Object _currentObject;

        public C_Object CurrentObject {
            get {
                return _currentObject;
            }
            set {
                if(!_currentObject)
                {
                    _currentObject = value;
                }
            }
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
