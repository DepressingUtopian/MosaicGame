using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CropImage : MonoBehaviour
{
    public GameObject pointerURL_GameObject;

    private GameObject []mosaicBlocks;

    private float mosaicHeight = 0;
    private float mosaicWidth = 0;

    public int verticalBlockCount = 8;
    public int gorizontalBlockCount = 10;
    private Dictionary<int, GameObject> GameObjectMosaicDictionaty;
    public string imgPath;
    // Start is called before the first frame update
    void Start()
    {
        BlockCreation();
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

                int deltaX = (int) mosaicHeight / verticalBlockCount;
                int deltaY = (int)mosaicWidth / gorizontalBlockCount;

                int x = 0;
                int y = 0;

                foreach (GameObject block in GameObjectMosaicDictionaty.Values)
                {
                    Texture2D cut_texture = new Texture2D(deltaX, deltaY, TextureFormat.ARGB32, false);

                    if (y >= mosaicHeight - deltaY)
                    {
                        y = 0;
                        break;
                    }
                    else
                    {
                        Color[] c = texture.GetPixels(x, y, deltaX, deltaY);

                        cut_texture.SetPixels(c);

                        cut_texture.Apply();
                        block.gameObject.GetComponent<Renderer>().material.color = Color.white;
                       // block.GetComponent<Renderer>().material.color = Color.white;
                        block.GetComponent<Renderer>().transform.Rotate(0,0,180, Space.Self);
                        block.GetComponent<Renderer>().material.mainTexture = cut_texture;
                        


                    }
                        //block.transform.localScale += new Vector3((float) (deltaX * 0.01),(float) (deltaY * 0.01), 0);
                        //block.transform.position += new Vector3((float)(x * 0.01), (float)(y * 0.01), 0);
                    if (x  >= mosaicWidth - deltaX)
                    {
                        x = 0;
                        y += deltaY;
                    }
                    else
                        x += deltaX;
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
        GameObjectMosaicDictionaty = new Dictionary<int, GameObject>(verticalBlockCount * gorizontalBlockCount);
        GameObject tempGameObject;
        int countBlocks = 0;
        for (int y = 0; y < verticalBlockCount; y++)
            {
            for (int x = 0; x < gorizontalBlockCount; x++)
            {

                tempGameObject = Instantiate(pointerURL_GameObject, new Vector3(x, y, 0), Quaternion.identity);
                tempGameObject.name = "block" + countBlocks;
                //tempGameObject.GetComponent<Renderer>().material.color = Color.white;
                //tempGameObject.transform.position += new Vector3((float)i, (float)j, 0);

                GameObjectMosaicDictionaty.Add(countBlocks++,tempGameObject);
                ;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public class ImageHandler
    {
        
        public ImageHandler(string imgPath)
        {
            
        }
    }
}


