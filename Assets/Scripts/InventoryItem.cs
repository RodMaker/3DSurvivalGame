using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    public bool isTrashable;
 
    // --- Item Info UI --- //
    private GameObject itemInfoUI;
 
    private TextMeshProUGUI itemInfoUI_itemName;
    private TextMeshProUGUI itemInfoUI_itemDescription;
    private TextMeshProUGUI itemInfoUI_itemFunctionality;
 
    public string thisName, thisDescription, thisFunctionality;
 
    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;
 
    public float healthEffect;
    public float hungerEffect;
    public float thirstEffect;

    // --- Equipping --- //
    public bool isEquippable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot;

    public bool isSelected;

    public bool isUseable;
    public GameObject itemPendingToBeUsed;
 
    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }
 
    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }
 
    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }
 
    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                // Setting this specific gameobject to be the item we want to destroy later
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, hungerEffect, thirstEffect);
            }

            if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;
            }

            if (isUseable)
            {
                itemPendingToBeUsed = gameObject;

                UseItem();
            }
        } 
    }

    private void UseItem()
    {
        itemInfoUI.SetActive(false);

        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolsScreenUI.SetActive(false);
        CraftingSystem.Instance.survivalScreenUI.SetActive(false);
        CraftingSystem.Instance.refineScreenUI.SetActive(false);
        CraftingSystem.Instance.constructionScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true;

        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Foundation":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            default:
                // do nothing
                break;
        }
    }

    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }

            if (isUseable && itemPendingToBeUsed == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }
 
    private void consumingFunction(float healthEffect, float hungerEffect, float thirstEffect)
    {
        itemInfoUI.SetActive(false);
 
        healthEffectCalculation(healthEffect);
 
        hungerEffectCalculation(hungerEffect);
 
        thirstEffectCalculation(thirstEffect);
 
    }
 
 
    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //
 
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;
 
        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }
 
 
    private static void hungerEffectCalculation(float hungerEffect)
    {
        // --- Calories --- //
 
        float hungerBeforeConsumption = PlayerState.Instance.currentHunger;
        float maxHunger = PlayerState.Instance.maxHunger;
 
        if (hungerEffect != 0)
        {
            if ((hungerBeforeConsumption + hungerEffect) > maxHunger)
            {
                PlayerState.Instance.setHunger(maxHunger);
            }
            else
            {
                PlayerState.Instance.setHunger(hungerBeforeConsumption + hungerEffect);
            }
        }
    }
 
 
    private static void thirstEffectCalculation(float thirstEffect)
    {
        // --- Hydration --- //
 
        float thirstBeforeConsumption = PlayerState.Instance.currentThirst;
        float maxThirst = PlayerState.Instance.maxThirst;
 
        if (thirstEffect != 0)
        {
            if ((thirstBeforeConsumption + thirstEffect) > maxThirst)
            {
                PlayerState.Instance.setThirst(maxThirst);
            }
            else
            {
                PlayerState.Instance.setThirst(thirstBeforeConsumption + thirstEffect);
            }
        }
    }
}
