using UnityEngine;
using System.Collections;

public class TestTerrainEditor : MonoBehaviour {

    float[,] heights;
    Terrain _terrain;

    // Use this for initialization
    void Start() {

        _terrain = Terrain.activeTerrain;
        heights = _terrain.terrainData.GetHeights(0, 0, _terrain.terrainData.heightmapWidth, _terrain.terrainData.heightmapHeight);

        for (int i = 0; i < _terrain.terrainData.heightmapWidth; i++)
        {
            for (int j = 0; j < _terrain.terrainData.heightmapHeight; j++)
            {
                if (i < 200 && j < 200)
                {
                    heights[i, j] = 1;
                }  
                     

            }
        }
      
        _terrain.terrainData.SetHeights(0, 0, heights);
            

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
