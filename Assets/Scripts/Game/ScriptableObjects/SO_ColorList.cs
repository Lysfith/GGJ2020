using Assets.Scripts.Game.Components.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewColorList", menuName = "Game/ColorList")]
    public class SO_ColorList : ScriptableObject
    {
        public List<Color> Colors;
    }
}
