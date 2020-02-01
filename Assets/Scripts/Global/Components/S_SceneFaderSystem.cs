using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Global.Components
{
    public class S_SceneFaderSystem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AnimationCurve _curve;

        public UnityEvent OnSceneLoaded;

        // Start is called before the first frame update

        void Start()
        {
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            var time = 1f;

            while (time > 0f)
            {
                time -= Time.deltaTime;
                var a = _curve.Evaluate(time);
                _canvasGroup.alpha = a;
                yield return 0;
            }

            OnSceneLoaded?.Invoke();
        }

        public void FadeOut(string scene)
        {
            StartCoroutine(FadeOutPrivate(scene));
        }

        private IEnumerator FadeOutPrivate(string scene)
        {
            var time = 0f;

            while (time < 1f)
            {
                time += Time.deltaTime;
                var a = _curve.Evaluate(time);
                _canvasGroup.alpha = a;
                yield return 0;
            }

            _canvasGroup.alpha = 1;

            SceneManager.LoadScene(scene);

        }
    }
}
