using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedController))]
public class HandRayInteraction : Interactor
{
    public GameObject cursor_obj = null;
    
    bool focusing;
    Buttons button = Buttons.Trigger;
    Selector select = Selector.Hand;

    RaycastHit hit;
    Vector3 cursor_pos = Vector3.zero;
    Vector3 hit_point_atgrab = Vector3.zero;

    // Use this for initialization
	void Start () {
        focusing = false;
        SetButton(button);
        SetController(GetComponent<SteamVR_TrackedController>(), button);
        SetColor(select);
    }

    protected override void Focus()
    {
        base.Focus();
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
           
            focusing = true;
            hit_point_atgrab = transform.InverseTransformPoint(hit.point);
            SetCursorPosition(hit.point);
            SetCursorRotation(transform.rotation);
            cursor_obj.transform.position = hit.point;
            SetGrabbable(hit.transform);
        }
        else
        {
            focusing = false;
            SetGrabbable(null);
            cursor_obj.transform.position = Vector3.zero;
        }

    }

    protected override void DoButton(Buttons b, bool v)
    {
        base.DoButton(b, v);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (IsGrabing() == false)
            Focus();
        else if (IsGrabing())
        {
            cursor_pos = transform.TransformPoint(hit_point_atgrab);
            cursor_obj.transform.position = cursor_pos;
            SetCursorPosition(cursor_pos);
            SetCursorRotation(transform.rotation);
        }
    }
}
