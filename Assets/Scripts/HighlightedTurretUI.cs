using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighlightedTurretUI : MonoBehaviour
{
    GameObject UI;
    GameObject highlightedTurret;
    TurretShoot turretShootScript;
    TextMeshProUGUI selectedTurretText;
    TextMeshProUGUI targetPriorityText;
    // TODO upgrade and sell prices

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("HighlightedTurretUI");
        Transform UIBG = UI.transform.Find("Background");
        selectedTurretText = UIBG.transform.Find("TurretTitle").GetComponent<TextMeshProUGUI>();
        targetPriorityText = UIBG.transform.Find("TargetPriority").Find("TurretPriorityText").GetComponent<TextMeshProUGUI>();
        UI.SetActive(false);
    }

    void Update()
    {
        if (highlightedTurret != null)
        {
            string currentTargetPriority = turretShootScript.GetTargetPriority().ToString();
            targetPriorityText.text = $"Targeting Priority\n<u>{currentTargetPriority}</u>";
        }
    }

    public GameObject GetHighlightedTurret()
    {
        Debug.Log(highlightedTurret);
        return highlightedTurret;
    }

    public void SetUIActive(bool flag, GameObject turret)
    {
        if (flag)
        {
            // Unlock Cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // Instantiated objects automatically have " (Clone)" appended to their name
            // so we should remove that before displaying it to the user
            highlightedTurret = turret;
            turretShootScript = highlightedTurret.GetComponent<TurretShoot>();
            selectedTurretText.text = $"Selected Turret\n<u>{turret.name.Replace("(Clone)", "")}</u>";
        }
        else
        {
            // Lock Cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        UI.SetActive(flag);
    }

    public void ChangeTurretTargetPriority(bool right)
    {
        turretShootScript.ChangeTargetPriority(right);
    }

}
