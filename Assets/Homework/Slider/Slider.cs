using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public Transform controller;

    public float min_y = 0.7379591f;
    public float max_y = 3.49f;

    Vector3 child_pos = Vector3.zero;

    private void Start()
    {

        #region STUDENT_CODE

        // Convert the controllers position into the slider handles local frame.
        // Use Unitys transform.InverseTransformPoint method for this.
        child_pos = transform.InverseTransformPoint(controller.position);

        #endregion
    }

    private void Update()
    {
        Vector3 local_pos = transform.localPosition;
        float slider_val = 0f;

        #region STUDENT_CODE

        // Convert the controllers current position into the slider handles local frame, calculate the change in position
        // along the movement axis and add it to local_pos and constrain between miny and maxy
        Vector3 curr = transform.InverseTransformPoint(controller.position);
        float y_val = local_pos.y;
        if (curr.y + y_val < min_y)
            y_val = min_y;
        else if (curr.y + y_val > max_y)
            y_val = max_y;
        else
            y_val += curr.y;

        Vector3 new_position = local_pos;
        new_position.y = y_val;
        local_pos = new_position;
        #endregion

        transform.localPosition = local_pos;

        #region STUDENT_CODE

        // Compute the slider value from 0f to 1f
        if (curr.y + y_val < min_y)
            slider_val = 0f;
        else if (curr.y + y_val > max_y)
            slider_val = 1f;
        else
            slider_val = (curr.y + y_val) / max_y;

        #endregion

        Debug.Log("Slider value:" + slider_val);

    }
}
