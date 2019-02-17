using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Assembler : MonoBehaviour
{
    public Transform controller;
    public DockPin pin;

    Vector3 child_pos = Vector3.zero;
    Quaternion child_rot = Quaternion.identity;

    void Start()
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        if ( body != null )
        {
            body.useGravity = false;
            body.isKinematic = true;
        }

        #region STUDENT_CODE

        // Start grabbing object. Note: From Grabber homework Start()
        child_rot = Quaternion.Inverse(controller.rotation) * transform.localRotation;
        Vector3 invert_scale = new Vector3(1.0f / controller.localScale.x, 1.0f / controller.localScale.y, 1.0f / controller.localScale.z);
        child_pos = transform.localPosition - controller.position;
        child_pos = Quaternion.Inverse(controller.rotation) * child_pos;
        child_pos = Vector3.Scale(child_pos, invert_scale);
        #endregion

        pin.Grab(controller);
    }

    private void Update()
    {
        if (pin.IsDocked)
        {
            pin.UpdateDocked();
        }
        else
        {
          Vector3 world_pos = Vector3.zero;
          Quaternion world_rot = Quaternion.identity;

          #region STUDENT_CODE

          // Update grabbed object. Note: From Grabber homework Update()
          world_rot = controller.rotation * child_rot;
          world_pos = Vector3.Scale(controller.localScale, child_pos);
          world_pos = controller.rotation * world_pos;
          world_pos = world_pos + controller.position;

          #endregion

          transform.position = world_pos;
          transform.rotation = world_rot;
        }
    }
}
