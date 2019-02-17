using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedController))]
public class HeadRayInteraction : Interactor
{
    public GameObject cursor_obj = null;
    public Transform head = null;

    Buttons button = Buttons.Trigger;
    Selector select = Selector.Head;

    RaycastHit hit;
    Vector3 cursor_pos = Vector3.zero;
    Vector3 hit_point_atgrab = Vector3.zero;

    // Use this for initialization
    void Start()
    {

        SetButton(button);
        SetController(GetComponent<SteamVR_TrackedController>(), button);
        SetColor(select);
    }

    protected override void Focus()
    {
        base.Focus();
        if (Physics.Raycast(head.position, head.forward, out hit))
        {
            hit_point_atgrab = head.InverseTransformPoint(hit.point);
            SetCursorPosition(hit.point);
            SetCursorRotation(transform.rotation);
            cursor_obj.transform.position = hit.point;
            SetGrabbable(hit.transform);
        }
        else
        {
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
            cursor_pos = head.TransformPoint(hit_point_atgrab);
            cursor_obj.transform.position = cursor_pos;
            SetCursorPosition(cursor_pos);
            SetCursorRotation(transform.rotation);
        }
    }
}