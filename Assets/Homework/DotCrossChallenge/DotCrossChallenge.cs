using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCrossChallenge : MonoBehaviour {
    public Transform pt1;
    public Transform pt2;
    public Transform pt3;
    public Transform pt_hand;

    // Create a new script DotCrossChallenge_YourName inherited from this script.
    // Then project pt_hand to the plane defined pt1, pt2 and pt3
    // 
    //
    // Pt1, pt2 and pt3 are position vectors that have endpoints that are coplaner.
    // While the endpoints of these vectors lie on the same plane the position vectors are not coplanar.
    // Use these points to find 


	// Use this for initialization
	void Start () {
        Vector3 pt = Vector3.zero;

        #region STUDENT_CODE
        // Using only pt1,2,3 on the plane project pt_hand to the plane.
        Vector3 V1 = pt1.position - pt2.position;
        Vector3 V2 = pt1.position - pt3.position;
        Vector3 V3 = Vector3.Cross(V2, V1);
        Vector3 V4 = V3 / V3.magnitude;
        Vector3 V5 = pt_hand.position - pt2.position;
        float result = Vector3.Dot(V5, V4);
        Vector3 V6 = V4 * result;
        Vector3 V7 = V6 - V5;
        Vector3 V8 = pt_hand.position - V6;
        pt = V8;

        #endregion

        pt_hand.localPosition = pt;
    }
}
