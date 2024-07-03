using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActivator : MonoBehaviour
{
    public Camera areaCamera;
    public Transform areaTransform;
    public LayerMask playerLayer;
    void Start()
    {
        areaCamera = GetComponentInChildren<Camera>();
        areaTransform = GetComponent<Transform>().GetChild(0);
    }


    void Update()
    {
        if (Physics2D.OverlapBox(areaTransform.position, areaTransform.localScale, 0f, playerLayer))
        {
            areaCamera.gameObject.SetActive(true);
            Debug.Log("In Area");
        }
        else
        {
            areaCamera.gameObject.SetActive(false);
        }
    }
}
