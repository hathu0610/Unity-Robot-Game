﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
    public float weaponRange = 50f;

    public Camera cam;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lineOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(lineOrigin, cam.transform.forward * weaponRange, Color.green);
    }
}
