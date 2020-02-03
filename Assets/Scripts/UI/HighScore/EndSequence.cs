using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndSequence : MonoBehaviour
{

    public RectTransform _score, _timer;
    public float _endScale;

    public void Play()
    {
        var sequence = DOTween.Sequence();
        sequence.Insert(0, _timer.DOAnchorPosY(_timer.anchoredPosition.y + 140, 1.5f));
        sequence.Insert(0, _score.DOAnchorMin(new Vector2(.4f, .4f), 1.5f));
        sequence.Insert(0, _score.DOAnchorMax(new Vector2(.4f, .4f), 1.5f));
        sequence.Insert(0, _score.DOScale(_endScale, 1.5f));
    }
}
