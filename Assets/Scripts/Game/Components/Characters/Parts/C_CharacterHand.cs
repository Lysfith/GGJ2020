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
        [SerializeField] private C_TriggerBot _triggerBot;
        [SerializeField] private C_Object _object;

        private void OnEnable()
        {
            _character = GetComponent<C_Character>();

            _triggerHand = GetComponentInChildren<C_TriggerHand>();
            _triggerBot = GetComponentInChildren<C_TriggerBot>();

            Assert.IsNotNull(_character);
            Assert.IsNotNull(_triggerHand);
            Assert.IsNotNull(_triggerBot);

            _character.Control.OnSouthButtonUp += ButtonSouthReleased;
            _character.Control.OnWestButtonUp += ButtonWestReleased;

            EnableHand();
        }

        private void OnDisable()
        {
            _character.Control.OnSouthButtonUp -= ButtonSouthReleased;
            _character.Control.OnWestButtonUp -= ButtonWestReleased;
        }

        private void ButtonWestReleased(object sender, EventArgs e)
        {
            if (_object != null)
            {
                Throw();
            }
        }

        private void ButtonSouthReleased(object sender, EventArgs e)
        {
            if (_object != null)
            {
                Release();
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
            _object.Take(this.gameObject);
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

            if (_triggerBot.CurrentBot != null)
            {
                _triggerBot.CurrentBot.AddPart(_object);
            }
            else
            {
                _object.Release(this.gameObject);
            }

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
            _object.Throw(transform.forward, this.gameObject);
            _object = null;

            EnableHand();
        }
    }
}
