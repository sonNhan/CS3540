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

    GameObject placementCursor, highlightedTurret;
    PlacementCursorBehavior placementCursorBehaviorScript;
    TurretShoot turretShoot;

    void Start()
    {
        placementCursor = GameObject.FindGameObjectWithTag("PlacementCursor");
        placementCursorBehaviorScript = placementCursor.GetComponent<PlacementCursorBehavior>();
    }

    public void TriggerUI()
    {
        switch (uiType)
        {
            case UIType.TARGETING_PRIO:
                turretShoot = placementCursorBehaviorScript.GetHighlightedTurret().GetComponent<TurretShoot>();
                turretShoot.ChangeTargetPriority();
                Text targetingText = gameObject.transform.Find("CurrentTargetingPriority").GetComponent<Text>();
                targetingText.text = turretShoot.GetTargetPriority().ToString();
                break;
            case UIType.UPGRADE:
                break;
            case UIType.SELL:
                break;
            default:
                Debug.Log("Invalid UI element to trigger!");
                break;
        }
    }
}
