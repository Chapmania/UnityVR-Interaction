using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbing : MonoBehaviour
{
    public Transform controller;

    Vector3 child_pos = Vector3.zero;
    Quaternion child_rot = Quaternion.identity;

    Vector3 world_pos = Vector3.zero;
    Quaternion world_rot = Quaternion.identity;

    private void Start()
    {
        world_pos = transform.position;
        world_rot = transform.rotation;

        #region STUDENT_CODE

        // Convert world pos, rot and scale values to be child values of controller.
        // Note: The controller is attached to the world and its scale is not 1f 
        child_rot = Quaternion.Inverse(controller.rotation) * world_rot;
        Vector3 invert_scale = new Vector3(1.0f / controller.localScale.x, 1.0f / controller.localScale.y, 1.0f / controller.localScale.z);
        child_pos = world_pos - controller.position;
        child_pos = Quaternion.Inverse(controller.rotation) * child_pos;
        child_pos = Vector3.Scale(child_pos, invert_scale);
        #endregion
    }

    private void Update()
    {
        #region STUDENT_CODE

        // Convert child_pos and child_rot back to world world_pos and world_rot by making them siblings of the controller.

        world_rot = controller.rotation * child_rot;
        world_pos = Vector3.Scale(controller.localScale, child_pos);
        world_pos = controller.rotation * world_pos;
        world_pos = world_pos + controller.position;
  
        #endregion

        transform.position = world_pos;
        transform.rotation = world_rot;
    }
}
