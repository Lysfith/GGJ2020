﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Components.Objects;
using UnityEngine.Assertions;
using static SoundList;

namespace Assets.Scripts.Game.Components.Repairing
{
    public class C_Workbench : MonoBehaviour
    {

        private const float MAX_ANGLE_DIFF = Mathf.PI / 4;
        private const float LED_INTENSITY = 2f;

        private float _lastAngle;
        [SerializeField] private ObjectType _workbenchType;

        [SerializeField] private Transform _objectPosition;

        [SerializeField] private Renderer _renderer;

        private C_Object _currentObject;
        private C_RobotArm _currentRobotArm;
        private C_RobotPart _currentRobotPart;

        public ObjectType WorkbenchType => _workbenchType;

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
                        ChangeLEDColor(new Color(1.0f, 0.15f, 0f));
                    }
                }
            }
        }

        private void OnEnable()
        {
            _renderer = GetComponentInChildren<Renderer>();

            Assert.IsNotNull(_renderer);
        }

        private void Update()
        {
            if(_currentObject == null)
            {
                return;
            }

            _currentObject.transform.localPosition = _currentObject.GetHandPosition();
        }

        public bool CanTakeObject()
        {
            if(_currentObject == null)
            {
                return false;
            }

            //if (_currentRobotArm != null && _currentRobotArm.Progress < 1f)
            //{
            //    return false;
            //}

            //if (_currentRobotPart != null && _currentRobotPart.Progress < _currentRobotPart.Hardness)
            //{
            //    return false;
            //}

            return true;
        }

        public C_Object TakeObject()
        {
            _currentRobotArm = null;
            _currentRobotPart = null;

            var obj = _currentObject;
            _currentObject = null;

            ChangeLEDColor(new Color(.04f, .04f, .04f));

            return obj;
        }

        private void ChangeLEDColor(Color color)
        {
            _renderer.materials[1].SetVector("_EmissionColor", color * LED_INTENSITY);
        }

        public void OnRepair(GameObject player)
        {
            if (_currentRobotPart == null)
            {
                return;
            }
            _currentRobotPart.Progress++;
            if(_currentRobotPart.Progress == _currentRobotPart.Hardness)
            {
                ChangeLEDColor(new Color(.04f, 1f, 0));
                SoundManager.PlaySound(Sound.workbenchok,player,true);
            }
        }

        public void UpdateAngle(float angle, GameObject player)
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
                ChangeLEDColor(new Color(.04f, 1f, 0));
                SoundManager.PlaySound(Sound.workbenchok, player, true);
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

            ChangeLEDColor(new Color(1.0f, 0.15f, 0));

            if (robotArm != null)
            {
                _currentRobotArm = robotArm;

                if (_currentRobotArm.Progress >= 1f)
                {
                    ChangeLEDColor(new Color(.04f, 1f, 0));
                }
            }
            else if (robotPart != null)
            {
                _currentRobotPart = robotPart;

                if (_currentRobotPart.Progress >= _currentRobotPart.Hardness)
                {
                    ChangeLEDColor(new Color(.04f, 1f, 0));
                }
            }

            obj.transform.SetParent(_objectPosition);
            obj.transform.localPosition = - obj.OffsetHand.localPosition;
            obj.transform.localRotation = Quaternion.identity;
            obj.EnterWorkbench();

        }
        public ObjectType GetWorkbenchType()
        {
            return _workbenchType;
        }
    }
}
