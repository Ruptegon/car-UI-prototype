using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RTT 
{
    public class Telltale : MonoBehaviour
    {
        [SerializeField]
        private Image tellTaleOnImage;

        [SerializeField]
        private TelltaleState state;

        [SerializeField]
        private float blinkingRate = 1;

        private TelltaleState lastState;
        private Sequence blinkingSequence;

        public void SetState(TelltaleState newState) 
        {
            state = newState;
        }

        private void Start()
        {
            lastState = state;
            OnStateChanged();
        }

        private void Update()
        {
            if (lastState != state) 
            {
                OnStateChanged();
            }
        }

        private void OnStateChanged()
        {
            if (lastState == TelltaleState.BLINKING) 
            {
                blinkingSequence?.Kill();
            }

            switch (state) 
            {
                case TelltaleState.ON:
                    tellTaleOnImage.enabled = true;
                    break;

                case TelltaleState.OFF:
                    tellTaleOnImage.enabled = false;
                    break;

                case TelltaleState.BLINKING:
                    StartBlinkingSequence();
                    break;
            }
            lastState = state;
        }

        private void StartBlinkingSequence() 
        {
            blinkingSequence = DOTween.Sequence();
            blinkingSequence.SetLoops(-1);
            blinkingSequence.AppendCallback(() => tellTaleOnImage.enabled = !tellTaleOnImage.enabled);
            blinkingSequence.AppendInterval(blinkingRate);
        }

        public enum TelltaleState { ON, OFF, BLINKING }
    }
}
