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
    TextMeshProUGUI rangeUpgradePriceText;
    TextMeshProUGUI damageUpgradePriceText;
    TextMeshProUGUI speedUpgradePriceText;
    TextMeshProUGUI sellValueText;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("HighlightedTurretUI");
        Transform UIBG = UI.transform.Find("Background");
        Transform upgrade = UIBG.transform.Find("Upgrade");
        selectedTurretText = UIBG.transform.Find("TurretTitle").GetComponent<TextMeshProUGUI>();
        targetPriorityText = UIBG.transform.Find("TargetPriority").Find("TurretPriorityText").GetComponent<TextMeshProUGUI>();
        rangeUpgradePriceText = upgrade.transform.Find("UpgradeRangeButton").Find("CostText (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        damageUpgradePriceText = upgrade.transform.Find("UpgradeDamageButton").Find("CostText (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        speedUpgradePriceText = upgrade.transform.Find("UpgradeSpeedButton").Find("CostText (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        sellValueText = UIBG.transform.Find("Sell").Find("SellButton").Find("ValueText (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        UI.SetActive(false);
    }

    void Update()
    {
        if (highlightedTurret != null)
        {
            string currentTargetPriority = turretShootScript.GetTargetPriority().ToString();
            targetPriorityText.text = $"Targeting Priority\n<u>{currentTargetPriority}</u>";
            if (!turretShootScript.CanAffordUpgrade(Constants.UpgradeType.RANGE))
            {
                rangeUpgradePriceText.color = Color.red;
            }
            else
            {
                rangeUpgradePriceText.color = Color.white;
            }
            if (!turretShootScript.CanAffordUpgrade(Constants.UpgradeType.DAMAGE))
            {
                damageUpgradePriceText.color = Color.red;
            }
            else
            {
                damageUpgradePriceText.color = Color.white;
            }
            if (!turretShootScript.CanAffordUpgrade(Constants.UpgradeType.SPEED))
            {
                speedUpgradePriceText.color = Color.red;
            }
            else
            {
                speedUpgradePriceText.color = Color.white;
            }
            rangeUpgradePriceText.text = turretShootScript.GetRangeUpgradeCost().ToString();
            damageUpgradePriceText.text = turretShootScript.GetDamageUpgradeCost().ToString();
            speedUpgradePriceText.text = turretShootScript.GetSpeedUpgradeCost().ToString();
            sellValueText.text = turretShootScript.GetSellValue().ToString();
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

    public void UpgradeTurretRange()
    {
        turretShootScript.Upgrade(Constants.UpgradeType.RANGE);
    }

    public void UpgradeTurretDamage()
    {
        turretShootScript.Upgrade(Constants.UpgradeType.DAMAGE);
    }

    public void UpgradeTurretSpeed()
    {
        turretShootScript.Upgrade(Constants.UpgradeType.SPEED);
    }

    public void SellTurret()
    {
        turretShootScript.SellTurret();
    }

}
