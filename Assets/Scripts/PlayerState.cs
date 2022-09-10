﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // player health
    public float currentHealth;
    public float maxHealth;

    // player hunger
    public float currentHunger;
    public float maxHunger;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    // player thirst
    public float currentThirst;
    public float maxThirst;

    public bool isHydrated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;

        StartCoroutine(increaseThirst());
    }
    
    IEnumerator increaseThirst()
    {
        while(true)
        {
            currentThirst -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentHunger -= 1;
        }
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setHunger(float newHunger)
    {
        currentHunger = newHunger;
    }

    public void setThirst(float newThirst)
    {
        currentThirst = newThirst;
    }
}
