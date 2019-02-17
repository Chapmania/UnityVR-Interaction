using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCalibration : MonoBehaviour {


    [System.Serializable]
    public class Correspondence
    {
        public Vector3 trackingPt;
        public Vector3 modelPt;
    }

    public Correspondence point1;
    public Correspondence point2;
    public Correspondence point3;
   
	void Start () {

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        #region STUDENT_CODE

        // Use the correspondences to compute the offset position and rotation that will align the two worlds.
        // Note: This assumes that the 3D model is accurate and in the same units used to measure the real world.

        //Vector3 diff = point1.trackingPt - point1.modelPt;
        //pos = diff;
        
        
        #endregion

        transform.rotation = rot;
        transform.position = pos;

    }

}
