using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convertions : MonoBehaviour {

	public void LocaltoWorld(Transform parent_transform, Vector3 pos, Quaternion rotation,Vector3 Scale)
    {
     


        Scale = Vector3.Scale(parent_transform.localScale, Scale);
        rotation = parent_transform.localRotation * rotation;
        pos = Vector3.Scale(parent_transform.localScale, pos);
        pos = parent_transform.localRotation * pos;
        pos = parent_transform.localPosition + pos;
       

    }
}
