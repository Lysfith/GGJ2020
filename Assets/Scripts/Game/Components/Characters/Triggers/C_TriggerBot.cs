using Assets.Scripts.Game.Components.Bots;
using Assets.Scripts.Game.Components.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Characters.Triggers
{
    public class C_TriggerBot : MonoBehaviour
    {
        public C_Bot CurrentBot;

        private void OnTriggerEnter(Collider other)
        {
            CurrentBot = other.GetComponent<C_Bot>();
        }

        private void OnTriggerExit(Collider other)
        {
            CurrentBot = null;
        }
    }
}
