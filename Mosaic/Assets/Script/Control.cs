using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public GameObject draggingObject;
    private static Camera targetCamera;
    private bool isSelected = false;
    private bool moveFlag = false;
    private bool isDragging = false;
    private bool TilePlate = false;
    private Vector3 posVec;
    private RaycastHit vision;
    private Shader defaultShader;
    private Vector3 defaultTransformPosition;
    public int blockID = 0;
    public static int count = 1;
    void Start()
    {
        defaultShader = this.GetComponent<Renderer>().material.shader;
        defaultTransformPosition = this.transform.position;
        targetCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Нумеруем объекты
        blockID = count;
        count++;
    }

    void Update()
    {
        Vector3 v = new Vector3 { x = 0.0f,y = -1.0f,z = 0.0f};
   
        Debug.DrawRay(this.transform.position, transform.TransformDirection(v) * 4.0f, Color.red, 2f);
        if (moveFlag)
        {
            
            transform.root.position = posVec;
            //transform.position = Vector3.MoveTowards(transform.position, posVec, Time.deltaTime);
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

            moveFlag = false;
            TilePlate = true;
            defaultTransformPosition = this.transform.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
           
            if (isSelected)
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out vision, 100.0f))
                {
                    if (vision.collider.tag == "mosaicBlock" && TilePlate)
                    {
                        //this.transform.position = defaultTransformPosition;
                      
                    }
                        Debug.Log("CLICK BLOCK");
                    if (vision.collider.tag == "mosaicTile")
                    {
                        posVec = vision.transform.position;
                        posVec.z -= 0.05f;
                        this.transform.localPosition = new Vector3 { x = 0, y = 0, z = 0 };
                        moveFlag = true;
                        isSelected = false;
                        this.GetComponent<Renderer>().material.shader = defaultShader;
                        TilePlate = true;
                        GameEvents.addToSolution(int.Parse(vision.collider.name), blockID);
                        if (GameEvents.checkedSolution())
                            Debug.LogFormat("{0}", "Победа!");
                    }

                    

                    if (vision.collider.tag == "mosaicBlock" && vision.transform.position != this.transform.position)
                    {
                       
                        isSelected = false;
                        isDragging = false;
                        this.GetComponent<Renderer>().material.shader = defaultShader;
                       this.transform.position = new Vector3 {x = this.transform.position.x, y = this.transform.position.y, z = defaultTransformPosition.z };
                        Debug.Log(defaultTransformPosition);
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
        v3.z = targetCamera.transform.position.z * (-1) - 1.0f;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        Debug.Log(v3); //Current Position of mouse in world space
        return v3;
    }
    private void OnMouseDown()
    {
       
        this.GetComponent<Renderer>().material.shader = Shader.Find("Custom/OutlineObjects");
        isSelected = true;
    }
    private void OnMouseDrag()
    {
        if (draggingObject != null && isSelected && !moveFlag)
        {
            draggingObject.GetComponent<Rigidbody>().MovePosition(CalculateMouse3DVector());
        }
    }
    private void OnMouseUp()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(this.transform.position, transform.TransformDirection(new Vector3 { x = 0.0f, y = -1.0f, z = 0.0f }) * 4.0f, 4f);

        if(hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
                if (hits[i].collider.tag == "mosaicBlock")
                    return;
            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].collider.tag == "mosaicTile")
                {
                    posVec = hits[i].transform.position;
                    posVec.z -= 0.05f;
                    TilePlate = true;
                    isSelected = false;
                    //this.transform.
                    this.transform.position = defaultTransformPosition;

                    this.transform.root.position = posVec;
                    this.transform.localPosition = new Vector3 { x = 0, y = 0, z = 0 };
                    defaultTransformPosition = this.transform.position;
                    //this.transform.GetChild(0).position = new Vector3 { x = 0, y = 0, z = 0 };
                    this.GetComponent<Renderer>().material.shader = defaultShader;

                    GameEvents.addToSolution(int.Parse(hits[i].collider.name), blockID);
                    if (GameEvents.checkedSolution())
                        Debug.LogFormat("{0}", "Победа!");

                    return;
                }
            }
        }       
    }
    
}

