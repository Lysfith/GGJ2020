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

        private void OnEnable()
        {
            _character = GetComponent<C_Character>();
            _agent = GetComponent<NavMeshAgent>();

            Assert.IsNotNull(_character);
            Assert.IsNotNull(_agent);
        }

        private void Update()
        {
            if(!_isEnabled)
            {
                return;
            }

            if(_character.Stats.Direction.x == 0 && _character.Stats.Direction.y == 0)
            {
                return;
            }

            var newPosition = new Vector3(_character.Stats.Direction.x, 0, _character.Stats.Direction.y);
            newPosition = newPosition.normalized * Time.deltaTime * _speed;
            _agent.Move(newPosition);
        }
    }
}
