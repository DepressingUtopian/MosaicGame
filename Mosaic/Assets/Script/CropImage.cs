using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CropImage : MonoBehaviour
{
    public GameObject pointerURL_GameObject;
    public GameObject pointerURL_MosaicTile;
    public string pathTextureMask;
    PuzzleGenerator generator;

    public GameObject[] pointerMosaicPrefabs;
    private Color[] ColorMask;
    private GameObject []mosaicBlocks;

    private float mosaicHeight = 0;
    private float mosaicWidth = 0;

    public int verticalBlockCount = 8;
    public int gorizontalBlockCount = 10;
    struct MosaicTile
    {
        public GameObject obj;
        public Vector3 vector3rotate;
    }
    private Dictionary<int, MosaicTile> GameObjectMosaicDictionaty;
    private Dictionary<int, GameObject> MosaicTileDictionaty;

    public string imgPath;

    // Start is called before the first frame update
    void Start()
    {
        BlockCreation();
       // CreateBitMask();
        MosaicTileCreation();
        //ImageHandler p = new ImageHandler("Assets/Source/Image/sample_image.png");
        //Debug.Log("Hello", "test");
        Texture2D texture = null;
        
        byte[] imgData;

        if (File.Exists(imgPath))
        {

            imgData = File.ReadAllBytes(imgPath);
            texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(imgData);

            ResizeTexture(ref texture);

            mosaicHeight = texture.height;
            mosaicWidth = texture.width;

                mosaicBlocks = GameObject.FindGameObjectsWithTag("mosaicBlock");

            if (mosaicBlocks != null)
            {
               // float cutHeight = mosaicHeight / verticalBlockCount;
               // float cutWidth = mosaicWidth / gorizontalBlockCount;

                int deltaX_old = (int) mosaicHeight / verticalBlockCount;
                int deltaY_old = (int)mosaicWidth / gorizontalBlockCount;
                int deltaX_new = 0;
                int deltaY_new = 0;
                int projection_size_x = (int)(deltaX_old * 0.25);
                int projection_size_y = (int)(deltaY_old * 0.25);
                int x = 0;
                int y = 0;
                int i = 0;
                int j = 0;
                string mosaic_code;
                Component[] hingeJoints;
                Debug.LogFormat("h = {0}, w = {1}",mosaicHeight,mosaicWidth);
                foreach (MosaicTile block in GameObjectMosaicDictionaty.Values)
                {

                    deltaX_new = deltaX_old;
                    deltaY_new = deltaY_old;
                    mosaic_code = generator.mosaicDivision[i, j];
                    if (i < gorizontalBlockCount - 1)
                        i++;
                    else
                    {
                        i = 0;
                        j++;
                    }

                    if (j == verticalBlockCount)
                        return;



                    if (mosaic_code[0] == '2') 
                        deltaY_new += projection_size_y;
                    if(mosaic_code[2] == '2')
                        deltaY_new += projection_size_y;
                    if (mosaic_code[1] == '2')
                        deltaX_new += projection_size_x;
                    if(mosaic_code[3] == '2')
                        deltaX_new += projection_size_x;

                  
                    Debug.LogFormat("x = {0}, y = {1} , dX = {2} , dY = {3}, code = {4}",x,y,deltaX_new,deltaY_new,mosaic_code);
                    Texture2D cut_texture = new Texture2D(deltaX_new, deltaY_new, TextureFormat.ARGB32, false);
                    if (y + deltaY_new >= mosaicHeight)
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
                       // cut_texture.ro
                        cut_texture.Apply();
                        block.obj.gameObject.GetComponent<Renderer>().material.color = Color.black;
                        // block.GetComponent<Renderer>().material.color = Color.white;
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
                        
                        //GO.GetComponentInChildren<Renderer>().material.mainTexture = cut_texture;



                    }
                        //block.transform.localScale += new Vector3((float) (deltaX * 0.01),(float) (deltaY * 0.01), 0);
                        //block.transform.position += new Vector3((float)(x * 0.01), (float)(y * 0.01), 0);
                    if (x  >= mosaicWidth - deltaX_old)
                    {
                        x = 0;
                        y += deltaY_old;
                    }
                    else
                        x += deltaX_old;
                }
                
            }

        }
    }

    private void ResizeTexture(ref Texture2D texture)
    {
        int newIMG_Height = 0;
        int newIMG_Width = 0;
        int resizeKoef = 0; //Показатель на сколько изображение будет смещаться относительно  размера текстуры
        try
        {
            if (gorizontalBlockCount > verticalBlockCount)
            {
                if (texture.width > texture.height)
                {
                    resizeKoef = (texture.width / gorizontalBlockCount - texture.height / verticalBlockCount) * verticalBlockCount;
                    newIMG_Height = texture.height + resizeKoef;
                    newIMG_Width = texture.width;
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
            else if (gorizontalBlockCount < verticalBlockCount)
            {

            }
               
        }
        catch (System.Exception e)
        {
            print(e.Message);
        }

        Texture2D new_texture = new Texture2D(newIMG_Width, newIMG_Height, TextureFormat.ARGB32, false);
        Color[] c = texture.GetPixels(0, 0, texture.width, texture.height);
        print((newIMG_Height - texture.height) / 2);
        new_texture.SetPixels((newIMG_Width - texture.width) / 2, (newIMG_Height - texture.height) / 2, texture.width, texture.height, c,0);

        texture = new_texture;
    }
    private void BlockCreation()
    {
        GameObjectMosaicDictionaty = new Dictionary<int, MosaicTile>(verticalBlockCount * gorizontalBlockCount);
        MosaicTile tempMosaicTile;
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
                    tempMosaicTile.obj = Instantiate(currentBlock, new Vector3(x, y, 0), Quaternion.Euler(rotate));

                tempMosaicTile.obj.name = currentBlock.name + " -> " + blockG + "x: " + rotate.x + "y: "+ rotate.y + "z:" + rotate.z;
                tempMosaicTile.vector3rotate = rotate;
                   // tempGameObject.transform.localScale = new Vector3(4.0f, 0.0f, 4.0f);
                //tempGameObject.GetComponent<Renderer>().material.color = Color.white;
                //tempGameObject.transform.position += new Vector3((float)i, (float)j, 0);
                // Vector3 temp = Quaternion.Euler(rotate).ToEulerAngles();
                GameObjectMosaicDictionaty.Add(countBlocks++, tempMosaicTile);
                }
            }
       }
    
    public void MosaicTileCreation()
    {
        MosaicTileDictionaty = new Dictionary<int, GameObject>(verticalBlockCount * gorizontalBlockCount);
        GameObject tempGameObject;
        int countBlocks = 0;
        for (int y = 0; y < verticalBlockCount; y++)
        {
            for (int x = 14; x < gorizontalBlockCount + 14; x++)
            {

                tempGameObject = Instantiate(pointerURL_MosaicTile, new Vector3(x, y, 0), Quaternion.identity);
              
                //tempGameObject.GetComponent<Renderer>().material.color = Color.white;
                //tempGameObject.transform.position += new Vector3((float)i, (float)j, 0);

                MosaicTileDictionaty.Add(countBlocks++, tempGameObject);
                ;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    /*
    public void CreateBitMask()
    {
        Texture2D texture = null;
        byte[] BitMap;

        if (File.Exists(pathTextureMask))
        {
            BitMap = File.ReadAllBytes(pathTextureMask);
            texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture.LoadImage(BitMap);
            Color[] c;
            c = texture.GetPixels(0, 0, texture.width, texture.height);

                for (int j = 0; j < c.Length; j++)
                {
                  
                    Debug.Log(c[j].r + " " + c[j].g + " " + c[j].b);

                }
        }
    }
    */
}


