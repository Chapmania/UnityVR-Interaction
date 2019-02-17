using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToLocal : MonoBehaviour {

    void PrintError(Transform child, Vector3 localPos, Quaternion localRot, Vector3 localScale)
    {
        Vector3 sibpos = (child.localRotation * Vector3.Scale(localPos, child.localScale)) + child.localPosition;
        sibpos = (transform.localRotation * Vector3.Scale(sibpos, transform.localScale)) + transform.localPosition;
        Quaternion sibrot = transform.localRotation * child.localRotation * localRot;
        Vector3 sibscale = Vector3.Scale( transform.localScale, Vector3.Scale(localScale, child.localScale) );

        float dpos = (sibpos - transform.localPosition).magnitude;
        float drot = 1f - Mathf.Abs((sibrot * Quaternion.Inverse(transform.localRotation)).w);
        float dscale = (sibscale - transform.localScale).magnitude;
        Debug.Log("Error: Pos:" + dpos.ToString("F2") + " Rot:" + drot.ToString("F2") + " Scale:" + dscale.ToString("F2"));
    }

    // Use this for initialization
    void Start () {

        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);

            Vector3 pos = child.localPosition;
            Quaternion rot = child.localRotation;
            Vector3 scale = child.localScale;

            #region STUDENT_CODE

            // First convert each child pos, rot and scale values to be a sibling of this transform. 
            // (see: previous localToWorld homework)
            // Then convert this transforms localPosition, localRotation and localScale to be local to the sibling frame.
            Vector3 scale_prime = Vector3.zero;
            Vector3 pos_prime = Vector3.zero;
            Quaternion rot_prime = Quaternion.identity;

            scale_prime = Vector3.Scale(transform.localScale, scale);
            rot_prime = transform.localRotation * rot;
            pos_prime = Vector3.Scale(transform.localScale, pos);
            pos_prime = transform.localRotation * pos_prime;
            pos_prime = transform.localPosition + pos_prime;

            scale.x = transform.localScale.x * (1 / scale_prime.x);
            scale.y = transform.localScale.y * (1 / scale_prime.y);
            scale.z = transform.localScale.z * (1 / scale_prime.z);
            rot = Quaternion.Inverse(rot_prime) * transform.localRotation;
            pos = transform.localPosition - pos_prime;
            pos = Quaternion.Inverse(rot_prime) * pos;
            pos.x = pos.x * (1 / scale_prime.x);
            pos.y = pos.y * (1 / scale_prime.y);
            pos.z = pos.z * (1 / scale_prime.z);


            #endregion

            // You get credit for each error value that prints as zero!
            PrintError(child, pos, rot, scale);

        }

    }
	
}
