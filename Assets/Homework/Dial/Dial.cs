using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    public Transform controller;

    Vector3 child_pos = Vector3.zero;

    private void Start()
    {

        #region STUDENT_CODE

        // Convert the controllers position into the dial knobs local frame.
        // Use Unitys transform.InverseTransformPoint method for this.
        child_pos = transform.InverseTransformPoint(controller.position);

        #endregion
    }

    private void Update()
    {
        Quaternion local_rot = transform.localRotation;
        float dial_val = 0f;

        #region STUDENT_CODE

        // Convert the controllers current position into the dial knobs local frame, calculate the change in rotation
        // about the knobs spin axis and add it to local_rot 
        Vector3 curr = transform.InverseTransformPoint(controller.position);
        child_pos.z = 0f;
        curr.z = 0f;
        Quaternion delta = Quaternion.FromToRotation(child_pos, curr);
        local_rot = local_rot * delta;
        #endregion

        transform.localRotation = local_rot;

        #region STUDENT_CODE

        // Compute the dial "notch" value visible at the top of the knob from 0f to 360f as it rotates 
        // away from the zero mark visible at the top of the backplate.
        Vector3 rotated_up = local_rot * Vector3.up;
        float result = Mathf.Acos(Vector3.Dot(rotated_up,Vector3.up));

        float degrees = result * (180.0f / Mathf.PI);
        if(Vector3.Dot(rotated_up,Vector3.right) > 0)
            degrees = 360 - degrees;

        dial_val = degrees;

        #endregion

        Debug.Log("Dial value:" + dial_val);

    }
}
