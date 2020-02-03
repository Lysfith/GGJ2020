using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class HighScorePanel : MonoBehaviour
    {
        [SerializeField] private HighScore _highScores;
        [SerializeField] private GameObject _highScorePrefab;

        private void OnEnable()
        {
            Clear();

            if(_highScores.joueur != null && _highScores.joueur.Any())
            {
                for(int i = 0; i < _highScores.joueur.Length; i++)
                {
                    if(_highScores.score[i] == 0)
                    {
                        continue;
                    }

                    ShowScore(_highScores.joueur[i], _highScores.score[i]);
                }
            }
        }

        private void ShowScore(string name, int score)
        {
            var go = Instantiate(_highScorePrefab, transform);
            var text = go.GetComponent<TextMeshProUGUI>();
            text.text = $"{score} - {name}";
        }

        private void Clear()
        {
            int childs = transform.childCount;
            for (int i = childs - 1; i > 0; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
