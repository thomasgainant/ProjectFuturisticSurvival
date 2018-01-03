using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.init();
	}

    public virtual void init()
    {

    }
	
	// Update is called once per frame
	void Update () {
        this.update();
	}

    protected virtual void update()
    {

    }
}
