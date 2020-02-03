using Assets.Scripts.Game.Components.Systems;
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
    public class C_CharacterMover : C_CharacterPart
    {
        [SerializeField] private C_Character _character;
        [SerializeField] private NavMeshAgent _agent;

        [SerializeField] private float _speed = 10;

        private bool _ismoving = false;


        private void OnEnable()
        {
            _character = GetComponent<C_Character>();
            _agent = GetComponent<NavMeshAgent>();

            Assert.IsNotNull(_character);
            Assert.IsNotNull(_agent);

        }

        private void FixedUpdate()
        {
            if (!_isEnabled)
            {
                return;
            }

            if (_character.Stats.Direction.x == 0 && _character.Stats.Direction.y == 0)
            {
                if (_ismoving == true)
                {
                    _character.Animator.SetBool("IsMoving", false);
                    _ismoving = false;
                    SoundManager.StopFoodStepSound(this.gameObject);
                }
                return;
            }

            if (_ismoving == false)
            {
                _character.Animator.SetBool("IsMoving", true);
                _ismoving = true;
                SoundManager.StartFoodStepSound(this.gameObject);
            }

            var direction = (new Vector3(_character.Stats.Direction.x, 0, _character.Stats.Direction.y)).normalized;

            var offset = direction * Time.fixedDeltaTime * _speed;
            _agent.Move(offset);

            var directionFacing = Vector3.Slerp(transform.forward, direction, Time.fixedDeltaTime * 20);
            transform.forward = directionFacing;
            //transform.forward = direction;
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.destination = destination;
        }
    }

}
