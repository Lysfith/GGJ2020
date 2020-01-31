using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Characters
{
    public class C_CharacterPart : MonoBehaviour
    {
        [SerializeField] protected bool _isEnabled { get; private set; }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }
    }
}
