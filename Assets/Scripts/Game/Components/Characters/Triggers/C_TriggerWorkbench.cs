using Assets.Scripts.Game.Components.Bots;
using Assets.Scripts.Game.Components.Objects;
using Assets.Scripts.Game.Components.Repairing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Components.Characters.Triggers
{
    public class C_TriggerWorkbench : MonoBehaviour
    {
        public C_Workbench CurrentWorkbench;

        private void OnTriggerEnter(Collider other)
        {
            CurrentWorkbench = other.GetComponent<C_Workbench>();
        }

        private void OnTriggerExit(Collider other)
        {
            CurrentWorkbench = null;
        }
    }
}
