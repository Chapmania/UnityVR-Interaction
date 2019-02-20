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
        //Being used by other classes//
    }

    protected virtual void DoButton(Buttons b, bool v)
    {
        //Being used by other classes//
    }

    protected virtual void SetGrabbing(bool val)
    {
        //Being used by other classes//
    }

    protected virtual void Update()
    {
        //Being used by other classes//
    }



}
