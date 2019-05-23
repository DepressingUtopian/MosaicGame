using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public GameObject wayPoint;
    void Start()
    {
    
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoint.transform.position, Time.deltaTime);

    }

    private void OnMouseDown()
    {
        Debug.Log("MOSAIC TOUCH");
        
    }
}

