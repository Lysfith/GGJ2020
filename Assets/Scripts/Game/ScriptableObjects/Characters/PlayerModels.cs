using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.ScriptableObjects.Characters
{

    [CreateAssetMenu(fileName = "New Player Models", menuName = "Game/Characters/Player Models")]
    public class PlayerModels : ScriptableObject
    {
        public GameObject[] prefabs;

    }
}
