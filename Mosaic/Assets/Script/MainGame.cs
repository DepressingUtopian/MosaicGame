using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    private Dictionary<int, GameObject> GameObjectMosaicDictionaty;
    private Dictionary<int, GameObject> MosaicTileDictionaty;
    public GameObject pointerURL_GameObject;
    public GameObject pointerURL_MosaicTile;

    public int verticalBlockCount = 8;
    public int gorizontalBlockCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        BlockCreation();
        MosaicTileCreation();

    }

    // Update is called once per frame
    void Update()
    {
        
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

                GameObjectMosaicDictionaty.Add(countBlocks++, tempGameObject);
                ;
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
                tempGameObject.GetComponent<Renderer>().material.color = Color.white;


                MosaicTileDictionaty.Add(countBlocks++, tempGameObject);

            }
        }
    }
}
