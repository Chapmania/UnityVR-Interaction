using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrabbing : MonoBehaviour {

    public Transform controller;

    Vector3 child_pos = Vector3.zero;
    Quaternion child_rot = Quaternion.identity;
    Vector3 child_scale = Vector3.one;

    Vector3 world_pos = Vector3.zero;
    Quaternion world_rot = Quaternion.identity;
    Vector3 world_scale = Vector3.one;

    Vector3 camera_start_pos = Vector3.zero;
    Quaternion camera_start_rot = Quaternion.identity;
    Vector3 camera_start_scale = Vector3.one;

    private void Start()
    {
        camera_start_pos = transform.localPosition;
        camera_start_rot = transform.localRotation;
        camera_start_scale = transform.localScale;

        #region STUDENT_CODE

        // The world always starts at its own origin which is reflected in the initial pos, rot and scale values.
        // Convert world_pos, world_rot and world_scale values to be child values of controller.
        // Note: The controller is attached to the world and its scale is not 1f 

        child_rot = Quaternion.Inverse(controller.localRotation) * world_rot;
        Vector3 invert_scale = new Vector3(1.0f / controller.localScale.x, 1.0f / controller.localScale.y, 1.0f / controller.localScale.z);
        child_pos = world_pos - controller.localPosition;
        child_pos = Quaternion.Inverse(controller.localRotation) * child_pos;
        child_pos = Vector3.Scale(child_pos, invert_scale);


        #endregion
    }

    private void Update()
    {
        Vector3 camera_pos = Vector3.zero;
        Quaternion camera_rot = Quaternion.identity;
        Vector3 camera_scale = Vector3.one;

        #region STUDENT_CODE

        // Convert child_pos, child_rot and child_scale back to world world_pos, world_rot and world_scale 
        // effectively making them siblings of the controller.

        world_rot = controller.localRotation * child_rot;
        world_pos = Vector3.Scale(controller.localScale, child_pos);
        world_pos = controller.localRotation * world_pos;
        world_pos = controller.localPosition + world_pos;

        // The world values are where the world would be if it were being moved as a sibling object of the camera. 
        // We cannot move the world so we move the camera instead.
        // Convert the camera_pos, camera_rot and camera_scale to be a child of the world as if it had been moved to world_pos, world_rot and world_scale.
        Vector3 inverted_scale = new Vector3(1.0f / world_scale.x, 1.0f / world_scale.y, 1.0f / world_scale.z);

        camera_rot = Quaternion.Inverse(world_rot) * camera_start_rot;
        camera_pos = camera_start_pos - world_pos;
        camera_pos = Quaternion.Inverse(world_rot) * camera_pos;
        camera_pos = Vector3.Scale(inverted_scale, camera_pos);

        #endregion

        transform.localPosition = camera_pos;
        transform.localRotation = camera_rot;
        transform.localScale = camera_scale;

    }

}
