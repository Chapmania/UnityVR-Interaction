using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldInteraction : MonoBehaviour {

    public SteamVR_TrackedController left_hand;
    public SteamVR_TrackedController right_hand;

    // boolean for left or right 
    bool left_active = false;
    bool right_active = false;

    ////camera start position, rotation, and scale;
    //Vector3 cam_start_position = Vector3.zero;
    //Quaternion cam_start_rotation = Quaternion.identity;
    //Vector3 cam_start_scale = Vector3.one;

    //child position, rotation, and scale;
    Vector3 child_position = Vector3.zero;
    Quaternion child_rotation = Quaternion.identity;
    Vector3 child_scale = Vector3.one;

    //world position, rotation, and scale;
    Vector3 world_position = Vector3.zero;
    Quaternion world_rotation = Quaternion.identity;
    Vector3 world_scale = Vector3.one;

    //camera position, rotation, and scale
    Vector3 camrarig_position = Vector3.zero;
    Quaternion camrarig_rotation = Quaternion.identity;
    Vector3 camrarig_scale = Vector3.one;


    Vector3 controller_pos;
    Quaternion controller_rot;
    Vector3 controller_scale;

    public bool dolly_mode = true;


    Vector3 two_hand_vec;

    void SetController( bool isOnGrab )
    {
        if ( left_active && right_active )
        {
            controller_pos = (right_hand.transform.localPosition + left_hand.transform.localPosition) * 0.5f;
            if (isOnGrab)
            {
                controller_rot = Quaternion.identity;
                controller_scale = Vector3.one;
                two_hand_vec = right_hand.transform.localPosition - left_hand.transform.localPosition;
            }
            else
            {
                Vector3 curr_two_hand_vec = right_hand.transform.localPosition - left_hand.transform.localPosition;

                Quaternion dq = Quaternion.FromToRotation(two_hand_vec, curr_two_hand_vec);
                controller_rot = dq;

                float grab_len = two_hand_vec.magnitude;
                float curr_len = curr_two_hand_vec.magnitude;

                // curr_len = ds * grab_len;
                float ds = curr_len / grab_len ;
                controller_scale = new Vector3(ds, ds, ds);
            }
        }
        else if ( left_active )
        {
            controller_pos = left_hand.transform.localPosition;
            controller_rot = left_hand.transform.localRotation;
            controller_scale = Vector3.one;
        }
        else if ( right_active )
        {
            controller_pos = right_hand.transform.localPosition;
            controller_rot = right_hand.transform.localRotation;
            controller_scale = Vector3.one;
        }

        //if ( dolly_mode )
        //{
        //    //Vector3 up = controller_rot * Vector3.up;
        //    //Quaternion dq = Quaternion.FromToRotation(up, Vector3.up);
        //    //controller_rot = dq * controller_rot;

        //    Vector3 up = Quaternion.Inverse(controller_rot) * Vector3.up;
        //    Quaternion dq = Quaternion.FromToRotation(Vector3.up, up);
        //    controller_rot = controller_rot * dq;
        //}
    }

    void ChildofController()
    {
        SetController( true );

        // Move world to CameraRig space
        Vector3 invert_scale = new Vector3(1.0f / transform.localScale.x, 1.0f / transform.localScale.y, 1.0f / transform.localScale.z);
        child_position = Vector3.zero - transform.localPosition;
        child_position = Quaternion.Inverse(transform.localRotation) * child_position;
        child_position = Vector3.Scale(child_position, invert_scale);

        child_rotation = Quaternion.Inverse(transform.localRotation) * Quaternion.identity;
        child_scale = Vector3.Scale( Vector3.one, invert_scale );

        // Move world to controller space

        child_rotation = Quaternion.Inverse(controller_rot) * child_rotation;
        invert_scale = new Vector3(1.0f / controller_scale.x, 1.0f / controller_scale.y, 1.0f / controller_scale.z);
        child_scale = Vector3.Scale(child_scale, invert_scale);


        child_position = child_position - controller_pos;
        child_position = Quaternion.Inverse(controller_rot) * child_position;
        child_position = Vector3.Scale(child_position, invert_scale);
    }

    // Use this for initialization
    void Start () {

        //Grip events for controller
        left_hand.Gripped += LeftGripped;
        left_hand.Ungripped += LeftunGripped;
        right_hand.Gripped += RightGripped;
        right_hand.Ungripped += RightunGripped;

    }

    private void LeftGripped(object sender, ClickedEventArgs e)
    {
        left_active = true;
        ChildofController();
    }

    private void LeftunGripped(object sender, ClickedEventArgs e)
    {
        left_active = false;
        if (right_active)
            ChildofController();
    }

    private void RightGripped(object sender, ClickedEventArgs e)
    {
        right_active = true;
        ChildofController();
    }

    private void RightunGripped(object sender, ClickedEventArgs e)
    {
        right_active = false;
        if (left_active)
            ChildofController();
    }

    // Update is called once per frame
    void Update () {

        if (left_active || right_active)
        {
            SetController(false);

            // Child to CameraRig
            world_scale = Vector3.Scale(controller_scale, child_scale);
            world_rotation = controller_rot * child_rotation;
            world_position = Vector3.Scale(controller_scale, child_position);
            world_position = controller_rot * world_position;
            world_position = controller_pos + world_position;


            // CameraRig to world
            world_scale = Vector3.Scale(transform.localScale, world_scale);
            world_rotation = transform.localRotation * world_rotation;
            world_position = Vector3.Scale(transform.localScale, world_position);
            world_position = transform.localRotation * world_position;
            world_position = transform.localPosition + world_position;




            // CameraRig (this) to child of world
            Vector3 inverted_scale = new Vector3(1.0f / world_scale.x, 1.0f / world_scale.y, 1.0f / world_scale.z);
            camrarig_rotation = Quaternion.Inverse(world_rotation) * transform.localRotation;
            camrarig_scale = Vector3.Scale(inverted_scale, transform.localScale);

            camrarig_position = transform.localPosition - world_position;
            camrarig_position = Quaternion.Inverse(world_rotation) * camrarig_position;
            camrarig_position = Vector3.Scale(inverted_scale, camrarig_position);



            if (dolly_mode)
            {
                Vector3 pivot = (camrarig_rotation * Vector3.Scale(camrarig_scale, controller_pos)) + camrarig_position;

                Quaternion dq = Quaternion.FromToRotation(camrarig_rotation * Vector3.up, Vector3.up);
                camrarig_rotation = dq * camrarig_rotation;

                camrarig_position = ((dq * (camrarig_position - pivot)) + pivot);
            }

            transform.localPosition = camrarig_position;
            transform.localRotation = camrarig_rotation;
            transform.localScale = camrarig_scale;
        }

    
    }
}
