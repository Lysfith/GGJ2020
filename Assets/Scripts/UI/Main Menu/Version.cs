using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class Version : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void OnEnable()
        {
            _text = GetComponent<TextMeshProUGUI>();

            _text.text = $"V{Application.version}";
        }
    }
}
