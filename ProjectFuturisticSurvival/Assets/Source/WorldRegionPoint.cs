using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRegionPoint : Entity {

    public override void init()
    {
        base.init();
    }

    protected override void update()
    {
        base.update();
    }

    public void display(int lod, float[] heightmap)
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = this.gameObject.AddComponent<MeshRenderer>();
        }

        MeshFilter filter = this.gameObject.GetComponent<MeshFilter>();
        if(filter == null)
        {
            filter = this.gameObject.AddComponent<MeshFilter>();
        }

        Mesh mesh = new Mesh();
        mesh.name = "WorldRegionPointMesh";

        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(0, heightmap[0], 0);
        vertices[1] = new Vector3(WorldRegion.REGIONPRECISION * lod, heightmap[1], 0);
        vertices[2] = new Vector3(0, heightmap[2], WorldRegion.REGIONPRECISION * lod);
        vertices[3] = new Vector3(WorldRegion.REGIONPRECISION * lod, heightmap[3], WorldRegion.REGIONPRECISION * lod);

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        mesh.triangles = tri;

        Vector3[] normals = new Vector3[4];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        mesh.normals = normals;

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;

        mesh.RecalculateNormals();

        filter.mesh = mesh;
        filter.sharedMesh = mesh;

        renderer.material = (Material)Instantiate(this.level.resources[60]);
    }

    public void undisplay()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Destroy(renderer);
        }

        MeshFilter filter = this.gameObject.GetComponent<MeshFilter>();
        if (filter != null)
        {
            Destroy(filter);
        }
    }
}
