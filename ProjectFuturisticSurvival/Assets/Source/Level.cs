using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public enum Status
    {
        Loading,
        Play
    }

    public Object[] resources;

    public Status status = Status.Loading;

    public WorldRegionManager regionManager;

	// Use this for initialization
	void Start () {
        this.resources = new Object[] {
            null,//0
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,//10
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,//20
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,//30
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,//40
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,//50
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            Resources.Load<Material>("Materials/default"),//60 Materials
            Resources.Load<Material>("Materials/sand")
        };

        if(this.regionManager == null)
        {
            this.regionManager = GameObject.FindObjectOfType<WorldRegionManager>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(this.status == Status.Loading)
        {
            //If at least one region is generated and has collisions, level is loaded
            for(int i = 0; i < WorldRegionManager.MAPSIZE; i++)
            {
                for (int j = 0; j < WorldRegionManager.MAPSIZE; j++)
                {
                    if(this.regionManager.regions[i,j] != null
                        && this.regionManager.regions[i, j].status != WorldRegion.Status.Ungenerated
                        && this.regionManager.regions[i, j].collisionObject.GetComponent<MeshCollider>() != null)
                    {
                        this.status = Status.Play;
                    }
                }
            }
        }
	}
}
