using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Rowcol {
    public int row;
    public int column;
    public Rowcol(int r = 0, int c = 0) {
        row = r;
        column = c;
    }

    public static Rowcol operator -(Rowcol rc) => new Rowcol(-rc.row, -rc.column);

    public static Rowcol operator+(Rowcol rc1, Rowcol rc2)
        => new Rowcol(rc1.row + rc2.row, rc1.column + rc2.column);

    public static Rowcol operator-(Rowcol rc1, Rowcol rc2) => rc1 + (-rc2);

    public override string ToString() {
        return "(" + row.ToString() + ", " + column.ToString() + ")";
    }
}

public class CustomGrid<T> where T : class {
    public int Width { get; set; }
    public int Height { get; set; }
    public Vector2 OriginPosition { get; set; }
    private T[,] _gridArray;
    private float _offsetX;
    private float _offsetY;
    private static readonly int[] DirectionX = new int[]{-1, -1, -1, 0, 0, 1, 1, 1};
    private static readonly int[] DirectionY = new int[]{-1, 0, 1, -1, 1, -1, 0, 1};

    public CustomGrid(int width, int height, Vector3 center, float offsetX, float offsetY) {
        Width = width;
        Height = height;
        Vector3 origin = center;
        origin.x -= Width * offsetX * 0.5f;
        origin.y += Height * offsetY * 0.5f;
        OriginPosition = origin;

        _offsetX = offsetX;
        _offsetY = offsetY;

        _gridArray = new T[height, width];
    }

    public T GetElement(int row, int column) {
        T result = null;
        if (IsValidRowcol(row, column)) {
            result = _gridArray[row, column];
        }
        return result;
    }

    public T GetElement(Rowcol rowcol) {
        return GetElement(rowcol.row, rowcol.column);
    }

    public void SetElement(int row, int column, T value) {
        _gridArray[row, column] = value;
    }

    public Vector3 RowcolToPoint(int row, int column) {
        float x = column * _offsetX;
        float y = row * _offsetY;
        return (Vector3)OriginPosition + new Vector3(x, -y);
    }

    public Vector3 RowcolToPointCenter(int row, int column) {
        float x = column * _offsetX + _offsetX * 0.5f;
        float y = row * _offsetY + _offsetY * 0.5f;
        return (Vector3)OriginPosition + new Vector3(x, -y);
    }

    public Vector3 RowcolToPointCenter(Rowcol rowcol) {
        return RowcolToPointCenter(rowcol.row, rowcol.column);
    }

    public Rowcol PointToRowcol(Vector2 position) {
        position -= OriginPosition;
        position.y = -position.y;

        int row = Mathf.FloorToInt(position.y / _offsetY);
        int col = Mathf.FloorToInt(position.x / _offsetX);

        return new Rowcol(row, col);
    }

    public bool IsValidRowcol(int row, int column) {
        return ((row >= 0) && (row < Height) && (column >= 0) && (column < Width));
    }

    public bool IsValidRowcol(Rowcol rowcol) {
        return IsValidRowcol(rowcol.row, rowcol.column);
    }

    public List<Rowcol> GetAdjustNode(int row, int column) {
        List<Rowcol> rowcolList = new List<Rowcol>();

        int numOfDirections = DirectionX.Length;
        for (int directionIndex = 0; directionIndex < numOfDirections; ++directionIndex) {
            int nextRow = row + DirectionY[directionIndex];
            int nextCol = column + DirectionX[directionIndex];

            if (GetElement(nextRow, nextCol) != null) {
                rowcolList.Add(new Rowcol(nextRow, nextCol));
            }
        }
        return rowcolList;
    }
}