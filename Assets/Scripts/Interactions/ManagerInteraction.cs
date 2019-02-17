using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInteraction : MonoBehaviour {

    //General varables
    public enum Buttons { Trigger, Pad, Padtouch, grip}
    public enum Interaction_type { Nothing, Object, Button, AssemblyObject ,Slider, Dial, Floor }
    public enum Selector { Hand , Direct, EyeHand, Head}
    Buttons Controller_button;
    SteamVR_TrackedController controller;
    protected Interaction_type obj_type;
    
    

    //Grabber Varables 
    protected bool isgrabbing; // bool to toggle grabbing of object 
    protected Transform grabbale_object; // transform of object to interact with
    protected Vector3 cursor_position; // variables for math for grabbing objects   
    protected Quaternion cursor_rotation;
    protected Vector3 child_pos;
    protected Quaternion child_rot;
    protected Rigidbody body; //ridgidbody of the object interactiing with 

    //slider//
    protected float slider_min_y = 0.7379591f;
    protected float slider_max_y = 3.49f;
    protected float slider_value = 0.0f;
    //dial//
    protected float dial_value = 0.0f;
    //button//
    protected bool pressed_button;
    protected Color reaction_color = Color.white;
    
    class ButtonEvent
    {
        public ManagerInteraction mgr;
        SteamVR_TrackedController controller;
        Buttons button;

        public ButtonEvent(SteamVR_TrackedController C, Buttons button_number)
        {
            controller = C;
            button = button_number;

            switch(button)
            {
                case Buttons.Trigger:
                    controller.TriggerClicked += Onpressed;
                    controller.TriggerUnclicked += Onunpressed;
                    break;

            }
        }
        ~ButtonEvent()
        {
            switch (button)
            {
                case Buttons.Trigger:
                    controller.TriggerClicked -= Onpressed;
                    controller.TriggerUnclicked -= Onunpressed;
                    break;
            }
        }

        public virtual void Onpressed(object sender, ClickedEventArgs e)
        {
           switch(button)
           {
                case Buttons.Trigger:
                    Debug.Log("Trigger pressed");
                    mgr.SetGrabbing(true);
                    //mgr.body.isKinematic = true;
                    //mgr.body.useGravity = false;
                    break;
           }

        }

        public virtual void Onunpressed(object sender, ClickedEventArgs e)
        {
            switch (button)
            {
                case Buttons.Trigger:
                    Debug.Log("Trigger unpressed");
                    mgr.SetGrabbing(false);
                    //mgr.body.isKinematic = false;
                    //mgr.body.useGravity = true;
                    break;

            }
        }

    }

    // Mutators 
    ButtonEvent Bevent;
    public void SetController(SteamVR_TrackedController c,Buttons b) {
        Bevent = new ButtonEvent(c, b);
        Bevent.mgr = this;
    }
    public void SetButton(Buttons b) { Controller_button = b; }
    //public void SetGrabbable(Transform gpos)
    //{
    //    grabbale_object = gpos;
        

    //    if(grabbale_object != null)
    //    {
    //        if (grabbale_object.tag == "Interactive object")
    //            obj_type = Interaction_type.Object;
    //        else if (grabbale_object.tag == "Slider")
    //            obj_type = Interaction_type.Slider;
    //        else if (grabbale_object.tag == "Dial")
    //            obj_type = Interaction_type.Dial;
    //        else if (grabbale_object.tag == "Button")
    //            obj_type = Interaction_type.Button;
    //        else if (grabbale_object.tag == "Floor")
    //            obj_type = Interaction_type.Nothing;
           
    //    }
    //}

    public void SetCursorPosition(Vector3 position) { cursor_position = position; }
    public void SetCursorRotation(Quaternion rotation) { cursor_rotation = rotation; }
    public void SetColor(Selector sel)
    {
        switch (sel)
        {
            case Selector.Direct:
                reaction_color = Color.green;
                break;
            case Selector.EyeHand:
                reaction_color = Color.yellow;
                break;
            case Selector.Hand:
                reaction_color = Color.magenta;
                break;
            case Selector.Head:
                reaction_color = Color.cyan;
                break;
        };

    }


    //Accessors
    public bool IsGrabing() { return isgrabbing; }
    public bool ButtonPressed() { return pressed_button; }


    //virtual Functions 
    protected virtual void Focus()
    {

    }

    protected virtual void DoButton(Buttons b, bool v)
    {

    }

    protected virtual void SetGrabbing(bool val)
    {

        //if (grabbale_object != null)
        //{
        //    if (val && obj_type == Interaction_type.Object)
        //    {

        //        Vector3 pos = cursor_position;
        //        Quaternion rot = cursor_rotation;

        //        Vector3 grabed_pos = grabbale_object.position;
        //        Quaternion grabed_rot = grabbale_object.rotation;

        //        //Convert to child pos and rot
        //        child_rot = Quaternion.Inverse(rot) * grabed_rot;
        //        child_pos = grabed_pos - pos;
        //        child_pos = Quaternion.Inverse(rot) * child_pos;


        //        isgrabbing = true;
        //    }
        //    else if (val && (obj_type == Interaction_type.Slider || obj_type == Interaction_type.Dial))
        //    {
        //        child_pos = grabbale_object.transform.InverseTransformPoint(cursor_position);
        //        isgrabbing = true;
        //    }
        //    else if (val && obj_type == Interaction_type.Button)
        //    {
        //        GameObject obj = GameObject.Find("Button Reaction Object");
        //        obj.GetComponent<MeshRenderer>().material.color = reaction_color;
        //        isgrabbing = true;
        //        pressed_button = true;
        //    }
        //    else if((val == false || obj_type == Interaction_type.Nothing))
        //    {
        //        isgrabbing = false;
        //        pressed_button = false;
        //    }

        //}
    }

    protected virtual void Update()
    {


       //if(grabbale_object != null)
       // {
       //     if (isgrabbing && obj_type == Interaction_type.Object)
       //     {
       //         Vector3 pos = cursor_position;
       //         Quaternion rot = cursor_rotation;

       //         //convert child to world and set object 

       //         Vector3 grabed_pos = child_pos;
       //         Quaternion grabed_rot = child_rot;

       //         grabed_rot = rot * grabed_rot;
       //         grabed_pos = rot * child_pos;
       //         grabed_pos = pos + grabed_pos;

       //         grabbale_object.position = grabed_pos;
       //         grabbale_object.rotation = grabed_rot;

       //     }
       //     else if (isgrabbing && obj_type == Interaction_type.Slider)
       //     {
       //         float local_pos = grabbale_object.localPosition.y;

       //         Vector3 curr = grabbale_object.InverseTransformPoint(cursor_position);
       //         if (curr.y + local_pos > slider_max_y)
       //             local_pos = slider_max_y;
       //         else if (curr.y + local_pos < slider_min_y)
       //             local_pos = slider_min_y;
       //         else
       //             local_pos += curr.y;

       //         Vector3 new_pos = grabbale_object.localPosition;
       //         new_pos.y = local_pos;
       //         grabbale_object.localPosition = new_pos;

       //         if (curr.y + local_pos < slider_min_y)
       //             slider_value = 0f;
       //         else if (curr.y + local_pos > slider_max_y)
       //             slider_value = 1f;
       //         else
       //             slider_value = (curr.y + local_pos) / slider_max_y;

       //         //Debug.Log("Slider value:" + slider_value);
       //     }
       //     else if (isgrabbing && obj_type == Interaction_type.Dial)
       //     {
       //         Quaternion local_rotation = grabbale_object.localRotation;
       //         Vector3 curr = grabbale_object.InverseTransformPoint(cursor_position);
       //         child_pos.z = 0f;
       //         curr.z = 0f;
       //         Quaternion delta = Quaternion.FromToRotation(child_pos, curr);
       //         local_rotation *= delta;
       //         grabbale_object.localRotation = local_rotation;

       //         Vector3 rotated_up = local_rotation * Vector3.up;
       //         float result = Mathf.Acos(Vector3.Dot(rotated_up, Vector3.up));
       //         float degrees = result * (180.0f / Mathf.PI);
       //         if (Vector3.Dot(rotated_up, Vector3.right) > 0)
       //             degrees = 360 - degrees;
       //         dial_value = degrees;

       //         //Debug.Log("Dial value:" + dial_value);
       //     }
       // }

    }
    


}
