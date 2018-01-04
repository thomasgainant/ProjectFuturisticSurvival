using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity {

    public float height = 2.0f;
    public float walkSpeed = 3.0f;

    public override void init()
    {
        base.init();
    }

    protected override void update()
    {
        base.update();

        if (this.level.status == Level.Status.Loading)
        {
            Rigidbody rigidBody = this.gameObject.GetComponent<Rigidbody>();
            if(rigidBody != null)
            {
                rigidBody.isKinematic = true;
                Destroy(rigidBody);
            }
        }
        else if (this.level.status == Level.Status.Play)
        {
            Rigidbody rigidBody = this.gameObject.GetComponent<Rigidbody>();
            if (rigidBody == null)
            {
                Vector3 terrainHeight = this.level.regionManager.getTerrainHeightAt(this.gameObject.transform.position);
                Debug.Log("terrainHeight: "+terrainHeight);
                this.gameObject.transform.position = terrainHeight + Vector3.up * (this.height/ 2.0f);

                rigidBody = this.gameObject.AddComponent<Rigidbody>();
                rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

                CapsuleCollider collider = this.gameObject.AddComponent<CapsuleCollider>();
                collider.radius = 0.35f;
                collider.height = this.height;
            }
        }
    }
}
