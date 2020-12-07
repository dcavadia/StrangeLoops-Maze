using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{

    private Grid grid;

    private const int MAZE_HEIGHT = 5;
    private const int MAZE_WEIGHT = 5;

    public GameObject Player;

    public Player currentPlayer;


    private void Awake()
    {
        grid = new Grid(MAZE_HEIGHT, MAZE_WEIGHT);

        AldousBroder.GenerateMaze(grid);

        BuildGrid(grid);

        Player = Instantiate(Player, new Vector3(0f, 1.33f, 0f), Quaternion.identity);

        currentPlayer = Player.GetComponent<Player>();

        //grid.MazeToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Plane p = new Plane(Camera.main.transform.forward, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100))
            {
                setStartGoal(hit.collider.gameObject.name);
            }

        }
        
    }

    private void BuildGrid(Grid grid)
    {

        float gridFloorHeight = 0.1f;
        float cellSize = 2f;
        float wallHeight = 2f;


        for (int row = 0; row < grid.Rows; row++)
        {
            for (int col = 0; col < grid.Columns;col++)
            {

                GameObject gridFloor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gridFloor.name = string.Format("{0},{1}", row, col);
                gridFloor.transform.position = new Vector3(row * cellSize, gridFloorHeight, col * cellSize);
                gridFloor.transform.localScale = new Vector3(cellSize, gridFloorHeight, cellSize);


                Cell actualCell = grid.GetCell(row, col);

                //if not linked, create wall
                if (!actualCell.IsLinked(actualCell.East))
                {
                    GameObject eastWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    eastWall.name = "East Wall";
                    eastWall.transform.position = new Vector3(row * cellSize, wallHeight / 2, (col * cellSize) - (cellSize / 2) + cellSize);
                    eastWall.transform.localScale = new Vector3(cellSize, wallHeight, gridFloorHeight);
                }

                //if not linked, create wall
                if (!actualCell.IsLinked(actualCell.North))
                {
                    GameObject northWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    northWall.name = "North Wall";
                    northWall.transform.position = new Vector3((row * cellSize) - (cellSize / 2), wallHeight / 2, col * cellSize);
                    northWall.transform.localScale = new Vector3(gridFloorHeight, wallHeight, cellSize);
                }


            }
        }

        float totalLenght = cellSize * grid.Rows;

        
        GameObject southWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        southWall.name = "South Wall";
        southWall.transform.position = new Vector3(totalLenght - (cellSize / 2), wallHeight / 2 , (totalLenght / 2) - (cellSize / 2));
        southWall.transform.localScale = new Vector3(gridFloorHeight, wallHeight, totalLenght);

        GameObject westWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        westWall.name = "West Wall";
        westWall.transform.position = new Vector3((totalLenght / 2) - (cellSize / 2), wallHeight / 2, -(cellSize / 2));
        westWall.transform.localScale = new Vector3(totalLenght, wallHeight, gridFloorHeight);


    }


    private void setStartGoal(string goal)
    {
        string[] Data = goal.Split(',');



        int goalRow = int.Parse(Data[0]) + 1;
        int goalCol = int.Parse(Data[1]) + 1;

        Debug.Log("ROW: " + goalRow);
        Debug.Log("COL: " + goalCol);

        string maze = grid.MazeToString();

        string[] mazeLines = maze.ToString().Split(Environment.NewLine.ToCharArray());

        StringBuilder strBuilder = new StringBuilder(mazeLines[2]);
        strBuilder[1] = 'A';
        mazeLines[2] = strBuilder.ToString();

        StringBuilder strBuilder2 = new StringBuilder(mazeLines[((goalRow * 2)-1)*2]);
        strBuilder2[((goalCol * 2) - 1) * 2] = 'B';
        mazeLines[((goalRow * 2) - 1) * 2] = strBuilder2.ToString();

        List<string> map = new List<string>
        {
            mazeLines[0],
            mazeLines[2],
            mazeLines[4],
            mazeLines[6],
            mazeLines[8],
            mazeLines[10],
            mazeLines[12],
            mazeLines[14],
            mazeLines[16],
            mazeLines[18],
            mazeLines[20]
        };


         Debug.Log(mazeLines[0]);
         Debug.Log(mazeLines[2]);
         Debug.Log(mazeLines[4]);
         Debug.Log(mazeLines[6]);
         Debug.Log(mazeLines[8]);
         Debug.Log(mazeLines[10]);
         Debug.Log(mazeLines[12]);
         Debug.Log(mazeLines[14]);
         Debug.Log(mazeLines[16]);
         Debug.Log(mazeLines[18]);
         Debug.Log(mazeLines[20]);

        Debug.Log(grid.MazeToString());

        SetPoints(map);

        


    }

    private void SetPoints(List<String> maze)
    {

        Tile start = new Tile();
        start.Y = maze.FindIndex(x => x.Contains("A"));
        start.X = maze[start.Y].IndexOf("A");

        Tile finish = new Tile();
        finish.Y = maze.FindIndex(x => x.Contains("B"));
        finish.X = maze[finish.Y].IndexOf("B");

        start.SetDistance(finish.X, finish.Y);

        List<Tile> activeTiles = new List<Tile>();
        activeTiles.Add(start);
        List<Tile> visitedTiles = new List<Tile>();

        while (activeTiles.Any())
        {
            Tile checkTile = activeTiles.OrderBy(x => x.CostDistance).First();


            if(checkTile.X == finish.X && checkTile.Y == finish.Y)
            {
                Debug.Log("destination!");

                Tile tile = checkTile;
                while (true) {

                    Debug.Log($"{tile.X} : {tile.Y}");
                    if(maze[tile.Y][tile.X] == 'B')
                    {
                        currentPlayer.SetTrace(tile.X, tile.Y);
                    } if (maze[tile.Y][tile.X] == ' ')
                    {
                        var newMapRow = maze[tile.Y].ToCharArray();
                        newMapRow[tile.X] = '*';
                        maze[tile.Y] = new string(newMapRow);

                        currentPlayer.SetTrace(tile.X, tile.Y);
                    }
                    tile = tile.Parent;
                    if (tile == null)
                    {
                        currentPlayer.SetMove();
                        maze.ForEach(x => Debug.Log(x));
                        Debug.Log("Done");
                        return;
                    }

                }

            }

            visitedTiles.Add(checkTile);
            activeTiles.Remove(checkTile);

            List<Tile> walkableTiles = GetWalkableTiles(maze, checkTile, finish);

            foreach(Tile walkableTile in walkableTiles)
            {

                if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    continue;

                if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                {
                    Tile existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                    if(existingTile.CostDistance > checkTile.CostDistance)
                    {
                        activeTiles.Remove(existingTile);
                        activeTiles.Add(walkableTile);
                    }
                }
                else
                {
                    activeTiles.Add(walkableTile);
                }


            }

        }


    }

    private static List<Tile> GetWalkableTiles(List<string> maze, Tile currentTile, Tile targetTile)
    {

        List<Tile> possibleTiles = new List<Tile>()
        {
            new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1},
            new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
            new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1},
            new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1},
        };

        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

        int maxX = maze.First().Length - 1;
        int maxY = maze.Count - 1;

        return possibleTiles
            .Where(tile => tile.X >= 0 && tile.X <= maxX)
            .Where(tile => tile.X >= 0 && tile.X <= maxX)
            .Where(tile => maze[tile.Y][tile.X] == ' ' || maze[tile.Y][tile.X] == 'B')
            .ToList();

    }



}
