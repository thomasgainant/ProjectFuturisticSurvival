using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public Object[] resources;

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
            Resources.Load<Material>("Materials/default")//60 Materials
        };
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
