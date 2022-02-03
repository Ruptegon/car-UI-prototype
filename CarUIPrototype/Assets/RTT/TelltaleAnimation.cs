using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelltaleAnimation : MonoBehaviour
{
    [SerializeField]
    private float blinkingRate = 2;

    [SerializeField]
    private Telltale telltale;

    private void Start()
    {
        StartTelltaleAnimationLoop();
    }

    private void StartTelltaleAnimationLoop() 
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetLoops(-1);
        sequence.AppendInterval(blinkingRate);
        sequence.AppendCallback(() => telltale.IsOn = !telltale.IsOn);
    }
}
