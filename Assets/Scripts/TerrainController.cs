using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public GameObject terrain;
    public GameObject end;

    private int[][] levelMap = new[]
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
    
    // Start is called before the first frame update
    void Start()
    {
        terrain.transform.localScale = new Vector3(10f,0.5f,10f);
        var placeable = this.transform.Find("Placeable");
        for (int i = 0; i < levelMap.Length; i++)
        {
            for (int j = 0; j < levelMap[i].Length; j++)
            {
                if (levelMap[i][j] == 1)
                {
                    terrain.tag = "Placeable";
                    terrain.name = $"Terrain_{i}_{j}";
                    terrain.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                    Instantiate(terrain, placeable);
                } else if (levelMap[i][j] == 2)
                {
                    var enemyStart = GameObject.Find("EnemyStart");
                    enemyStart.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                    
                } else if (levelMap[i][j] == 3)
                {
                    end.transform.position = new Vector3(-j * 10 + 45, 0.5f, i * 10 - 45);
                    Instantiate(end, placeable);
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
