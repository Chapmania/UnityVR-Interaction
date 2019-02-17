using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockPinInteraction : MonoBehaviour {

    public float break_distance;
    Transform controller = null;

    Vector3 grab_after_snap = Vector3.zero;
    DockPinTarget _dock = null;
    Transform parent_saved_data = null;

    private void OnTriggerEnter(Collider other)
    {
        if (controller == null || _dock != null)
            return;
        _dock = other.gameObject.GetComponent<DockPinTarget>();
        if (_dock != null)
            SnapDock(other.transform);
    }

    void SnapDock( Transform otherdock)
    {
        parent_saved_data = otherdock;
        transform.SetParent(otherdock, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        grab_after_snap = transform.InverseTransformPoint(controller.position);
        
    }
    void RemoveDock()
    {
        transform.SetParent(parent_saved_data);
        _dock = null;
    }
    bool TestBreakLimit()
    {
        bool break_snap = false;
        Vector3 error = grab_after_snap - transform.InverseTransformPoint(controller.position);
        if (error.magnitude > break_distance)
            break_snap = true;

        return break_snap;
    }
    void RotatePin()
    {
        Vector3 curr = transform.InverseTransformPoint(controller.position);
        Vector3 child_pos = grab_after_snap;
        child_pos.z = 0f;
        curr.z = 0f;
        Quaternion delta = Quaternion.FromToRotation(child_pos, curr);
        transform.localRotation = transform.localRotation * delta;
    }

    public bool IsDocked
    {
        get { return _dock != null; }
    }
    public void Grab( Transform _controller) { controller = _controller; }
    public void SetContollerPosition(Vector3 cursor_pos) { controller.position = cursor_pos; }
    public void Release() { controller = null; }

    public void UpdateDock()
    {
        if (controller != null && _dock != null)
        {
            // Get current controller locally to measure stress on the joint. 
            if (TestBreakLimit())
                RemoveDock();
            else
                RotatePin();
        }
    }
}
