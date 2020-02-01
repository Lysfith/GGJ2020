using Assets.Scripts.Game.Components.Characters.Others;
using Assets.Scripts.Game.Components.Characters.Parts;
using Assets.Scripts.Game.Components.Characters.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Game.Components.Characters
{
    public class C_Character : MonoBehaviour
    {
        [SerializeField] private C_CharacterStats _stats;

        [SerializeField] private C_CharacterMover _mover;
        [SerializeField] private C_CharacterControl _control;
        [SerializeField] private C_Repair _repair;
        [SerializeField] private C_CharacterHand _hand;

        public C_CharacterStats Stats => _stats;
        public C_CharacterMover Mover => _mover;
        public C_CharacterControl Control => _control;

        private void OnEnable()
        {
            _stats = GetComponent<C_CharacterStats>();
            _mover = GetComponent<C_CharacterMover>();
            _control = GetComponent<C_CharacterControl>();
            _repair = GetComponent<C_Repair>();
            _hand = GetComponent<C_CharacterHand>();

            Assert.IsNotNull(_mover);
            Assert.IsNotNull(_control);
            Assert.IsNotNull(_repair);
            Assert.IsNotNull(_hand);


            _mover.Enable();
            _control.Enable();
            _repair.Enable();
            _hand.Enable();
        }

        private void OnDisable()
        {
            _mover.Disable();
            _control.Disable();

            _repair.Disable();
            _hand.Disable();
        }
    }
}
