using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRegion : Entity {

    public static int REGIONSIZE = 1000;//Number of WorldRegionPoints
    public static float REGIONPRECISION = 1.0f;//World space between two WorldRegionPoints at lod 1

    public WorldRegionPoint[,] points;

	public override void init()
    {
        base.init();

        this.generate();
        this.display(1);
    }

    protected override void update()
    {
        base.update();
    }

    private void generate()
    {
        this.points = new WorldRegionPoint[REGIONSIZE,REGIONSIZE];

    }

    private void display(int lod)
    {

    }
}
