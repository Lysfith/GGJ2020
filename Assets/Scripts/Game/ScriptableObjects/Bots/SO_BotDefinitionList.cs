using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.ScriptableObjects.Bots
{
    [CreateAssetMenu(fileName = "NewBotDefinitionList", menuName = "Game/Bots/BotDefinitionList")]
    public class SO_BotDefinitionList : ScriptableObject
    {
        public List<SO_BotDefinition> Bots;
    }
}
