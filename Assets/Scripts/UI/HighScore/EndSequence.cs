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
        _score.anchorMin = new Vector2(0.5f, 0);
        _score.anchorMax = new Vector2(0.5f, 0);

        var sequence = DOTween.Sequence();
        sequence.Insert(0, _timer.DOAnchorPosY(_timer.anchoredPosition.y + 140, 1.5f));
        sequence.Insert(0, _score.DOAnchorPos(new Vector2(0, 300f), 1.5f));
        //sequence.Insert(0, _score.DOAnchorMin(new Vector2(.4f, .4f), 1.5f));
        //sequence.Insert(0, _score.DOAnchorMax(new Vector2(.4f, .4f), 1.5f));
        sequence.Insert(0, _score.DOScale(_endScale, 1.5f));
    }
}
