using Assets.Scripts.Game.Components.Characters.Triggers;
using Assets.Scripts.Game.Components.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Characters.Parts
{
    public class C_CharacterHand : C_CharacterPart
    {
        [SerializeField] private C_Character _character;

        [SerializeField] private Transform _hand;
        [SerializeField] private C_TriggerHand _triggerHand;
        [SerializeField] private C_Object _object;

        private void OnEnable()
        {
            _character = GetComponent<C_Character>();

            Assert.IsNotNull(_character);

            _character.Control.OnSouthButtonDown += ButtonPressed;
            _character.Control.OnSouthButtonUp += ButtonReleased;

            EnableHand();
        }

        private void OnDisable()
        {
            _character.Control.OnSouthButtonDown -= ButtonPressed;
            _character.Control.OnSouthButtonUp -= ButtonReleased;
        }

        private void ButtonPressed(object sender, EventArgs e)
        {
            //if (_object != null)
            //{
            //    return;
            //}

            //Take();
        }

        private void ButtonReleased(object sender, EventArgs e)
        {
            if (_object != null)
            {
                Throw();
            }
            else
            {
                Take();
            }
        }

        private void EnableHand()
        {
            _triggerHand.gameObject.SetActive(true);
        }

        private void DisableHand()
        {
            _triggerHand.gameObject.SetActive(false);
        }

        public void Take()
        {
            if(!_isEnabled)
            {
                return;
            }

            if(_triggerHand.CurrentObject == null)
            {
                return;
            }

            _object = _triggerHand.CurrentObject;
            _object.Take();
            _object.transform.SetParent(_hand);
            _object.transform.localPosition = Vector3.zero;

            DisableHand();
        }

        public void Release()
        {
            if (!_isEnabled)
            {
                return;
            }

            if (_object == null)
            {
                return;
            }

            _object.transform.SetParent(null);
            _object.Release();
            _object = null;

            EnableHand();
        }

        public void Throw()
        {
            if (!_isEnabled)
            {
                return;
            }

            if (_object == null)
            {
                return;
            }

            _object.transform.SetParent(null);
            _object.Throw(transform.forward);
            _object = null;

            EnableHand();
        }
    }
}
