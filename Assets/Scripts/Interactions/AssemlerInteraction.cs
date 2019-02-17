using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemlerInteraction : MonoBehaviour {

    public SteamVR_TrackedController left_controller;
    Transform controller;
    public DockPinInteraction pin;

    Vector3 child_position = Vector3.zero;
    Quaternion child_rotation = Quaternion.identity;

	// Use this for initialization
	void Start () {

        left_controller.TriggerClicked += AssemblPickup;
        
    }

    private void AssemblPickup(object sender, ClickedEventArgs e)
    {
        controller = left_controller.transform;
        child_rotation = Quaternion.Inverse(controller.rotation) * transform.localRotation;
        Vector3 invert_scale = new Vector3(1.0f / controller.localScale.x, 1.0f / controller.localScale.y, 1.0f / controller.localScale.z);
        child_position = transform.localPosition - controller.position;
        child_position = Quaternion.Inverse(controller.rotation) * child_position;
        child_position = Vector3.Scale(child_position, invert_scale);
        pin.Grab(controller);
    }

    // Update is called once per frame
    void Update () {
        if (pin != null && pin.IsDocked)
            pin.UpdateDock();
        else
        {
            Vector3 world_position = Vector3.zero;
            Quaternion world_rotation = Quaternion.identity;

            world_rotation = controller.rotation * child_rotation;
            world_position = Vector3.Scale(controller.localScale, child_position);
            world_position = controller.rotation * world_position;
            world_position = world_position + controller.position;

            transform.position = world_position;
            transform.rotation = world_rotation;
           
        }
	}
}
