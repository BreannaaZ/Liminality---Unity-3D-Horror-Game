using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;

public class MazeGenerator2 : MonoBehaviour
{
    [SerializeField]
    private MazeCell2 _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell2[,] _mazeGrid;

    void Start()
    {
        _mazeGrid = new MazeCell2[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                _mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);
        GetComponent<NavMeshSurface>().BuildNavMesh();

        AddEntranceAndExit();
    }

    private void GenerateMaze(MazeCell2 previousCell, MazeCell2 currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell2 nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell2 GetNextUnvisitedCell(MazeCell2 currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell2> GetUnvisitedCells(MazeCell2 currentCell)
    {
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell2 previousCell, MazeCell2 currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.localPosition.x < currentCell.transform.localPosition.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.localPosition.x > currentCell.transform.localPosition.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.localPosition.z < currentCell.transform.localPosition.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.localPosition.z > currentCell.transform.localPosition.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void AddEntranceAndExit()
    {
        // Always bottom-left corner (0, 0) for entrance
        MazeCell2 entrance = _mazeGrid[0, 0];
        ClearOuterWall(entrance);

        // Always top-right corner (width - 1, depth - 1) for exit
        MazeCell2 exit = _mazeGrid[_mazeWidth - 1, _mazeDepth - 1];
        ClearOuterWall(exit);
    }

    private void ClearOuterWall(MazeCell2 cell)
    {
        int x = (int)cell.transform.localPosition.x;
        int z = (int)cell.transform.localPosition.z;

        if (x == 0)
        {
            cell.ClearLeftWall();
        }
        else if (x == _mazeWidth - 1)
        {
            cell.ClearRightWall();
        }
        else if (z == 0)
        {
            cell.ClearBackWall();
        }
        else if (z == _mazeDepth - 1)
        {
            cell.ClearFrontWall();
        }
    }

}