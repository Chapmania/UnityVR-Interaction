using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DockPin : MonoBehaviour
{
    public float local_break_dist;  // The distance the controller moves in the local space after snapping before unsnapping.
    Transform controller = null;    // The controller during grab.

    // The local controller position the moment after the snap. 
    // You need this to test when stress on the joint exceeds local_break_dist.
    // You also need this to rotate the pin joint after the snap.
    protected Vector3 grabpt_after_snap = Vector3.zero;

    // The dock we might be snapped to.
    protected DockPinTarget othdock = null;

    // We save the hierarchy values prior to snapping so we can restore them if the snap breaks.
    Transform parent_save = null;

    private void OnTriggerEnter(Collider other)
    {
        // Reject collisions when not being grabbed and when already snapped.
        if (controller == null || othdock != null )
            return;

        // Look to see if we collided with a pin target.
        othdock = other.gameObject.GetComponent<DockPinTarget>();
        if (othdock != null)
        {
            // Do an initial alignment
            Snap(othdock.transform);

        }
    }

    void Snap( Transform othdock)
    {
        #region STUDENT_CODE

        // Save the wheels current parent and snap the wheel to othdock
        // Note: After snapping make sure the wheel and the dock are not offset.
        parent_save = othdock;
        transform.SetParent(othdock, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        #endregion

        // Save the initial controller grab offset to be used to measure ongoing stress on the dock.
        grabpt_after_snap = transform.InverseTransformPoint(controller.position);

        RotatePin();
    }

    void Unsnap()
    {
        #region STUDENT_CODE

        // Unsnap the wheel from the dock
        transform.SetParent(parent_save);
        #endregion

        othdock = null;
    }

    bool TestBreakSnap()
    {
        bool break_snap = false;

        #region STUDENT_CODE
        // Write a test to determine if the controller has moved past local_break_dist in the local frame breaking the snap
        Vector3 error = grabpt_after_snap - transform.InverseTransformPoint(controller.position);
        if (error.magnitude > local_break_dist)
            break_snap = true;
        #endregion

        return break_snap;
    }

    void RotatePin()
    {
        #region STUDENT_CODE
        Vector3 curr = transform.InverseTransformPoint(controller.position);
        Vector3 child_pos = grabpt_after_snap;
        child_pos.z = 0f;
        curr.z = 0f;
        Quaternion delta = Quaternion.FromToRotation(child_pos, curr);
        transform.localRotation = transform.localRotation * delta;
        #endregion
    }

    // We provide a public read-only property to keep the internal variable private.
    public bool IsDocked
    {
        get { return othdock != null; }
    }

    public void Grab(Transform c)
    {
        // Snaps can occur only when a controller is grabbing. 
        controller = c;
    }

    public void Release()
    {
        // Snapping is disabled when controller is released.
        controller = null;
    }

    // This co-routine is running when the DockPin is snapped and grabbed by a controller.
    // We use a co-routine instead of the Update() method to avoid having Update() being called on every part every frame.
    public void UpdateDocked()
    {
        if (controller != null && othdock != null)
        {
            // Get current controller locally to measure stress on the joint. 
            if ( TestBreakSnap() )
                Unsnap();
            else
                RotatePin();
        }
    }
}

