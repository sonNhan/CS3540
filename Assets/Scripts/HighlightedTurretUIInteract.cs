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
    GameController gameControllerScript;

    void Start()
    {
        placementCursor = GameObject.FindGameObjectWithTag("PlacementCursor");
        placementCursorBehaviorScript = placementCursor.GetComponent<PlacementCursorBehavior>();
        gameControllerScript = GameObject.Find("LevelManager").GetComponent<GameController>();
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
                if (gameControllerScript.GetMoney() >= 10)
                {
                    turretShoot.UpgradeTurret();
                    gameControllerScript.AddMoney(-10);
                }
                break;
            case UIType.SELL:
                gameControllerScript.AddMoney(5);
                placementCursorBehaviorScript.UnhighlightTurret(highlightedTurret);
                turretShoot.SellTurret();
                break;
            default:
                Debug.Log("Invalid UI element to trigger!");
                break;
        }
    }
}
