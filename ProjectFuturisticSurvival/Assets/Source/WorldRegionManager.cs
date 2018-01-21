using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRegionManager : Entity {

    public enum RegionsType
    {
        Sand,
        Rock
    }

    public static int MAPSIZE = 256;
    public static float MAXHEIGHT = 2048;

    public static int REGIONSPOINTSGENERATEBUFFERSIZE = 100;
    public static int REGIONSPOINTSDISPLAYBUFFERSIZE = 100;
    public static int REGIONSGENERATIONBUFFERSIZE = 3;
    public static int REGIONSDISPLAYBUFFERSIZE = 1;

    public WorldRegion[,] regions;
    public List<WorldRegion> generatingRegions = new List<WorldRegion>();
    public List<WorldRegion> displayingRegions = new List<WorldRegion>();

    public RegionsType regionsType = RegionsType.Sand;
    public Vector2 regionsNoiseOffset = Vector2.zero;
    public float regionsNoiseMultiply = 10.0f;
    public float regionsNoiseYOffset = 0.0f;

    public Camera mainCamera;

    public override void init()
    {
        base.init();

        this.regions = new WorldRegion[MAPSIZE, MAPSIZE];

        switch(this.regionsType)
        {
            case RegionsType.Sand:
                this.regionsNoiseOffset = Vector2.zero;
                this.regionsNoiseMultiply = 10.0f;
                this.regionsNoiseYOffset = 0.0f;
                break;
            case RegionsType.Rock:
                this.regionsNoiseOffset = new Vector2(0.0f, 0.0f);
                this.regionsNoiseMultiply = 50.0f;
                this.regionsNoiseYOffset = -10.0f;
                break;
        }

        this.spawnRegionAt(Vector2.zero);
    }

    protected override void update()
    {
        base.update();

        Vector2 currentCameraCoors = new Vector2(
            Mathf.FloorToInt(this.mainCamera.gameObject.transform.position.x / (WorldRegion.REGIONSIZE * WorldRegion.REGIONPRECISION)),
            Mathf.FloorToInt(this.mainCamera.gameObject.transform.position.z / (WorldRegion.REGIONSIZE * WorldRegion.REGIONPRECISION))
        );

        for (int i = 0; i < MAPSIZE; i++)
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
                //Handles generation
                else if (this.regions[i, j] == null)
                {
                    Vector2 diff = new Vector2(i,j) - currentCameraCoors;
                    if(Mathf.FloorToInt(diff.magnitude) <= 2)
                    {
                        this.spawnRegionAt(new Vector2(i,j));
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
        worldRegion.manager = this;
        worldRegion.lod = WorldRegion.REGIONSIZE;
        worldRegion.worldRegionCoors = coors;

        this.regions[Mathf.FloorToInt(coors.x), Mathf.FloorToInt(coors.y)] = worldRegion;

        return worldRegion;
    }

    public Vector3 getTerrainHeightAt(Vector3 position)
    {
        Vector3 result = new Vector3(position.x, 0.0f, position.z);

        Ray ray = new Ray(new Vector3(position.x, MAXHEIGHT + 0.0f, position.z), Vector3.down);
        RaycastHit[] hits = Physics.RaycastAll(ray, MAXHEIGHT * 2.0f);
        if(hits.Length > 0){
            hits = Utils.SortCollisions(hits);
            result = new Vector3(position.x, hits[0].point.y, position.z);
        }

        return result;
    }
}
