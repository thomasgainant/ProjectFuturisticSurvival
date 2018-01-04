using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    public Camera eye;

    public override void init()
    {
        base.init();

        this.eye = this.gameObject.transform.Find("Main Camera").gameObject.GetComponent<Camera>();
    }

    protected override void update()
    {
        base.update();

        float currentSpeed = this.walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= 2.0f;
        }

        if(Input.GetKey(KeyCode.Z))
        {
            this.gameObject.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed = this.walkSpeed / 2.0f;
            this.gameObject.transform.Translate(Vector3.back * currentSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            this.gameObject.transform.Translate(Vector3.left * currentSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(Vector3.right * currentSpeed * Time.deltaTime, Space.Self);
        }

        if(Input.GetAxis("Mouse X") != 0.0f)
        {
            this.gameObject.transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X") * Time.deltaTime * 25.0f, 0.0f), Space.World);
        }
        if (Input.GetAxis("Mouse Y") != 0.0f)
        {
            this.eye.gameObject.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Time.deltaTime * 25.0f, 0.0f, 0.0f), Space.Self);
        }
    }
}
