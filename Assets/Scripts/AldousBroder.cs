using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AldousBroder
{
    
    public static void GenerateMaze(Grid grid)
    {
        Cell currentCell = grid.GetRandomCell();

        System.Random random = new System.Random((int)DateTime.Now.Ticks);

        int unvisited = grid.GetSize() - 1;

        while(unvisited > 0)
        {
            List<Cell> neighbours = currentCell.Neighbours();

            int randomNeighbour = random.Next(neighbours.Count);

            Cell neighbour = neighbours[randomNeighbour];

            if (!neighbour.Links.Any())
            {
                currentCell.LinkCells(neighbour, false);
                unvisited--;
            }

            currentCell = neighbour;

        }

    }

}
