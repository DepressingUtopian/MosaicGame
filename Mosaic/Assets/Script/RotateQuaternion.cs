using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 V;
    public float angle;
    private float temp_angle;
    private Quaternion q;
    // Start is called before the first frame update
    void Start()
    {
        temp_angle = angle * (Mathf.PI / 180);
        V = V.normalized;
        q = new Quaternion(Mathf.Sin(temp_angle / 2) * V.x, Mathf.Sin(temp_angle / 2) * V.y, Mathf.Sin(temp_angle / 2) * V.z, Mathf.Cos(temp_angle / 2));
    }

    // Update is called once per frame
    void Update()
    {
        temp_angle = angle * (Mathf.PI / 180);
       // V = V.normalized;
        q = new Quaternion(Mathf.Sin(temp_angle / 2) * V.x, Mathf.Sin(temp_angle / 2) * V.y, Mathf.Sin(temp_angle / 2) * V.z, Mathf.Cos(temp_angle / 2));
        transform.rotation = q;
    }
}
