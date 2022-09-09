using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI hungerCounter;

    public GameObject playerState;

    private float currentHunger, maxHunger;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        float fillValue = currentHunger / maxHunger;
        slider.value = fillValue;

        hungerCounter.text = currentHunger + " / " + maxHunger;
    }
}
