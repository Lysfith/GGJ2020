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
    public class C_Repair : C_CharacterPart
    {
        [SerializeField] private C_Character _character;

        private bool _isRepairing;

        private void OnEnable()
        {
            _character = GetComponent<C_Character>();
            _character.Control.OnWestButtonDown += Repair;
            _character.Control.OnWestButtonUp += Release;

            Assert.IsNotNull(_character);
        }

        private void OnDisable() {
            _character.Control.OnWestButtonDown -= Repair;
            _character.Control.OnWestButtonUp -= Release;
        }

        private void Repair(object sender, object args) {
            _character.Mover.Disable();
        }

        private void Release(object sender, object args)
        {
            _character.Mover.Enable();
        }

    }
}
