using Assets.Scripts.Game.Components.Characters.Triggers;
using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.Components.Repairing;
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
        [SerializeField] private float _raycastDistance;

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

            if (!TakeFromWorkbench())
            {
                if (_triggerHand.CurrentObject == null)
                {
                    return;
                }

                Take(_triggerHand.CurrentObject);
            }

            
        }

        private void Take(C_Object obj)
        {
            _object = obj;
            _object.Take(this.gameObject);
            _object.transform.SetParent(_hand);
            _object.transform.localPosition = new Vector3(
                _object.OffsetHand.localPosition.x * _object.transform.localScale.x,
                _object.OffsetHand.localPosition.y * _object.transform.localScale.y,
                _object.OffsetHand.localPosition.z * _object.transform.localScale.z);

            DisableHand();

        }


        private bool TakeFromWorkbench()
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastDistance, LayerMask.GetMask("Workbench")))
            {
                var workbench = hit.collider.GetComponent<C_Workbench>();
                if (workbench != null && workbench.CanTakeObject())
                {
                    var obj = workbench.TakeObject();
                    Take(obj);
                    _character.Mover.Enable();
                }

                return true;
            }

            return false;
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
                _triggerBot.CurrentBot.AddPart(_object, this.gameObject);
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
