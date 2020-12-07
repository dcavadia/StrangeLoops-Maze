using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{

    public int Row;
    public int Column;

    public Cell North;
    public Cell South;
    public Cell East;
    public Cell West;

    public List<Cell> Links = new List<Cell>();

    //Constructor
    public Cell(int row, int column)
    {
        Row = row;
        Column = row;
    }

    public Cell LinkCells(Cell cell, bool noDirection)
    {

        Links.Add(cell);

        if (!noDirection)
        {
            cell.LinkCells(this, true);
        }

        return this;

    }

    public bool IsLinked(Cell cell)
    {
        if (Links.Contains(cell))
        {
            return true;
        }
        return false;
    }

    public List<Cell> Neighbours()
    {
        List<Cell> neighbours = new List<Cell>();

        if (North != null)
        {
            neighbours.Add(North);
        }
        if (East != null)
        {
            neighbours.Add(East);
        }
        if (South != null)
        {
            neighbours.Add(South);
        }
        if (West != null)
        {
            neighbours.Add(West);
        }

        return neighbours;

    }
    
}
