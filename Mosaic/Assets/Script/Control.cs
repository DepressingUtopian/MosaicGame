using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public GameObject draggingObject;
    private bool isSelected = false;
    private bool moveFlag = false;
    private bool isDragging = false;
    private Vector3 posVec;
    private RaycastHit vision;
    private Shader defaultShader;
    private Vector3 defaultTransformPosition;
    void Start()
    {
        defaultShader = this.GetComponent<Renderer>().material.shader;
        defaultTransformPosition = this.transform.position;
    }

    void Update()
    {

        if (moveFlag)
        {

            transform.position = Vector3.MoveTowards(transform.position, posVec, Time.deltaTime);
        }

        if(!isDragging && isSelected && !moveFlag)
        {
            if (draggingObject)
            {
                draggingObject.GetComponent<Rigidbody>().isKinematic = true;
                isDragging = true;
            }
        }
        

        if (Vector3.Distance(transform.position, posVec) < 0.001f)
        {
            // Меняем местами положение цилиндра.
            moveFlag = false;
           
           // transform.position *= -1.0f;
        }

        if (Input.GetMouseButtonDown(0))
        {
           
            if (isSelected)
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out vision, 100.0f))
                {
                    if (vision.collider.tag == "mosaicTile")
                    {
                        posVec = vision.transform.position;
                        posVec.z = defaultTransformPosition.z;
                        moveFlag = true;
                    }
                        
                    if (vision.collider.tag == "mosaicBlock" && vision.transform.position != this.transform.position)
                    {
                        isSelected = false;
                        isDragging = false;
                        this.GetComponent<Renderer>().material.shader = defaultShader;
                        this.transform.position = defaultTransformPosition;
                     
                        Debug.Log(defaultTransformPosition);
                        Debug.Log("Default");


                    }

                    if (vision.collider.gameObject.GetComponent<Rigidbody>() && Vector3.Distance(vision.collider.gameObject.transform.position, transform.position) <= 3f)
                    {
                        draggingObject = vision.collider.gameObject;
                        Debug.Log("DRAG");
                    }

                }
        }
        else
        {
            if (draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            isDragging = false;
        }
    }

    private Vector3 CalculateMouse3DVector()
    {
        Vector3 v3 = Input.mousePosition;
        v3.z = 8f;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        Debug.Log(v3); //Current Position of mouse in world space
        return v3;
    }
    private void OnMouseDown()
    {
        Debug.Log("MOSAIC TOUCH");
        this.GetComponent<Renderer>().material.shader = Shader.Find("Custom/OutlineObjects");
        isSelected = true;
    }
    private void OnMouseDrag()
    {
        if (draggingObject != null && isSelected && !moveFlag)
        {
           //this.transform.position = new Vector3() { x = this.transform.position.x, y = this.transform.position.y, z = -2.0f };
            draggingObject.GetComponent<Rigidbody>().MovePosition(CalculateMouse3DVector());
        }
    }
}

