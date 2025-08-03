using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance {get; set; }

    public bool onTarget;
    public GameObject interaction_Info_UI;
    Text interaction_text;

    private Transform highlight;

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        onTarget = false;
    }

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

    void Update()
    {
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            highlight = hit.transform;

            if (interactable && interactable.playerInRange)
            {
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
                onTarget = true;

                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                interaction_Info_UI.SetActive(false);
                onTarget = false;

                highlight = null;
            }

        }

        else
        {
            interaction_Info_UI.SetActive(false);
            onTarget = false;
        }
    }
}