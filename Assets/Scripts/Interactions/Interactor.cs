using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : ManagerInteraction
{

    public void SetGrabbable(Transform gpos)
    {
        grabbale_object = gpos;


        if (grabbale_object != null)
        {
            if (grabbale_object.tag == "Interactive object")
                obj_type = Interaction_type.Object;
            else if (grabbale_object.tag == "Slider")
                obj_type = Interaction_type.Slider;
            else if (grabbale_object.tag == "Dial")
                obj_type = Interaction_type.Dial;
            else if (grabbale_object.tag == "Button")
                obj_type = Interaction_type.Button;
            else if (grabbale_object.tag == "Floor")
                obj_type = Interaction_type.Nothing;

            //Get Rigidbody of object 
            body = gpos.GetComponent<Rigidbody>();
        }
    }

    protected override void SetGrabbing(bool val)
    {
        base.SetGrabbing(val);

        if (grabbale_object != null)
        {

            if (val && obj_type == Interaction_type.Object)
            {

                Vector3 pos = cursor_position;
                Quaternion rot = cursor_rotation;

                Vector3 grabed_pos = grabbale_object.position;
                Quaternion grabed_rot = grabbale_object.rotation;

                //Convert to child pos and rot
                child_rot = Quaternion.Inverse(rot) * grabed_rot;
                child_pos = grabed_pos - pos;
                child_pos = Quaternion.Inverse(rot) * child_pos;


                isgrabbing = true;
            }
            else if (val && (obj_type == Interaction_type.Slider || obj_type == Interaction_type.Dial))
            {
                child_pos = grabbale_object.transform.InverseTransformPoint(cursor_position);
                isgrabbing = true;
            }
            else if (val && obj_type == Interaction_type.Button)
            {
                GameObject obj = GameObject.Find("Button Reaction Object");
                obj.GetComponent<MeshRenderer>().material.color = reaction_color;
                isgrabbing = true;
                pressed_button = true;
            }
            else if ((val == false || obj_type == Interaction_type.Nothing))
            {
                isgrabbing = false;
                pressed_button = false;
            }

        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        
        if (grabbale_object != null)
        {

            if (isgrabbing && obj_type == Interaction_type.Object)
            {
                Vector3 pos = cursor_position;
                Quaternion rot = cursor_rotation;

                //convert child to world and set object 
                Vector3 grabed_pos = child_pos;
                Quaternion grabed_rot = child_rot;

                grabed_rot = rot * grabed_rot;
                grabed_pos = rot * child_pos;
                grabed_pos = pos + grabed_pos;

                grabbale_object.position = grabed_pos;
                grabbale_object.rotation = grabed_rot;

            }
            else if (isgrabbing && obj_type == Interaction_type.Slider)
            {
                float local_pos = grabbale_object.localPosition.y;

                Vector3 curr = grabbale_object.InverseTransformPoint(cursor_position);
                if (curr.y + local_pos > slider_max_y)
                    local_pos = slider_max_y;
                else if (curr.y + local_pos < slider_min_y)
                    local_pos = slider_min_y;
                else
                    local_pos += curr.y;

                Vector3 new_pos = grabbale_object.localPosition;
                new_pos.y = local_pos;
                grabbale_object.localPosition = new_pos;

                if (curr.y + local_pos < slider_min_y)
                    slider_value = 0f;
                else if (curr.y + local_pos > slider_max_y)
                    slider_value = 1f;
                else
                    slider_value = (curr.y + local_pos) / slider_max_y;

            }
            else if (isgrabbing && obj_type == Interaction_type.Dial)
            {
                Quaternion local_rotation = grabbale_object.localRotation;
                Vector3 curr = grabbale_object.InverseTransformPoint(cursor_position);
                child_pos.z = 0f;
                curr.z = 0f;
                Quaternion delta = Quaternion.FromToRotation(child_pos, curr);
                local_rotation *= delta;
                grabbale_object.localRotation = local_rotation;

                Vector3 rotated_up = local_rotation * Vector3.up;
                float result = Mathf.Acos(Vector3.Dot(rotated_up, Vector3.up));
                float degrees = result * (180.0f / Mathf.PI);
                if (Vector3.Dot(rotated_up, Vector3.right) > 0)
                    degrees = 360 - degrees;
                dial_value = degrees;


            }
        }
    }
}
