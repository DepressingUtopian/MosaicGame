using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CropImage : MonoBehaviour
{
    public GameObject pointerURL_GameObject = null;
    public GameObject pointerURL_MosaicTile;

    PuzzleGenerator generator;

    public GameObject[] pointerMosaicPrefabs;
    private GameObject []mosaicBlocks;

    public GameObject Backplate;

    private float mosaicHeight = 0;
    private float mosaicWidth = 0;

    static public int verticalBlockCount = 2;
    static public int gorizontalBlockCount = 2;

    private float scaleX = 1.0f;
    private float scaleZ = 1.0f;
    public float scalePower = 0.6f;
    static public string imgPath;
    static public bool isImageScale = true;
    static public bool isMosaicСollected = false;
    


    // Start is called before the first frame update
    void Start()
    {
        runCropProcess();
    }
    public void runCropProcess()
    {
        computeScale(scalePower);
        BlockCreation();
        MosaicTileCreation();
        Cropping();
    }
    private void Cropping()
    {
        GameEvents.initCollected(verticalBlockCount * gorizontalBlockCount);

        Texture2D texture = null;

        byte[] imgData;

        if (File.Exists(imgPath))
        {

            imgData = File.ReadAllBytes(imgPath);
            texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(imgData);

            ResizeTexture(ref texture);

            mosaicHeight = (int)texture.height / verticalBlockCount * verticalBlockCount;
            mosaicWidth = (int)texture.width / gorizontalBlockCount * gorizontalBlockCount;

            mosaicBlocks = GameObject.FindGameObjectsWithTag("mosaicBlock");

            if (mosaicBlocks != null)
            {

                int deltaX_old = (int)mosaicHeight / verticalBlockCount;
                int deltaY_old = (int)mosaicWidth / gorizontalBlockCount;
                int deltaX_new = 0;
                int deltaY_new = 0;
                int projection_size_x = (int)(deltaX_old * 0.25);
                int projection_size_y = (int)(deltaY_old * 0.25);
                int x = 0, y = 0, i = 0, j = 0;

                string mosaic_code;
                Component[] hingeJoints;
                Debug.LogFormat("h = {0}, w = {1}", mosaicHeight, mosaicWidth);
                foreach (GameEvents.MosaicTile block in GameEvents.GameObjectMosaicDictionaty.Values)
                {

                    deltaX_new = deltaX_old;
                    deltaY_new = deltaY_old;
                    mosaic_code = generator.mosaicDivision[i, j];
                   

                    if (mosaic_code[0] == '2')
                        deltaY_new += projection_size_y;
                    if (mosaic_code[2] == '2')
                        deltaY_new += projection_size_y;
                    if (mosaic_code[1] == '2')
                        deltaX_new += projection_size_x;
                    if (mosaic_code[3] == '2')
                        deltaX_new += projection_size_x;

                    Debug.LogFormat("x = {0}, y = {1} , dX = {2} , dY = {3}, code = {4}", x, y, deltaX_new, deltaY_new, mosaic_code);
                    Texture2D cut_texture = new Texture2D(deltaX_new, deltaY_new, TextureFormat.ARGB32, false);
                    if (y >= mosaicHeight)
                    {
                        y = 0;
                        break;
                    }

                    else
                    {
                        int new_x = x;
                        int new_y = y;

                        if (mosaic_code[3] == '2' && x > 0)
                            new_x -= projection_size_x;
                        if (mosaic_code[2] == '2' && y > 0)
                            new_y -= projection_size_y;


                        Color[] c = texture.GetPixels(new_x, new_y, deltaX_new, deltaY_new);
                        float angle = 0.0f;
                        cut_texture.SetPixels(c);
                        cut_texture.Apply();
                        block.obj.gameObject.GetComponent<Renderer>().material.color = Color.green;

                        hingeJoints = block.obj.GetComponentsInChildren<Renderer>();
                        foreach (Renderer joint in hingeJoints)
                        {
                            joint.material.mainTexture = cut_texture;
                            angle = block.vector3rotate.x;
                            if (angle >= 90.0f)
                                angle -= 90.0f;
                            else
                            if (angle == 0.0f)
                                angle = -90.0f;
                            else
                            if (angle == 90.0f)
                                angle = 0.0f;

                            joint.material.SetFloat("_RotationAngle", angle);
                        }

                    }

                    if (x >= mosaicWidth - deltaX_old)
                    {
                        x = 0;
                        y += deltaY_old;
                    }
                    else
                        x += deltaX_old;

                    if (i < gorizontalBlockCount - 1)
                        i++;
                    else
                    {
                        i = 0;
                        j++;
                    }
                    if (j == verticalBlockCount)
                        return;
                }

            }

        }
    }
    private void ResizeTexture(ref Texture2D texture)
    {
        int newIMG_Height = 0;
        int newIMG_Width = 0;
        int resizeKoef = 0; //Показатель на сколько изображение будет смещаться относительно  размера текстуры
        if (!isImageScale)
        {
            try
            {
                if (texture.width > texture.height)
                {
                    if ((resizeKoef = (texture.width / gorizontalBlockCount - texture.height / verticalBlockCount)) > 0)
                    {
                        newIMG_Height = texture.height + resizeKoef * verticalBlockCount;
                        newIMG_Width = texture.width;
                    }
                    else
                    {
                        resizeKoef *= -1;
                        newIMG_Width = texture.width + resizeKoef * gorizontalBlockCount;
                        newIMG_Height = texture.height;
                    }
                }
                else if (texture.width < texture.height)
                {
                    resizeKoef = (texture.height / verticalBlockCount - texture.width / gorizontalBlockCount) * gorizontalBlockCount;
                    newIMG_Width = texture.width + resizeKoef;
                    newIMG_Height = texture.height;
                }
                else if (texture.width == texture.height)
                {

                    resizeKoef = (texture.height / verticalBlockCount - texture.width / gorizontalBlockCount) * gorizontalBlockCount;
                    newIMG_Width = texture.width + resizeKoef;
                    newIMG_Height = texture.height;
                }
            }
            catch (System.Exception e)
            {
                print(e.Message);
            }
        }
        else
        {
            try
            {
                if (texture.width > texture.height)
                {
                    if ((resizeKoef = (texture.width / gorizontalBlockCount - texture.height / verticalBlockCount)) > 0)
                    {
                        newIMG_Width = texture.width - resizeKoef * gorizontalBlockCount;
                        newIMG_Height = texture.height;
                    }
                    else
                    {
                        resizeKoef *= -1;
                        newIMG_Height = texture.height - resizeKoef * verticalBlockCount;
                        newIMG_Width = texture.width;
                    }
                }
                else if (texture.width < texture.height)
                {
                    resizeKoef = (texture.height / verticalBlockCount - texture.width / gorizontalBlockCount) * gorizontalBlockCount;
                    newIMG_Width = texture.width - resizeKoef;
                    newIMG_Height = texture.height;
                }
                else if (texture.width == texture.height)
                {

                    resizeKoef = (texture.height / verticalBlockCount - texture.width / gorizontalBlockCount) * gorizontalBlockCount;
                    newIMG_Width = texture.width - resizeKoef;
                    newIMG_Height = texture.height;
                }


            }
            catch (System.Exception e)
            {
                print(e.Message);
            }

        }
        Texture2D new_texture = new Texture2D(newIMG_Width, newIMG_Height, TextureFormat.ARGB32, false);
        Color[] c;
        if (!isImageScale)
        {
            c = texture.GetPixels(0, 0, texture.width, texture.height);
            new_texture.SetPixels((newIMG_Width - texture.width) / 2, (newIMG_Height - texture.height) / 2, texture.width, texture.height, c, 0);
        }
        else
        {
            c = texture.GetPixels((texture.width - newIMG_Width) / 2, (texture.height - newIMG_Height) / 2, newIMG_Width, newIMG_Height);
            new_texture.SetPixels(0, 0, newIMG_Width, newIMG_Height, c, 0);
        }
        Debug.LogFormat(" x  - {0} ,y  - {1}", (newIMG_Width - texture.width) / 2, (newIMG_Height - texture.height) / 2);

     
        

        texture = new_texture;
    }
    private void BlockCreation()
    {
        GameEvents.GameObjectMosaicDictionaty = new Dictionary<int, GameEvents.MosaicTile>(verticalBlockCount * gorizontalBlockCount);
        GameEvents.MosaicTile tempMosaicBlock;
        GameObject currentBlock = new GameObject();
        string blockG = "";
        int countBlocks = 0;
        Vector3 rotate = new Vector3();
        generator = new PuzzleGenerator(verticalBlockCount, gorizontalBlockCount);
        
        for (int y = 0; y < verticalBlockCount; y++)
            {
                for (int x = 0; x < gorizontalBlockCount; x++)
                {

                    for (int k = 0; k < pointerMosaicPrefabs.Length; k++)
                    {
                        currentBlock = null;
                        if (generator.GetInstantiateBlockJigsawPuzzle(x, y, pointerMosaicPrefabs[k], ref rotate))
                            {
                               currentBlock = pointerMosaicPrefabs[k];
                               blockG = generator.mosaicDivision[x, y];
                               break;
                            
                            }
                    }

                if (currentBlock == null)
                {
                    Debug.Log(blockG);
                    continue;
                }

                tempMosaicBlock.obj = Instantiate(currentBlock, new Vector3(Random.Range(0 - Backplate.transform.localScale.x / 2 * 0.70f, Backplate.transform.localScale.x / 2 * 0.70f), Random.Range(0 - Backplate.transform.localScale.y / 2 * 0.70f, Backplate.transform.localScale.y / 2 * 0.70f), 0.06f), Quaternion.Euler(rotate));
                //tempMosaicBlock.obj = Instantiate(currentBlock,new Vector3( x, y, 0.06f), Quaternion.Euler(rotate));

                tempMosaicBlock.obj.name = countBlocks.ToString();
                tempMosaicBlock.obj.transform.root.transform.localScale = new Vector3 { x = scaleX, y = 1.0f, z = scaleZ };
                tempMosaicBlock.vector3rotate = rotate;
                tempMosaicBlock.blockID = x + y * gorizontalBlockCount;

                GameEvents.GameObjectMosaicDictionaty.Add(countBlocks++, tempMosaicBlock);
                }
            }
       }
    
    public void MosaicTileCreation()
    {
        GameEvents.MosaicTileDictionaty = new Dictionary<int, GameObject>(verticalBlockCount * gorizontalBlockCount);
        GameObject tempGameObject;
        int countBlocks = 0;
        for (float y = 0 - verticalBlockCount / 2; y < verticalBlockCount / 2; y++)
        {
            for (float x = 0 - gorizontalBlockCount / 2; x < gorizontalBlockCount / 2; x++)
            {

                tempGameObject = Instantiate(pointerURL_MosaicTile, new Vector3(x * scaleX, y * scaleZ, 0.1f), Quaternion.identity);
                tempGameObject.transform.root.transform.localScale = new Vector3 { x = scaleX, y = scaleZ, z = 0.01f };
                //tempGameObject.GetComponent<Renderer>().material.color = Color.white;
                //tempGameObject.transform.position += new Vector3((float)i, (float)j, 0);
                tempGameObject.name = countBlocks.ToString();
               
                GameEvents.MosaicTileDictionaty.Add(countBlocks++, tempGameObject);
                

            }
        }
    }
    public void computeScale(float scale_power)
    {
        float koef = Mathf.Max(gorizontalBlockCount,verticalBlockCount);
        float koef2 = Mathf.Min(Backplate.transform.localScale.x, Backplate.transform.localScale.y);
            scaleX = (koef2 * scale_power) / koef;
            scaleZ = scaleX;
    }
    // Update is called once per frame
    void Update()
    {

    }
    
}


