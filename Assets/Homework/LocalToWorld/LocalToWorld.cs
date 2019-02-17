using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToWorld : MonoBehaviour
{
    void PrintError(Vector3 worldPos, Quaternion worldRot, Vector3 worldScale)
    {
        float dpos = (worldPos - transform.position).magnitude;
        float drot = 1f - Mathf.Abs((transform.rotation * Quaternion.Inverse(worldRot)).w);
        float dscale = (worldScale - transform.lossyScale).magnitude;
        Debug.Log("Error: Pos:" + Mathf.Round(dpos * 1000f) / 1000f + " Rot:" + Mathf.Round(drot * 1000f) / 1000f + " Scale:" + Mathf.Round(dscale * 1000f) / 1000f);
    }

    void Start()
    {

        Vector3 pos = transform.localPosition;
        Quaternion rot = transform.localRotation;
        Vector3 scale = transform.localScale;

        Transform curr = transform.parent;
        while (curr != null)
        {
            #region STUDENT_CODE

            scale = Vector3.Scale(curr.localScale,scale);
            rot = curr.localRotation * rot;
            pos = Vector3.Scale(curr.localScale, pos);
            pos = curr.localRotation * pos;
            pos = curr.localPosition + pos;
           
            //convert.LocaltoWorld(curr, pos, rot, scale);

            #endregion

            curr = curr.parent;
        }

        PrintError(pos, rot, scale);
    }
}