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

    public static int[][] levelMap = new[]
    {
        new int[] {7,7,6,6,7,6,6,7,7,6},
        new int[] {6,7,6,7,4,7,5,4,4,5},
        new int[] {5,4,5,4,1,6,1,1,1,1},
        new int[] {4,5,4,1,0,0,0,0,0,3},
        new int[] {1,4,1,1,0,1,4,0,1,1},
        new int[] {1,1,0,0,0,1,1,0,1,4},
        new int[] {2,0,0,1,0,0,0,0,1,5},
        new int[] {4,1,1,5,1,1,5,1,4,6},
        new int[] {5,4,5,4,6,7,6,7,7,6},
        new int[] {7,6,7,6,7,6,7,6,6,7}
    };

    public static List<List<GameObject>> terrainList = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        terrain.transform.localScale = new Vector3(10f, 0.5f, 10f);
        var placeable = this.transform.Find("Placeable");
        for (int i = 0; i < levelMap.Length; i++)
        {
            var row = new List<GameObject>();
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
                        var enemyStart = GameObject.Find("EnemyStart");
                        enemyStart.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        break;
                    case 3:
                        end.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                        Instantiate(end, placeable);
                        break;
                    case 4:
                        vegitatedTerrain[0].tag = "Unplaceable";
                        vegitatedTerrain[0].transform.position = new Vector3(-j * 10 + 45, 0, i * 10 - 45);
                        Instantiate(vegitatedTerrain[0]);
                        break;
                    case 5:
                        vegitatedTerrain[1].tag = "Unplaceable";
                        vegitatedTerrain[1].transform.position = new Vector3(-j * 10 + 45, 0, i * 10 - 45);
                        Instantiate(vegitatedTerrain[1]);
                        break;
                    case 6:
                        vegitatedTerrain[2].tag = "Unplaceable";
                        vegitatedTerrain[2].transform.position = new Vector3(-j * 10 + 45, 0, i * 10 - 45);
                        Instantiate(vegitatedTerrain[2]);
                        break;
                    case 7:
                        vegitatedTerrain[3].tag = "Unplaceable";
                        vegitatedTerrain[3].transform.position = new Vector3(-j * 10 + 45, 0, i * 10 - 45);
                        Instantiate(vegitatedTerrain[3]);
                        break;

                }
            }
            terrainList.Add(row);
        }
    }
}
