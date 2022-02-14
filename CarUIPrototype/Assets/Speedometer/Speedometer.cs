using System;
using TMPro;
using UnityEngine;

namespace Speedometer 
{
    public class Speedometer : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer velocitySlider;

        [SerializeField]
        private TextMeshPro currentVelocityText;

        [SerializeField]
        private int maxChange = 2;

        [SerializeField]
        private int minChange = 0;

        [SerializeField]
        private int maxSpeed = 200;

        [SerializeField]
        private int seed = 2111;

        private System.Random random;
        private int currentVelocityValue;
        private int sign = 1;

        void Start()
        {
            random = new System.Random(seed);
            velocitySlider.material.SetFloat("_MaxSpeed", maxSpeed);
            currentVelocityValue = random.Next(0, maxSpeed);
            velocitySlider.material.SetFloat("_Speed", currentVelocityValue);
            currentVelocityText.text = currentVelocityValue.ToString();
        }

        void FixedUpdate()
        {
            if (currentVelocityValue > maxSpeed * 9 / 10)
            {
                sign = -1;
            }
            else if (currentVelocityValue < maxSpeed / 10) 
            {
                sign = 1;
            }

            currentVelocityValue = Math.Max(0, Math.Min(maxSpeed, currentVelocityValue + sign * random.Next(minChange, maxChange + 1)));
            velocitySlider.material.SetFloat("_Speed", currentVelocityValue);
            currentVelocityText.text = currentVelocityValue.ToString();
        }
    }
}
