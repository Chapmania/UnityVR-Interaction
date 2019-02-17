using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;


[RequireComponent(typeof(SteamVR_TrackedController))]
public class EyeHandInteraction : Interactor
{

    //public variables 
    public Transform camerarig = null;
    public GameObject cursor_obj = null;

    //private
    Buttons button = Buttons.Trigger;
    Selector select = Selector.EyeHand;

    RaycastHit hit;
    float inital_eyehand = 0.0f;
    float inital_eyehit = 0.0f;

    // Use this for initialization
    void Start () {

        SetButton(button);
        SetController(GetComponent<SteamVR_TrackedController>(), button);
        SetColor(select);

    }

    protected override void Focus()
    {
        base.Focus();
        Vector3 eyepos = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye);
        Vector3 eye = (camerarig.localRotation * (Vector3.Scale(eyepos, camerarig.localScale))) + camerarig.localPosition;
        if (Physics.Raycast(eye, transform.position - eye, out hit))
        {
            Debug.Log(hit.collider.name);
            Vector3 hit_position = hit.point;
            inital_eyehand = (transform.position - eye).magnitude;
            inital_eyehit = (hit_position - eye).magnitude;
            SetCursorPosition(hit_position);
            SetCursorRotation(transform.rotation);
            cursor_obj.transform.position = hit.point;
            SetGrabbable(hit.transform);
        }
        else
        {
            Debug.Log("no Object");
            SetGrabbable(null);
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
        {
            Focus();
        }
        else if (IsGrabing())
        {
            Vector3 eyepos = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye);
            Vector3 eye = (camerarig.localRotation * (Vector3.Scale(eyepos, camerarig.localScale))) + camerarig.localPosition;
           
            // Do the ratio math here...
            Vector3 pos = Vector3.zero;
            Vector3 curr_eyehand = transform.position - eye;
            float new_length = curr_eyehand.magnitude * inital_eyehit / inital_eyehand;
            Vector3 newcurr_cursor = curr_eyehand.normalized * new_length;
            pos = newcurr_cursor + eye;
            cursor_obj.transform.position = pos;
            SetCursorPosition(pos);
            SetCursorRotation(transform.rotation);
        }

    }
}
