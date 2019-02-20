using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedController))]
public class DirectInteraction : Interactor
{

    Buttons button = Buttons.Trigger;
    Selector select = Selector.Direct;


    void Start()
    {
        SetButton(button);
        SetController(GetComponent<SteamVR_TrackedController>(), button);
        SetColor(select);
    }


    private void OnTriggerEnter(Collider other)
    {
        SetGrabbable(other.transform);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsGrabing() == false)
            SetGrabbable(null);
    }

    protected override void DoButton(Buttons b, bool v)
    {
        base.DoButton(b,v);
    }

    protected override void Focus()
    {

        base.Focus();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SetCursorPosition(transform.position);
        SetCursorRotation(transform.rotation);

    }

}
