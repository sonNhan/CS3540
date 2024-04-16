using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCursorBehavior : MonoBehaviour
{

    [SerializeField]
    float cursorSensitivity = 10f;
    [SerializeField]// HACK: need a cleaner way to represent different selectable turrets
    GameObject turret1, turret2;
    [SerializeField]
    GameObject ExplosionSpell, BlizzardSpell;
    [SerializeField]
    float explosionRadius, blizzardRadius;
    [SerializeField]
    int explosionDamage, blizzardDamage, blizzardSlow;
    [SerializeField]
    int explosionManaCost, blizzardManaCost;

    List<Constants.Ability> learnedAbilities;
    GameObject terrain;
    GameObject currentTurret, highlightedTurret, hoveredTurret, placementPointer, turretParent;
    HighlightedTurretUI highlightedTurretUIScript;
    GameObject levelManager;
    GameController gameControllerScript;
    Renderer placementPointerRenderer;
    bool selectedTurret = false;
    float highlightDelay = 0.5f, lastPlacedTime = 0f;
    float minX, minZ, maxX, maxZ;

    // Start is called before the first frame update
    void Start()
    {
        learnedAbilities = new List<Constants.Ability>();
        levelManager = GameObject.Find("LevelManager");
        gameControllerScript = levelManager.GetComponent<GameController>();
        terrain = GameObject.Find("Terrain").transform.Find("Unplaceable").gameObject;
        placementPointer = GameObject.Find("PlacementPointer");
        placementPointerRenderer = placementPointer.GetComponent<Renderer>();
        turretParent = GameObject.Find("Turrets");
        highlightedTurretUIScript = GameObject.Find("UI").GetComponent<HighlightedTurretUI>();
        DetermineBounds();
        // Temp
        learnedAbilities.Add(Constants.Ability.EXPLOSION);
        learnedAbilities.Add(Constants.Ability.BLIZZARD);
    }

    // Update is called once per frame
    void Update()
    {
        // Placement cursor should be disabled when a turret is highlighted 
        // to allow for the player to interact with the UI
        if (highlightedTurret == null)
        {
            MovePointer();
            RotatePointer();
            if (selectedTurret)
            {
                MoveTurret();
            }
        }
        PlaceTurret();
        HighlightTurret();
        CheckForAbilityCast();
    }

    void DetermineBounds()
    {
        TerrainController terrainController = GameObject.Find("Terrain").GetComponent<TerrainController>();
        List<GameObject> tiles = terrainController.GetTiles();
        foreach (GameObject tile in tiles)
        {
            minX = Mathf.Min(minX, tile.transform.position.x);
            maxX = Mathf.Max(maxX, tile.transform.position.x);
            minZ = Mathf.Min(minZ, tile.transform.position.z);
            maxZ = Mathf.Max(maxZ, tile.transform.position.z);
        }

    }

    void MovePointer()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        placementPointer.transform.Translate(mouseX * cursorSensitivity, mouseY * cursorSensitivity, 0f);

        float clampX = Mathf.Clamp(placementPointer.transform.position.x, minX, maxX);
        float clampZ = Mathf.Clamp(placementPointer.transform.position.z, minZ, maxZ);

        placementPointer.transform.position = new Vector3(clampX, placementPointer.transform.position.y, clampZ);
    }

    void RotatePointer()
    {
        // Get the rotation of the camera
        Quaternion cameraRotation = Camera.main.transform.rotation;

        // Set the rotation of the cursor to match the rotation of the camera
        placementPointer.transform.rotation = Quaternion.Euler(90f, cameraRotation.eulerAngles.y, cameraRotation.eulerAngles.z);
    }

    void MoveTurret()
    {
        currentTurret.transform.position = placementPointer.transform.position;

        // Check if we are placing on valid terrain, and represent that in the turret's placement indicator
        currentTurret.GetComponent<TurretPlacement>().OnValidTerrain();
    }

    void PlaceTurret()
    {
        // Choose turret
        if (!selectedTurret && Input.GetKeyDown(KeyCode.Alpha1) && gameControllerScript.GetMoney() >= 20)
        {
            if (highlightedTurret != null)
            {
                UnhighlightTurret();
            }
            currentTurret = Instantiate(turret1, placementPointer.transform.position, Quaternion.identity);
            placementPointerRenderer.enabled = false;
            selectedTurret = true;
            GameController.AddMoney(-20);
        }
        else if (!selectedTurret && Input.GetKeyDown(KeyCode.Alpha2) && gameControllerScript.GetMoney() >= 40)
        {
            if (highlightedTurret != null)
            {
                UnhighlightTurret();
            }
            currentTurret = Instantiate(turret2, placementPointer.transform.position, Quaternion.identity);
            placementPointerRenderer.enabled = false;
            selectedTurret = true;
            GameController.AddMoney(-40);
        }
        // TODO: handle other keys for other turrets in the future

        // Confirm selection
        if (selectedTurret && Input.GetMouseButton(0) && currentTurret.GetComponent<TurretPlacement>().Placeable)
        {
            lastPlacedTime = Time.time;

            // Disable rendering of turret's range indicator
            if (Physics.Raycast(currentTurret.transform.position, Vector3.down, out var hit))
            {
                var ground = hit.transform.gameObject;
                if (ground.CompareTag("Placeable"))
                {
                    var script = ground.GetComponent<PlaceableTerrainScript>();
                    script.isPlaceable = false;
                    script.turret = currentTurret;
                    currentTurret.GetComponent<TurretPlacement>().SetPlaced(true);
                    currentTurret.GetComponent<TurretPlacement>().SetTile(script);
                    var position = ground.transform.position;
                    currentTurret.transform.position = new Vector3(position.x, position.y + 0.5f, position.z);
                    currentTurret.tag = "Turret";
                    Transform rangeIndicators = currentTurret.transform.Find("RangeIndicators");
                    Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
                    Transform attackIndicator = rangeIndicators.transform.Find("AttackRange");
                    placementIndicator.GetComponent<Renderer>().enabled = false;
                    attackIndicator.GetComponent<Renderer>().enabled = false;

                    // Put the turret into the turret parent object
                    currentTurret.transform.parent = turretParent.transform;
                    selectedTurret = false;

                    // Render the placement cursor again
                    placementPointerRenderer.enabled = true;
                }
            }
        }

        // Cancel selection
        if (selectedTurret && Input.GetKey(KeyCode.Q))
        {
            Destroy(currentTurret);
            selectedTurret = false;
            placementPointerRenderer.enabled = true;
        }
    }

    // Opens a menu for controlling a selected turret
    void HighlightTurret()
    {
        if (Time.time - lastPlacedTime >= highlightDelay)
        {
            if (!selectedTurret && Input.GetMouseButton(0) && hoveredTurret != null && highlightedTurret == null)
            {
                // Lock Cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                // Hide the placement cursor
                placementPointerRenderer.enabled = false;

                Transform rangeIndicators = hoveredTurret.transform.Find("RangeIndicators");
                Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
                Transform attackIndicator = rangeIndicators.transform.Find("AttackRange");
                placementIndicator.GetComponent<Renderer>().enabled = true;
                attackIndicator.GetComponent<Renderer>().enabled = true;
                // highlight the new turret
                highlightedTurret = hoveredTurret;
                // enable the context menu for a highlighted turret
                highlightedTurretUIScript.SetUIActive(true, highlightedTurret);
            }
        }
        // Unhighlight a turret if we have a highlighted turret and we click on the ground with nothing
        if (Input.GetKeyDown(KeyCode.Q) && highlightedTurret != null)
        {
            UnhighlightTurret();
        }
    }

    void CheckForAbilityCast()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CastAbility(0);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            CastAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CastAbility(2);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            CastAbility(3);
        }
    }

    void CastAbility(int index)
    {
        // Ability must be exist in a spell slot to be used
        if (learnedAbilities.Count > index)
        {
            Constants.Ability ability = learnedAbilities[index];
            int mana = gameControllerScript.GetMana();
            switch (ability)
            {
                case Constants.Ability.EXPLOSION:
                    if (mana < explosionManaCost)
                    {
                        break;
                    }
                    gameControllerScript.AddMana(-explosionManaCost);
                    GameObject explosion = Instantiate(ExplosionSpell, placementPointer.transform.position,
                            placementPointer.transform.rotation);
                    AbilityProperties explosionProperties = explosion.GetComponent<AbilityProperties>();
                    explosionProperties.AddAbilityEffect(Constants.AbilityEffect.DAMAGE, explosionDamage);
                    explosionProperties.SetRadius(explosionRadius);
                    explosionProperties.SetSpellDuration(1f);
                    explosionProperties.Activate();
                    break;
                case Constants.Ability.BLIZZARD:
                    if (mana < blizzardManaCost)
                    {
                        break;
                    }
                    gameControllerScript.AddMana(-blizzardManaCost);
                    GameObject blizzard = Instantiate(BlizzardSpell, placementPointer.transform.position,
                            placementPointer.transform.rotation);
                    AbilityProperties blizzardProperties = blizzard.GetComponent<AbilityProperties>();
                    blizzardProperties.AddAbilityEffect(Constants.AbilityEffect.SLOW, blizzardSlow);
                    blizzardProperties.AddAbilityEffect(Constants.AbilityEffect.DAMAGE, blizzardDamage);
                    blizzardProperties.SetRadius(blizzardRadius);
                    blizzardProperties.SetSpellDuration(4f);
                    blizzardProperties.Activate();
                    break;
                default:
                    break;
            }
        }
    }

    public void LearnAbility(Constants.Ability ability)
    {
        // Only learn an ability once
        if (learnedAbilities.IndexOf(ability) == -1)
        {
            Debug.Log($"Learned ability {ability}");
            learnedAbilities.Add(ability);
        }
    }

    public void UnhighlightTurret()
    {
        if (highlightedTurret == null)
        {
            return;
        }
        // Unlock Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Show the placement cursor again
        placementPointerRenderer.enabled = true;

        // disable the context menu for a highlighted turret
        highlightedTurretUIScript.SetUIActive(false, highlightedTurret);

        Transform rangeIndicators = highlightedTurret.transform.Find("RangeIndicators");
        Transform placementIndicator = rangeIndicators.transform.Find("PlacementRange");
        Transform attackIndicator = rangeIndicators.transform.Find("AttackRange");
        placementIndicator.GetComponent<Renderer>().enabled = false;
        attackIndicator.GetComponent<Renderer>().enabled = false;
        highlightedTurret = null;
    }

    // Gets the turret that the pointer is currently hovering over
    public void SetHoveredTurret(GameObject turret)
    {
        hoveredTurret = turret;
    }

    public GameObject GetHoveredTurret()
    {
        return hoveredTurret;
    }

    public GameObject GetHighlightedTurret()
    {
        return highlightedTurret;
    }

}
