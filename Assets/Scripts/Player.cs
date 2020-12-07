using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 newPosition;

    public List<Tuple<int, int>> mylist = new List<Tuple<int, int>>();

    public List<Vector3> movements = new List<Vector3>();

    public Tuple<int, int> originalPosition;

    public enum moveDirecion
    {
        up,
        down,
        left,
        right
    }

    private void Awake()
    {
        startPosition = transform.position;
        newPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movePlayer(moveDirecion direction)
    {
        switch (direction)
        {
            case moveDirecion.right:
                newPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + 0.5f);
                break;
            case moveDirecion.left:
                newPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - 0.5f);
                break;
            case moveDirecion.down:
                newPosition = new Vector3(startPosition.x + 1f, startPosition.y, startPosition.z);
                break;
            case moveDirecion.up:
                newPosition = new Vector3(startPosition.x - 1f, startPosition.y, startPosition.z);
                break;
        }

        startPosition = newPosition;
        movements.Add(newPosition);
    }

    public void SetMove()
    {
        mylist.Reverse();
        originalPosition = new Tuple<int, int>(1, 1);

        foreach(Tuple<int, int> list in mylist){

            if(list.Item1 > originalPosition.Item1){
                originalPosition = new Tuple<int, int>(list.Item1, originalPosition.Item2);
                movePlayer(moveDirecion.right);
            }
            if (list.Item1 < originalPosition.Item1)
            {
                originalPosition = new Tuple<int, int>(list.Item1, originalPosition.Item2);
                movePlayer(moveDirecion.left);
            }
            if (list.Item2 > originalPosition.Item2)
            {
                originalPosition = new Tuple<int, int>(originalPosition.Item1, list.Item2);
                movePlayer(moveDirecion.down);
            }
            if (list.Item2 < originalPosition.Item2)
            {
                movePlayer(moveDirecion.up);
                originalPosition = new Tuple<int, int>(originalPosition.Item1, list.Item2);
            }

        }

        StartCoroutine(MoveCoroutine(movements));
    }

    IEnumerator MoveCoroutine(List<Vector3> movement)
    {
        foreach(Vector3 move in movement)
        {
            float timeToStart = Time.time;
            while(Vector3.Distance(transform.position, move) > 0.01)
            {
                transform.position = Vector3.Lerp(transform.position, move, (Time.time - timeToStart) * 10f);
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetTrace(int x, int y)
    {
        mylist.Add(new Tuple<int, int>(x, y));
    }
}
