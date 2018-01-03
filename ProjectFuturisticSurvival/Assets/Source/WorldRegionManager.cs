using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRegionManager : Entity {

    public static int MAPSIZE = 256;

    public WorldRegion[,] regions;

    public Camera mainCamera;

    public override void init()
    {
        base.init();

        this.regions = new WorldRegion[MAPSIZE, MAPSIZE];

        this.spawnRegionAt(Vector2.zero);
    }

    protected override void update()
    {
        base.update();

        for(int i = 0; i < MAPSIZE; i++)
        {
            for (int j = 0; j < MAPSIZE; j++)
            {
                //Handles LOD
                if(this.regions[i,j] != null && this.regions[i, j].status != WorldRegion.Status.Ungenerated)
                {
                    Vector3 diff = this.regions[i, j].gameObject.transform.position - this.mainCamera.gameObject.transform.position;
                    int distInRegionSize = Mathf.FloorToInt(diff.magnitude/(WorldRegion.REGIONSIZE * WorldRegion.REGIONPRECISION));
                    int newLod = distInRegionSize * 2;
                    if (newLod < 1)
                    {
                        newLod = 1;
                    }
                    else if(newLod > WorldRegion.REGIONSIZE)
                    {
                        newLod = WorldRegion.REGIONSIZE;
                    }

                    if(newLod != this.regions[i,j].lod){
                        this.regions[i, j].setLod(newLod);
                    }
                }
            }
        }
    }

    public WorldRegion spawnRegionAt(Vector2 coors)
    {
        GameObject worldRegionObj = new GameObject("WorldRegion");
        worldRegionObj.transform.position = new Vector3(WorldRegion.REGIONSIZE * WorldRegion.REGIONPRECISION * coors.x, 0.0f, WorldRegion.REGIONSIZE * WorldRegion.REGIONPRECISION * coors.y);

        WorldRegion worldRegion = worldRegionObj.AddComponent<WorldRegion>();
        worldRegion.lod = WorldRegion.REGIONSIZE;
        worldRegion.worldRegionCoors = coors;

        this.regions[Mathf.FloorToInt(coors.x), Mathf.FloorToInt(coors.y)] = worldRegion;

        return worldRegion;
    }
}
