using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telltale : MonoBehaviour
{
    [SerializeField]
    private Image tellTaleOnImage;

    public bool IsOn { get { return isOn; } set => SetIsOn(value); }
    private bool isOn;

    private void Start()
    {
        isOn = tellTaleOnImage.enabled;
    }

    private void SetIsOn(bool newIsOn) 
    {
        isOn = newIsOn;
        tellTaleOnImage.enabled = isOn;
    }
}
