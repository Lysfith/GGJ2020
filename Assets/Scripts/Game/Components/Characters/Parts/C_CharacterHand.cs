using Assets.Scripts.Game.Components.Box;
using Assets.Scripts.Game.Components.Characters.Triggers;
using Assets.Scripts.Game.Components.Objects;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Characters.Parts
{
    public class C_CharacterHand : C_CharacterPart
    {
        [SerializeField] private C_Character _character;

        [SerializeField] private Transform _hand;
        [SerializeField] private C_TriggerHand _triggerHand;
        [SerializeField] private C_TriggerBot _triggerBot;
        [SerializeField] private C_TriggerWorkbench _triggerWorkbench;
        [SerializeField] private C_Object _object;
        [SerializeField] private float _raycastDistance;

        private void OnEnable()
        {
            _character = GetComponent<C_Character>();

            _triggerHand = GetComponentInChildren<C_TriggerHand>();
            _triggerBot = GetComponentInChildren<C_TriggerBot>();
            _triggerWorkbench = GetComponentInChildren<C_TriggerWorkbench>();

            Assert.IsNotNull(_character);
            Assert.IsNotNull(_triggerHand);
            Assert.IsNotNull(_triggerBot);
            Assert.IsNotNull(_triggerWorkbench);

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
            if (!_isEnabled)
            {
                return;
            }

            if (!TakeFromWorkbench())
            {
                var objects = _triggerHand.GetObjects();

                if (!objects.Any())
                {
                    return;
                }

                float min = float.MaxValue;
                C_Object objMin = null;

                foreach (var obj in objects)
                {
                    var distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < min)
                    {
                        min = distance;
                        objMin = obj;
                    }
                }

                Take(objMin);
            }


        }

        private void Take(C_Object obj)
        {
            _object = obj;
            _object.Take(this.gameObject);
            _object.transform.SetParent(_hand);
            _object.transform.localPosition = Vector3.zero;
            _object.transform.localRotation = Quaternion.identity;
            //_object.transform.localPosition = new Vector3(
            //    _object.OffsetHand.localPosition.x * _object.transform.localScale.x,
            //    _object.OffsetHand.localPosition.y * _object.transform.localScale.y,
            //    _object.OffsetHand.localPosition.z * _object.transform.localScale.z);

            DisableHand();

        }


        private bool TakeFromWorkbench()
        {
            // Does the ray intersect any objects excluding the player layer
            if (_triggerWorkbench.CurrentWorkbench != null && _triggerWorkbench.CurrentWorkbench.CanTakeObject())
            {
                var obj = _triggerWorkbench.CurrentWorkbench.TakeObject();
                Take(obj);
                _character.Mover.Enable();

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

            if (_triggerWorkbench.CurrentWorkbench != null && _triggerWorkbench.CurrentWorkbench.WorkbenchType != _object.ObjectType)
            {
                SoundManager.PlaySound(SoundList.Sound.error, gameObject);
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

            if(_object.gameObject.layer == LayerMask.NameToLayer("Waste"))
            {
                _object.Throw(transform.forward, this.gameObject);
                _object = null;
                EnableHand();
                return;
            }

            var hits = Physics.RaycastAll(_character.transform.position + Vector3.up, transform.forward, _raycastDistance, LayerMask.GetMask("Wall", "WallObjectWaste"));

            if (!hits.Any())
            {
                var box = _object.GetComponent<C_Box>();
                if (box == null)
                {
                    _object.Throw(transform.forward, this.gameObject);
                }
                else
                {
                    box.Open(this.gameObject);
                }

                _object = null;

                EnableHand();
            }
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(_character.transform.position + Vector3.up, transform.forward * _raycastDistance);
        }
    }
}
