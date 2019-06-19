using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents 
{
    static public int[] collectedMosaicBlock;
    static public Dictionary<int, MosaicTile> GameObjectMosaicDictionaty;
    static public Dictionary<int, GameObject> MosaicTileDictionaty;
    public struct MosaicTile
    {
        public GameObject obj;
        public Vector3 vector3rotate;
        public static int count = 0;
        public int blockID;
    }
   

    public static void initCollected(int size)
    {
        collectedMosaicBlock = new int[size];
    }
    public static void addToSolution(int id_tile,int id_block)
    {
        collectedMosaicBlock[id_tile] = id_block;
    }
    public static bool checkedSolution()
    {
        for(int i = 0;i < collectedMosaicBlock.Length - 1; i++)
        {
            if(collectedMosaicBlock[i + 1] - collectedMosaicBlock[i] != 1)
                return false;
        }
        return true;
    }
    
}
