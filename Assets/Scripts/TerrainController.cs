using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public GameObject terrain;
    public GameObject[] vegitatedTerrain;
    public GameObject end;

    /*
    Representations:
    0 - Enemy path
    1 - Placeable tile
    2 - Enemy Start Tile
    3 - Enemy end tile
    4 - Small veg tile 1
    5 - Small veg tile 2
    6 - High veg tile 1
    7 - High veg tile 2
    8 - Shop Tile
    */
    // private static int[][] levelMap = new[]
    // {
    //     new int[] {7,6,5,4,7,6,4,5,5,6},
    //     new int[] {4,7,4,5,4,5,4,5,4,5},
    //     new int[] {5,8,5,1,1,1,1,1,1,7},
    //     new int[] {4,5,0,0,0,1,0,0,0,1},
    //     new int[] {1,1,0,1,0,1,0,1,0,1},
    //     new int[] {1,1,0,1,0,1,0,1,0,4},
    //     new int[] {2,0,0,1,0,0,0,1,0,5},
    //     new int[] {4,1,1,1,1,1,1,1,0,3},
    //     new int[] {5,4,1,1,1,1,1,1,1,6},
    //     new int[] {7,5,6,4,5,6,7,4,6,7}
    // };
    // Start is called before the first frame update
    private void ClearLevel()
    {
        var turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (var turret in turrets)
        {
            Destroy(turret);
        }
        var placeable = this.transform.Find("Placeable");
        foreach (Transform child in placeable)
        {
            Destroy(child.gameObject);
        }
        var unplaceable = this.transform.Find("Unplaceable");
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
        terrain.transform.localScale = new Vector3(10f, 0.5f, 10f);
        var placeable = this.transform.Find("Placeable");
        var unplaceable = this.transform.Find("Unplaceable");
        for (int i = 0; i < levelMap.Length; i++)
        {
            for (int j = 0; j < levelMap[i].Length; j++)
            {
                switch (levelMap[i][j])
                {
                    case 1:
                        terrain.tag = "Placeable";
                        terrain.name = $"Terrain_{i}_{j}";
                        terrain.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(terrain, placeable);
                        break;
                    case 2:
                        var enemyStart = GameObject.Find("LevelManager");
                        enemyStart.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        break;
                    case 3:
                        end.tag = "Unplaceable";
                        end.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(end, unplaceable);
                        break;
                    case 4:
                        vegitatedTerrain[0].tag = "Unplaceable";
                        vegitatedTerrain[0].transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(vegitatedTerrain[0], unplaceable);
                        break;
                    case 5:
                        vegitatedTerrain[1].tag = "Unplaceable";
                        vegitatedTerrain[1].transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(vegitatedTerrain[1], unplaceable);
                        break;
                    case 6:
                        vegitatedTerrain[2].tag = "Unplaceable";
                        vegitatedTerrain[2].transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(vegitatedTerrain[2], unplaceable);
                        break;
                    case 7:
                        vegitatedTerrain[3].tag = "Unplaceable";
                        vegitatedTerrain[3].transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(vegitatedTerrain[3], unplaceable);
                        break;
                    case 8:
                        vegitatedTerrain[4].tag = "ShopTile";
                        vegitatedTerrain[4].transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(vegitatedTerrain[4], unplaceable);
                        break;

                }
            }
        }
    }
}
