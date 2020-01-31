using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.Components.Characters.Parts
{
    public class C_CharacterControl : C_CharacterPart
    {
        private const float JOYSTICK_DEAD_ZONE = 0.3f;

        [SerializeField] private C_Character _character;
        [SerializeField] private Gamepad _gamepad;

        private void OnEnable()
        {
            _character = GetComponent<C_Character>();

            Assert.IsNotNull(_character);
        }

        private void Update()
        {
            if (!_isEnabled)
            {
                return;
            }

            Input();
        }

        private void Input()
        {
            if (_gamepad == null)
            {
                return;
            }

            var leftX = _gamepad.leftStick.x.ReadValue();
            var leftY = _gamepad.leftStick.y.ReadValue();

            LeftJoystickEvent(leftX, leftY);

            var rightX = _gamepad.rightStick.x.ReadValue();
            var rightY = _gamepad.rightStick.y.ReadValue();

            RightJoystickEvent(rightX, rightY);

            if (_gamepad.aButton.wasPressedThisFrame)
            {
                ButtonAEvent();
            }

            if (_gamepad.bButton.wasPressedThisFrame)
            {
                ButtonBEvent();
            }

            if (_gamepad.xButton.wasPressedThisFrame)
            {
                ButtonXEvent();
            }

            if (_gamepad.yButton.wasPressedThisFrame)
            {
                ButtonYEvent();
            }

            if (_gamepad.leftShoulder.wasPressedThisFrame)
            {
                ButtonL1Event();
            }

            if (_gamepad.rightShoulder.wasPressedThisFrame)
            {
                ButtonR1Event();
            }

            if (_gamepad.leftTrigger.wasPressedThisFrame)
            {
                ButtonL2Event();
            }

            if (_gamepad.rightTrigger.wasPressedThisFrame)
            {
                ButtonR2Event();
            }
        }

        private void LeftJoystickEvent(float x, float y)
        {
            x = -JOYSTICK_DEAD_ZONE < x && x < JOYSTICK_DEAD_ZONE ? 0 : x;
            y = -JOYSTICK_DEAD_ZONE < y && y < JOYSTICK_DEAD_ZONE ? 0 : y;

            _character.Stats.Direction.x = x;
            _character.Stats.Direction.y = y;
        }

        private void RightJoystickEvent(float x, float y)
        {
            x = -JOYSTICK_DEAD_ZONE < x && x < JOYSTICK_DEAD_ZONE ? 0 : x;
            y = -JOYSTICK_DEAD_ZONE < y && y < JOYSTICK_DEAD_ZONE ? 0 : y;


        }

        private void ButtonAEvent()
        {

        }

        private void ButtonXEvent()
        {

        }

        private void ButtonBEvent()
        {

        }

        private void ButtonYEvent()
        {

        }

        private void ButtonL1Event()
        {
            //Animator.SetInteger("Stance", 0);
            //Animator.SetTrigger("ChangeStance");
        }

        private void ButtonR1Event()
        {
            //Animator.SetInteger("Stance", 1);
            //Animator.SetTrigger("ChangeStance");
        }

        private void ButtonL2Event()
        {
            //Animator.SetInteger("Stance", 2);
            //Animator.SetTrigger("ChangeStance");
        }

        private void ButtonR2Event()
        {
            //Animator.SetInteger("Stance", 3);
            //Animator.SetTrigger("ChangeStance");
        }

        public void SetGamepad(Gamepad gamepad)
        {
            _gamepad = gamepad;
        }

    }
}
