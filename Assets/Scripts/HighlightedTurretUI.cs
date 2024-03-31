using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighlightedTurretUI : MonoBehaviour
{
    GameObject UI;
    GameObject highlightedTurret;
    TextMeshProUGUI selectedTurretText;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("HighlightedTurretUI");
        selectedTurretText = UI.transform.Find("TurretTitle").GetComponent<TextMeshProUGUI>();
        UI.SetActive(false);
    }

    public void SetUIActive(bool flag, GameObject turret)
    {
        highlightedTurret = turret;
        selectedTurretText.text = $"Selected Turret\n<u>{turret.name.Replace("(Clone)", "")}</u>";
        UI.SetActive(flag);
    }

}
