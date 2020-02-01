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
    public class C_RepairRoll : C_CharacterPart
    {

        private const float MAX_ANGLE_DIFF = Mathf.PI / 4;

        [SerializeField] private C_Character _character;



        private float _lastAngle = 0.0f;
        private float _progress = 0.0f;
        private float _turns = 5f;

        private void OnEnable()
        {
            _character = GetComponent<C_Character>();
            _progress = 0.0f;

            Assert.IsNotNull(_character);
        }

        private void Update() {

            var direction = _character.Stats.Direction;
            float angle = Mathf.Atan2(direction.y, direction.x);

            float angleDelta = angle - _lastAngle;
            
            // Wrap angles to [0, 360[
            if (angleDelta > Mathf.PI) angleDelta -= 2 * Mathf.PI;
            if (angleDelta < -Mathf.PI) angleDelta += 2 * Mathf.PI;

            if(Mathf.Abs(angleDelta) < MAX_ANGLE_DIFF) {
                _progress += Mathf.Abs(angleDelta);
            }


            Debug.Log("dO : " + angleDelta);
            Debug.Log("Progress : " + _progress / (2 * Mathf.PI));

            _lastAngle = angle;
        }

    }
}
