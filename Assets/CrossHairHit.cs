using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairHit : MonoBehaviour
{
    Ray ray;
    RaycastHit hitInfo;
    Camera MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = MainCamera.transform.position;
        ray.direction = MainCamera.transform.forward;
        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;
    }
}
