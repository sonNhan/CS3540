using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightedTurretUIInteract : MonoBehaviour
{
    public enum UIType
    {
        TARGETING_PRIO,
        UPGRADE,
        SELL

    }

    [SerializeField]
    UIType uiType;

    GameObject placementCursor;
    PlacementCursorBehavior placementCursorBehaviorScript;

    void Start()
    {
        placementCursor = GameObject.FindGameObjectWithTag("PlacementCursor");
        placementCursorBehaviorScript = placementCursor.GetComponent<PlacementCursorBehavior>();
    }

    public void TriggerUI()
    {
        GameObject highlightedTurret = placementCursorBehaviorScript.GetHighlightedTurret();
        TurretShoot turretShoot = highlightedTurret.GetComponent<TurretShoot>();
        switch (uiType)
        {
            case UIType.TARGETING_PRIO:
                turretShoot.ChangeTargetPriority();
                Text targetingText = gameObject.transform.Find("CurrentTargetingPriority").GetComponent<Text>();
                targetingText.text = turretShoot.GetTargetPriority().ToString();
                break;
            case UIType.UPGRADE:
                // TODO: subtract gold/check if we have enough gold
                turretShoot.UpgradeTurret();
                break;
            case UIType.SELL:
                placementCursorBehaviorScript.UnhighlightTurret(highlightedTurret);
                turretShoot.SellTurret();
                break;
            default:
                Debug.Log("Invalid UI element to trigger!");
                break;
        }
    }
}
