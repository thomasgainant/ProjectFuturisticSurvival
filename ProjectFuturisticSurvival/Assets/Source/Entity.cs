using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public Level level;

	// Use this for initialization
	void Start () {
        this.init();
	}

    public virtual void init()
    {
        if(this.level == null)
        {
            this.level = GameObject.FindObjectOfType<Level>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        this.update();
	}

    protected virtual void update()
    {

    }
}
