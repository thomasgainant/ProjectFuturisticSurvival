using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRegion : Entity {

    public enum Status
    {
        Ungenerated,
        Generated,
        Displayed,
        Dirty,
        DirtyCollision
    }

    public static int REGIONSIZE = 64;//Number of WorldRegionPoints - Always multiple of 2
    public static float REGIONPRECISION = 1.0f;//World space between two WorldRegionPoints at lod 1

    public WorldRegion.Status status = Status.Ungenerated;

    public bool computingDisplay = false;
    public bool computingCollision = false;

    public float[,] heightmap;
    public WorldRegionPoint[,] points;
    public int lod = 1; //Always multiple of 2

    public GameObject collisionObject;

	public override void init()
    {
        base.init();

        this.heightmap = new float[REGIONSIZE, REGIONSIZE];
        for (int i = 0; i < REGIONSIZE; i++)
        {
            for (int j = 0; j < REGIONSIZE; j++)
            {
                this.heightmap[i, j] = 0.0f;
            }
        }

        this.loadHeightmapFromNoise(0, 0);

        this.collisionObject = new GameObject("Collision");
        this.collisionObject.transform.position = this.gameObject.transform.position;
        this.collisionObject.transform.SetParent(this.gameObject.transform);

        StartCoroutine(this.generate());
    }

    protected override void update()
    {
        base.update();

        if(this.status == Status.Dirty && !this.computingDisplay)
        {
            StartCoroutine(this.display(this.lod));
        }

        if (this.status == Status.DirtyCollision && !this.computingCollision)
        {
            StartCoroutine(this.display(this.lod));
        }
    }

    private IEnumerator generate()
    {
        this.points = new WorldRegionPoint[REGIONSIZE,REGIONSIZE];
        for(int i = 0; i < REGIONSIZE; i++)
        {
            for(int j = 0; j < REGIONSIZE; j++)
            {
                GameObject pointObj = new GameObject("WorldRegionPoint");
                pointObj.transform.position = this.gameObject.transform.position
                    + new Vector3(REGIONPRECISION * i, 0.0f, REGIONPRECISION * j);
                pointObj.transform.SetParent(this.gameObject.transform);

                WorldRegionPoint point = pointObj.AddComponent<WorldRegionPoint>();
                point.init();
                this.points[i,j] = point;

                yield return new WaitForEndOfFrame();
            }
        }

        this.status = Status.Generated;
        Debug.Log("WorldRegion "+this.ToString()+" generated");
        StartCoroutine(this.generateCollision());
        StartCoroutine(this.display(this.lod));
    }

    private IEnumerator display(int lod)
    {
        this.computingDisplay = true;

        for (int i = 0; i < REGIONSIZE - lod; i = i + lod)
        {
            for (int j = 0; j < REGIONSIZE - lod; j = j + lod)
            {
                float[] pointHeightmap = new float[] {
                    this.heightmap[i,j],
                    this.heightmap[i+lod,j],
                    this.heightmap[i,j+lod],
                    this.heightmap[i+lod,j+lod]
                };

                this.points[i, j].display(lod, pointHeightmap);
                yield return new WaitForEndOfFrame();
            }
        }

        this.computingDisplay = false;
        this.status = Status.Displayed;
        Debug.Log("WorldRegion " + this.ToString() + " displayed");
    }

    public IEnumerator generateCollision()
    {
        this.computingCollision = true;

        Mesh totalMesh = new Mesh();
        totalMesh.name = "totalCollisionMesh";

        Vector3[] vertices = new Vector3[4 * REGIONSIZE * REGIONSIZE];
        int[] tri = new int[6 * REGIONSIZE * REGIONSIZE];

        int verticesIndex = 0;
        int triIndex = 0;
        for (int i = 0; i < REGIONSIZE - 1; i++)
        {
            for (int j = 0; j < REGIONSIZE - 1; j++)
            {
                vertices[verticesIndex] = new Vector3(WorldRegion.REGIONPRECISION * i, this.heightmap[i,j], WorldRegion.REGIONPRECISION * j);
                vertices[verticesIndex + 1] = new Vector3((WorldRegion.REGIONPRECISION * i) + WorldRegion.REGIONPRECISION, this.heightmap[i+1, j], WorldRegion.REGIONPRECISION * j);
                vertices[verticesIndex + 2] = new Vector3(WorldRegion.REGIONPRECISION * i, this.heightmap[i, j+1],(WorldRegion.REGIONPRECISION * j) + WorldRegion.REGIONPRECISION);
                vertices[verticesIndex + 3] = new Vector3((WorldRegion.REGIONPRECISION * i) + WorldRegion.REGIONPRECISION, this.heightmap[i+1, j+1], (WorldRegion.REGIONPRECISION * j) + WorldRegion.REGIONPRECISION);

                verticesIndex += 4;

                tri[triIndex] = verticesIndex;
                tri[triIndex + 1] = verticesIndex + 2;
                tri[triIndex + 2] = verticesIndex + 1;

                tri[triIndex + 3] = verticesIndex + 2;
                tri[triIndex + 4] = verticesIndex + 3;
                tri[triIndex + 5] = verticesIndex + 1;

                triIndex += 6;
            }
        }

        totalMesh.vertices = vertices;
        totalMesh.triangles = tri;

        MeshCollider collider = this.collisionObject.GetComponent<MeshCollider>();
        if (collider == null)
        {
            collider = this.collisionObject.AddComponent<MeshCollider>();
        }

        collider.sharedMesh = totalMesh;

        this.computingCollision = false;
        this.status = Status.Displayed;
        yield return null;
    }

    public void loadHeightmapFromNoise(int x, int y)
    {
        for (int i = 0; i < REGIONSIZE - 1; i++)
        {
            for (int j = 0; j < REGIONSIZE - 1; j++)
            {
                this.heightmap[i,j] = Mathf.PerlinNoise(x + (i / (REGIONSIZE + 0.0f)), y + (j / (REGIONSIZE + 0.0f))) * 10.0f;
            }
        }
    }

    public void loadHeightmapFromString(string raw)
    {
        string[] rawPoints = raw.Split('|');

        int i = 0;
        int j = 0;
        foreach(string point in rawPoints)
        {
            this.heightmap[i,j] = float.Parse(point);
            j++;

            if(j >= REGIONSIZE)
            {
                j = 0;
                i++;
            }
        }
    }

    public string saveHeightmap()
    {
        string result = "";

        for (int i = 0; i < REGIONSIZE - 1; i++)
        {
            for (int j = 0; j < REGIONSIZE - 1; j++)
            {
                result += ""+this.heightmap[i,j];

                if(!(i == REGIONSIZE - 1 && j == REGIONSIZE - 1))
                {
                    result += "|";
                }
            }
        }

        return result;
    }
}
