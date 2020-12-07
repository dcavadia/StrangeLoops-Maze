using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Grid
{
    public int Rows;
    public int Columns;

    private Cell[,] cells;

    System.Random random = new System.Random((int)DateTime.Now.Ticks);

    //Constructor
    public Grid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;

        StartGrid();
        StartCells();
    }

    private void StartGrid()
    {
        cells = new Cell[Rows, Columns];

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                cells[row, col] = new Cell(row, col);
            }
        }

    }

    private void StartCells()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if(row> 0)
                {
                    cells[row, col].North = cells[row - 1, col];
                }
                if (row < (Rows - 1))
                {
                    cells[row, col].South = cells[row + 1, col];
                }
                if (col < (Columns - 1))
                {
                    cells[row, col].East = cells[row, col + 1];
                }
                if(col > 0)
                {
                    cells[row, col].West = cells[row, col - 1];
                }

            }
        }
    }

    public Cell GetRandomCell()
    {
        int row = random.Next(Rows - 1);
        int col = random.Next(Columns - 1);

        return cells[row, col];
    }

    public Cell GetCell(int row, int col)
    {
        if(row >= 0 && row <= Rows)
        {
            if(col >= 0 && col <= Columns)
            {
                return cells[row, col];
            }
        }

        return null;
    }

    public int GetSize()
    {
        return (Columns * Rows);
    }

    public string MazeToString()
    {
        StringBuilder map = new StringBuilder("+");

        //First line(border)
        for (int i = 0; i < Columns; i++)
        {
            map.Append("---+");
        }
        map.AppendLine();


        for (int row = 0; row < Rows; row++)
        {
            string top = "|";
            string bottom = "+";

            for (int col = 0; col < Columns; col++)
            {
                Cell currentCell = GetCell(row, col);
                string body = $" {ContentsOf(currentCell)} ";

                string south = currentCell.IsLinked(currentCell.South) ? "   " : "---";
                const string CORNER = "+";
                bottom += south + CORNER;

                string east = currentCell.IsLinked(currentCell.East) ? " " : "|";
                top += body + east;

            }

            map.AppendLine(top);
            map.AppendLine(bottom);
        }

        return map.ToString();
    }

    protected virtual string ContentsOf(Cell cell)
    {
        return " ";
    }


}
