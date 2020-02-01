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

        [SerializeField] private Transform _objectPosition;

        [SerializeField] private Renderer _renderer;

        private C_Object _currentObject;
        private C_RobotArm _currentRobotArm;
        private C_RobotPart _currentRobotPart;

        public C_Object CurrentObject {
            get {
                return _currentObject;
            }
            set {
                if(!_currentObject || !value)
                {
                    _currentObject = value;
                    if(_currentObject)
                    {
                        _renderer.materials[1].SetColor("_BaseColor", new Color(1.0f, 0.15f, 0));
                    }
                }
            }
        }

        private void OnEnable()
        {
            _renderer = GetComponentInChildren<Renderer>();

            Assert.IsNotNull(_renderer);
        }

        public bool CanTakeObject()
        {
            if(_currentObject == null)
            {
                return false;
            }

            if (_currentRobotArm != null && _currentRobotArm.Progress < 1f)
            {
                return false;
            }

            if (_currentRobotPart != null && _currentRobotPart.Progress < _currentRobotPart.Hardness)
            {
                return false;
            }

            return true;
        }

        public C_Object TakeObject()
        {
            _currentRobotArm = null;
            _currentRobotPart = null;

            _renderer.materials[1].SetColor("_BaseColor", new Color(.04f, .04f, .04f));

            return _currentObject;
        }

        public void OnRepair()
        {
            if (_currentRobotPart == null)
            {
                return;
            }

            _currentRobotPart.Progress++;
            if(_currentRobotPart.Progress == _currentRobotPart.Hardness)
            {
                _renderer.materials[1].SetColor("_BaseColor", new Color(.04f, 1f, 0));
            }
        }

        public void UpdateAngle(float angle)
        {
            if (_currentRobotArm == null)
            {
                return;
            }

            float angleDelta = angle - _lastAngle;
            // Wrap angle diff
            if (angleDelta > Mathf.PI) angleDelta -= 2 * Mathf.PI;
            if (angleDelta < -Mathf.PI) angleDelta += 2 * Mathf.PI;

            if (Mathf.Abs(angleDelta) < MAX_ANGLE_DIFF)
            {
                if (_currentObject.ObjectType == ObjectType.LEFT_ARM)
                {
                    _currentRobotArm.Progress += Mathf.Max(0, angleDelta) / (2 * Mathf.PI * _currentRobotArm.Hardness);
                }
                else
                {
                    _currentRobotArm.Progress += Mathf.Max(0, -angleDelta) / (2 * Mathf.PI * _currentRobotArm.Hardness);
                }
            }

            if (_currentRobotArm.Progress == 1.0f)
            {
                _renderer.materials[1].SetColor("_BaseColor", new Color(.04f, 1f, 0));
            }

            _lastAngle = angle;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (_currentObject != null)
            {
                return;
            }

            var obj = collision.collider.GetComponent<C_Object>();

            if (obj == null || obj.ObjectType != _workbenchType)
            {
                return;
            }

            var robotArm = collision.collider.GetComponent<C_RobotArm>();
            var robotPart = collision.collider.GetComponent<C_RobotPart>();

            if (robotArm == null && robotPart == null)
            {
                return;
            }

            _currentObject = obj;

            _renderer.materials[1].SetColor("_BaseColor", new Color(1.0f, 0.15f, 0));

            if (robotArm != null)
            {
                _currentRobotArm = robotArm;
            }
            else if (robotPart != null)
            {
                _currentRobotPart = robotPart;
            }

            obj.transform.SetParent(transform);
            obj.transform.localPosition = _objectPosition.localPosition;
            obj.transform.localRotation = _objectPosition.localRotation;
            obj.EnterWorkbench();

        }
    }
}
