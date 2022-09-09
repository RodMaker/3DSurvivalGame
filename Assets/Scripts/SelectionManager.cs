using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;

    public GameObject selectedObject;

    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text;

    public Image centerDotImage;
    public Image handIcon;
 
    //DONT FORGET TO ALSO ADD -  using UnityEngine.UI;  - at the top of the script
    
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
 
    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }
 
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
 
            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName(); 
                interaction_Info_UI.SetActive(true);

                if (interactable.CompareTag("Pickable"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }
                else
                {
                    handIcon.gameObject.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);
                }

            }
            else // if there is a hit, but without an Interactable Object
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                
                handIcon.gameObject.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
            }
 
        }
        else // if there is no hit at all
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);

            handIcon.gameObject.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
        }
       
    
    }
}
