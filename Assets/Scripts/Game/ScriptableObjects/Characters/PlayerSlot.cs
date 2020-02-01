using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.ScriptableObjects.Characters
{

    [CreateAssetMenu(fileName = "New Player Slot", menuName = "Game/Characters/Player Slot")]
    public class PlayerSlot : ScriptableObject
    {

        public Color _color;
        public bool _active;
        public PlayerType _type;
        public Gamepad _gamepad;
        public bool _ready;

    }
}
