using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Assets.Scripts.Game.Components.Repairing;

namespace Assets.Scripts.Game.Components.Characters.Parts
{
    public class C_Repair : C_CharacterPart
    {
        [SerializeField] private C_Character _character;
        [SerializeField] private float _raycastDistance;

        private C_Workbench _currentWorkbench;

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
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.forward, out hit, _raycastDistance, LayerMask.GetMask("Workbench")))
            {
                var workbench = hit.collider.GetComponent<C_Workbench>();
                if (workbench)
                {
                    _character.Mover.Disable();
                    workbench.OnRepair();
                    _currentWorkbench = workbench;
                    if(workbench.GetWorkbenchType() == ObjectType.LEFT_ARM || workbench.GetWorkbenchType() == ObjectType.RIGHT_ARM)
                        SoundManager.PlaySound(SoundList.Sound.workbenchturn, this.gameObject, false);
                    else
                        SoundManager.PlaySound(SoundList.Sound.workbenchtap, this.gameObject, false);
                }
            }

        }

        private void Release(object sender, object args)
        {
            _currentWorkbench = null;
            _character.Mover.Enable();
        }

        private void Update()
        {
            if(_currentWorkbench)
            {
                var direction = _character.Stats.Direction;
                float angle = Mathf.Atan2(direction.y, direction.x);

                _currentWorkbench.UpdateAngle(angle);
            }
        }

    }
}
