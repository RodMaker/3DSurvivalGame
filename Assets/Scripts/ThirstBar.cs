using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThirstBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI thirstCounter;

    public GameObject playerState;

    private float currentThirst, maxThirst;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentThirst = playerState.GetComponent<PlayerState>().currentThirst;
        maxThirst = playerState.GetComponent<PlayerState>().maxThirst;

        float fillValue = currentThirst / maxThirst;
        slider.value = fillValue;

        thirstCounter.text = currentThirst + " / " + maxThirst;
    }
}
