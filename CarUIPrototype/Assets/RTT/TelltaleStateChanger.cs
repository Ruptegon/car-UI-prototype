using DG.Tweening;
using UnityEngine;

namespace RTT 
{
    /// <summary>
    /// Helper for visualisation tests. Changes states of Telltales between firstStateToChange and secondStateToChange once every interval.
    /// </summary>
    public class TelltaleStateChanger : MonoBehaviour
    {
        [SerializeField]
        private float interval = 2;

        [SerializeField]
        private Telltale telltale;

        [SerializeField]
        private Telltale.TelltaleState firstStateToChange;
        
        [SerializeField]
        private Telltale.TelltaleState secondStateToChange;

        private Sequence sequence;

        private void OnEnable()
        {
            StartTelltaleStateChangerLoop();
        }

        private void OnDisable()
        {
            sequence.Kill();
        }

        private void StartTelltaleStateChangerLoop() 
        {
            sequence = DOTween.Sequence();
            sequence.SetLoops(-1);
            sequence.AppendCallback(() => telltale.SetState(firstStateToChange));
            sequence.AppendInterval(interval);
            sequence.AppendCallback(() => telltale.SetState(secondStateToChange));
            sequence.AppendInterval(interval);
        }
    }
}
