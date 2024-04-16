using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    // Anything 5 or higher is a decorated unplaceable tile
    const int BLANK = -1, ROAD = 0, PLACEABLE = 1, ENEMY_START = 2, ENEMY_END = 3, SHOP = 4;

    [SerializeField]
    GameObject road, placeable, shop, enemyEnd;
    [SerializeField]
    GameObject[] decoratedTiles;

    GameObject terrain;
    List<GameObject> tiles;

    TerrainController()
    {
        tiles = new List<GameObject>();
    }

    void ClearLevel()
    {
        var turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (var turret in turrets)
        {
            Destroy(turret);
        }
        var placeable = transform.Find("Placeable");
        foreach (Transform child in placeable)
        {
            Destroy(child.gameObject);
        }
        var unplaceable = transform.Find("Unplaceable");
        foreach (Transform child in unplaceable)
        {
            if (child.name == "DirtGround")
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        var waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        foreach (var waypoint in waypoints)
        {
            Destroy(waypoint);
        }
    }

    public void InitLevel(int[][] levelMap)
    {
        ClearLevel();
        placeable.transform.localScale = new Vector3(10f, 0.5f, 10f);
        Transform placeableParent = transform.Find("Placeable");
        Transform unplaceableParent = transform.Find("Unplaceable");
        for (int i = 0; i < levelMap.Length; i++)
        {
            for (int j = 0; j < levelMap[i].Length; j++)
            {
                int tileIndex = levelMap[i][j];
                Vector3 tilePosition = new Vector3(-j * 10 + 45, 0f, i * 10 - 45);
                switch (tileIndex)
                {
                    case BLANK:
                        break;
                    case ROAD:
                        road.tag = "Unplaceable";
                        road.transform.position = tilePosition;
                        tiles.Add(Instantiate(road, unplaceableParent));
                        break;
                    case PLACEABLE:
                        placeable.tag = "Placeable";
                        placeable.name = $"Terrain_{i}_{j}";
                        tilePosition.y = 0.5f;
                        placeable.transform.position = tilePosition;
                        tiles.Add(Instantiate(placeable, placeableParent));
                        break;
                    case ENEMY_START:
                        // generate road
                        road.tag = "Unplaceable";
                        road.transform.position = tilePosition;
                        tiles.Add(Instantiate(road, unplaceableParent));
                        // set start
                        tilePosition.y = 0.5f;
                        Transform enemyStartParent = GameObject.Find("EnemyStarts").transform;
                        GameObject enemyStart = new GameObject("EnemyStart")
                        {
                            tag = "EnemyStart",
                            transform =
                            {
                                position = tilePosition,
                                parent = enemyStartParent
                            }
                        };
                        break;
                    case ENEMY_END:
                        // generate road
                        road.tag = "Unplaceable";
                        road.transform.position = tilePosition;
                        tiles.Add(Instantiate(road, unplaceableParent));
                        // generate goal on top
                        enemyEnd.tag = "Unplaceable";
                        tilePosition.y = 0.5f;
                        enemyEnd.transform.position = tilePosition;
                        Instantiate(enemyEnd, unplaceableParent);
                        break;
                    case SHOP:
                        shop.tag = "ShopTile";
                        tilePosition.y = 0.5f;
                        shop.transform.position = tilePosition;
                        tiles.Add(Instantiate(shop, unplaceableParent));
                        break;
                    default:
                        // Decorated unplaceable tiles
                        GameObject unplaceableTilePrefab = decoratedTiles[tileIndex - 5]; // sub by 5, since there are 5 set prefabs 
                                                                                          // before decorated tiles
                        unplaceableTilePrefab.tag = "Unplaceable";
                        tilePosition.y = 0.5f;
                        unplaceableTilePrefab.transform.position = tilePosition;
                        tiles.Add(Instantiate(unplaceableTilePrefab, unplaceableParent));
                        break;
                }
            }
        }
    }

    public List<GameObject> GetTiles()
    {
        return tiles;
    }

}
