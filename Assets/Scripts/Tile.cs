using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{

    public int X { get; set; }
    public int Y { get; set; }
    public int Cost;
    public int Distance;
    public int CostDistance => Cost + Distance;
    public Tile Parent;

    //Straight line distance
    public void SetDistance(int targetX, int targetY)
    {
        this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
    }



}
