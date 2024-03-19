using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedTurretUIBehavior : MonoBehaviour
{
    GameObject targetingPriorityMenu, sellMenu, upgradeMenu;
    GameObject currentHighlightedUI;
    List<GameObject> UIElements;
    int highlightedIndex = 0; // which UI element is highlighted
    bool UIVisible;

    // Start is called before the first frame update
    void Start()
    {
        UIElements = new List<GameObject>();
        targetingPriorityMenu = GameObject.FindGameObjectWithTag("TargetingPriorityUI");
        UIElements.Add(targetingPriorityMenu);
        upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeUI");
        UIElements.Add(upgradeMenu);
        sellMenu = GameObject.FindGameObjectWithTag("SellUI");
        UIElements.Add(sellMenu);
        ShowUI(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleUI();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }
    }

    void CycleUI()
    {
        if (UIVisible)
        {
            if (highlightedIndex + 1 >= UIElements.Count)
            {
                highlightedIndex = 0;
                ShowUIButton(false, highlightedIndex);
            }
            else
            {
                ShowUIButton(false, highlightedIndex);
                highlightedIndex++;
            }
            ShowUIButton(true, highlightedIndex);
        }
    }

    void Interact()
    {
        HighlightedTurretUIInteract highlightedUIScript = UIElements[highlightedIndex].GetComponent<HighlightedTurretUIInteract>();
        highlightedUIScript.TriggerUI();

    }

    public void ShowUIButton(bool flag, int index)
    {
        currentHighlightedUI = UIElements[index];
        currentHighlightedUI.transform.Find("Button").gameObject.SetActive(flag);
    }

    public void ShowUI(bool flag)
    {
        if (flag)
        {
            upgradeMenu.SetActive(true);
            targetingPriorityMenu.SetActive(true);
            sellMenu.SetActive(true);
            ShowUIButton(true, highlightedIndex);
            UIVisible = true;
        }
        else
        {
            upgradeMenu.SetActive(false);
            targetingPriorityMenu.SetActive(false);
            sellMenu.SetActive(false);
            UIVisible = false;
        }
    }

}
