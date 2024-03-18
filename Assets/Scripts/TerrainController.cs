using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public GameObject terrain;
    public GameObject end;

    public static int[][] levelMap = new[]
    {
        new int[] {1,1,1,1,1,1,1,1,1,1},
        new int[] {1,1,1,1,1,1,1,1,1,1},
        new int[] {1,1,1,1,1,1,1,1,1,1},
        new int[] {1,1,1,1,0,0,0,0,0,3},
        new int[] {1,1,1,1,0,1,1,0,1,1},
        new int[] {1,1,0,0,0,1,1,0,1,1},
        new int[] {2,0,0,1,0,0,0,0,1,1},
        new int[] {1,1,1,1,1,1,1,1,1,1},
        new int[] {1,1,1,1,1,1,1,1,1,1},
        new int[] {1,1,1,1,1,1,1,1,1,1}
    };
    
    public static List<List<GameObject>> terrainList = new List<List<GameObject>>();
    
    // Start is called before the first frame update
    void Start()
    {
        terrain.transform.localScale = new Vector3(10f,0.5f,10f);
        var placeable = this.transform.Find("Placeable");
        for (int i = 0; i < levelMap.Length; i++)
        {
            var row = new List<GameObject>();
            for (int j = 0; j < levelMap[i].Length; j++)
            {
                if (levelMap[i][j] == 1)
                {
                    terrain.name = $"Terrain_{i}_{j}";
                    terrain.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                    row.Add(Instantiate(terrain, placeable));
                } else if (levelMap[i][j] == 2)
                {
                    var enemyStart = GameObject.Find("EnemyStart");
                    enemyStart.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                    row.Add(enemyStart);
                } else if (levelMap[i][j] == 3)
                {
                    end.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                    row.Add(Instantiate(end));
                }
            }
            terrainList.Add(row);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
