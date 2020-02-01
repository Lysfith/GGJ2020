using Assets.Scripts.Game.Components.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.ScriptableObjects.Objects
{
    [CreateAssetMenu(fileName = "NewObjectList", menuName = "Game/Objects/ObjectList")]
    public class SO_ObjectList : ScriptableObject
    {
        public List<C_Object> Objects;
    }
}
