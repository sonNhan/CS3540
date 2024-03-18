using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTerrainScript : MonoBehaviour
{
    public bool isPlaceable = true;
    public GameObject turret;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaceable)
        {
            this.gameObject.tag = "Placeable";
        }
        else
        {
            this.gameObject.tag = "Unplaceable";
        }
    }
}
