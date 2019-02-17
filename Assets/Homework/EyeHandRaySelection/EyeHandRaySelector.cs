using UnityEngine;

public class EyeHandRaySelector : MonoBehaviour
{
    public Transform eye_pos = null;
    public Transform cursor_ball_obj = null;
    public Transform wheel = null;

    float armscale = 1f;
    Vector3 cursor_pos = Vector3.zero;
    Quaternion cursor_rot = Quaternion.identity;
    bool isGrabbing = false;

    Vector3 child_pos = Vector3.zero;
    Quaternion child_rot = Quaternion.identity;

    //Variables to save vectors and other data
    float eyehand_initial = 0f;
    float eyehit_initial = 0f;
    float ratio = 0.0f;

    private void FixedUpdate()
    {
        if (isGrabbing)
        {
            #region STUDENT_CODE

            // Use the current hand to eye distance and the ratio of hit-eye to hand-eye set at the moment of grab
            // to compute the new world cursor position. Use the hand (this) rotation for the world cursor rotation.
            // Conver the child offset values to world and set the wheels position and rotation to the result.

            Vector3 curr_eyehand = transform.position - eye_pos.position;
            float new_length = curr_eyehand.magnitude * eyehit_initial / eyehand_initial;
            Vector3 new_curr_hitpt = curr_eyehand.normalized * new_length;
            new_curr_hitpt += eye_pos.position;

            cursor_rot = transform.rotation;
            cursor_pos = new_curr_hitpt;

            wheel.position = (cursor_rot * child_pos) + cursor_pos;
            wheel.rotation = cursor_rot * child_rot;
            // Set the cursor_ball_obj to the world cursor position.
            cursor_ball_obj.position = cursor_pos;
            #endregion
        }
        else
        {

            #region STUDENT_CODE

            // Raycast from eye_pos to hand_pos (this) until you hit the wheel.
            // At the moment of the first ray hit save the hand to eye distance, the hitpt to eye distance and
            // and convert the wheel position and rotation to be a child of the world hitpt and cursor (this) rotation.
            RaycastHit hit;
           if (Physics.Raycast(eye_pos.position, transform.position - eye_pos.position, out hit) && hit.collider.name == "Wheel")
            {
                Debug.Log("wheel");
                Vector3 hitposition = hit.point;
                eyehand_initial = (transform.position - eye_pos.position).magnitude;
                eyehit_initial = (hitposition - eye_pos.position).magnitude;

                cursor_pos = hitposition;
                cursor_rot = transform.rotation;

                child_rot = Quaternion.Inverse(cursor_rot) * wheel.rotation;
                child_pos = wheel.position - cursor_pos;
                child_pos = Quaternion.Inverse(cursor_rot) * child_pos;

                isGrabbing = true;
            }
 

            #endregion

        }
    }
}

