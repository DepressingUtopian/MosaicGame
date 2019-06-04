﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class PuzzleGenerator : MonoBehaviour
{
    public int height_grid = 0;
    public int width_grid = 0;
    private string[] CornerBlocks = {"2200","1100","2100","1200"};
    private string[] SideBlocks = { "2101","1202", "1011", "2220", "2201", "2210", "2011", "0211" };
    private string[] CenterBlocks = { "2222","1111","1221","1222","1121" };
    public  string[,] mosaicDivision;
    enum formBlocks { Corner,Side,Center};
    // Start is called before the first frame update
    public PuzzleGenerator(int height,int width)
    {
        this.height_grid = height;
        this.width_grid = width;
        mosaicDivision = new string[width_grid,height_grid ];
        DivisionProcess();
    }
    void Start()
    {
       // mosaicDivision = new string[height_grid, width_grid];

       // DivisionProcess();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DivisionProcess()
    {
        string direction;
        for (int y = 0; y < height_grid; y++)
        {
            for (int x = 0; x < width_grid; x++)
            {
               
             mosaicDivision[x, y] = GetMosaicDirection(x, y);
                
             // Debug.LogFormat("i: {0} j: {1} value: {2}", x, y, mosaicDivision[x, y]);
                //Debug.Log("i: {0} j: {1} value: {2}",i,j, mosaicDivision[i, j]);

            }
        }
    }
    private string GetMosaicPlate(int x, int y)
    {
        string PlateCode = "";
        if ((x == 0 && y == 0) || (x == 0 && y == height_grid) || (x == width_grid && y == 0) || (x == width_grid && y == height_grid))
        {
            GenСombinations(ref PlateCode, (0100), formBlocks.Corner);
        }
        else if (x == 0 || y == 0)
        {
            GenСombinations(ref PlateCode, (0100), formBlocks.Side);
        }
        else
        {
            GenСombinations(ref PlateCode, (0100), formBlocks.Center);
        }
        return PlateCode;
    }
    private void GenСombinations(ref string _PlateCode,byte direction, formBlocks _form)
    {
        Random rand = new Random();
     
        switch (_form)
        {
            case formBlocks.Corner:
            {
              string temp = CornerBlocks[Random.Range(0,CornerBlocks.Length - 1)];
              _PlateCode = temp;
              break;
            }
            case formBlocks.Side:
            {
                    string temp = SideBlocks[Random.Range(0, CornerBlocks.Length - 1)];
                    _PlateCode = temp;
                    break;
            }
            case formBlocks.Center:
            {
               string temp = CenterBlocks[Random.Range(0, CornerBlocks.Length - 1)];
               _PlateCode = temp;
               break;
            }

        }
    }
    private string GetMosaicDirection(int x , int y)
    {
       //char[] BlockSeq = new char[4];
        int[] BlockSeq = new int[4];
        if (x == 0)
        {
         BlockSeq[3] = 0;
        }
        else if (mosaicDivision[x - 1,y] != null)
        {
            if (mosaicDivision[x - 1, y][1] == '2')
                BlockSeq[3] = 1;
            else
                BlockSeq[3] = 2;
        }
        else
            BlockSeq[3] = Random.Range(1, 3);

        if (x == width_grid - 1)
        {
            BlockSeq[1] = 0;
        }
        else if (mosaicDivision[x + 1, y] != null)
        {
            if (mosaicDivision[x + 1, y][3] == '2')
                BlockSeq[1] = 1;
            else
                BlockSeq[1] = 2;
        }
        else
            BlockSeq[1] = Random.Range(1, 3);

        if (y == 0)
        {
            BlockSeq[2] = 0;
        }
        else if (mosaicDivision[x, y - 1] != null)
        {
            if (mosaicDivision[x, y - 1][0] == '2')
                BlockSeq[2] = 1;
            else
                BlockSeq[2] = 2;
        }
        else
            BlockSeq[2] = Random.Range(1, 3);

        if (y == height_grid - 1)
        {
            BlockSeq[0] = 0;
        }
        else if (mosaicDivision[x, y + 1] != null)
        {
            if (mosaicDivision[x, y + 1][2] == '2')
                BlockSeq[0] = 1;
            else
                BlockSeq[0] = 2;
        }
        else
            BlockSeq[0] = (char)Random.Range(1, 3);
      
        return string.Join("", BlockSeq);
    }
    public bool GetInstantiateBlockJigsawPuzzle(int x, int y, GameObject block,ref Vector3 new_rotate)
    {
        
       string nameBlock = String.Concat(block.name.OrderBy(c => c));
       string codeBlock = String.Concat(mosaicDivision[x, y].OrderBy(c => c));
       string orig_nameBlock = String.Copy(block.name);

      new_rotate = new Vector3() { x = 0.0f,y = 90.0f,z = -90.0f}; //Положение по умолчанию.
       

        if (codeBlock.Equals(nameBlock))
         {
            if (!mosaicDivision[x, y].Equals(orig_nameBlock))
            {
                int count = 0;
                while (!mosaicDivision[x, y].Equals(orig_nameBlock))
                {
                    if (count == 4)
                        return false;

                        linePitch(ref orig_nameBlock);
                        new_rotate.x += 90.0f;
                        count++;
                }
            }
        }
        else
            return false;

        if(!mosaicDivision[x, y].Equals(orig_nameBlock))
            return false;
        /* if (new_rotate.z >= 360.0f)
             new_rotate.z -= 360.0f;
         if (new_rotate.z <= -360.0f)
             new_rotate.z += 360.0f;*/
        //new_rotate = new_rotate.x * Vector3.forward;
        return true;
    }
    private void linePitch(ref string str)
    {
        char[] temp_str = str.ToCharArray();
        temp_str[0] = str[3]; //Костыль с учетом что размер массива 4
        for (int i = 1; i < str.Length; i++)
        {
            temp_str[i] = str[i - 1];
        }
        str = string.Join("", temp_str);
    }
   
} 
