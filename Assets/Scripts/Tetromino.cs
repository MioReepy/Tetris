using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I,
    O, 
    T,
    J,
    L,
    S,
    Z
}

[Serializable]
public struct TetrominoData
{
    public Tetromino TetrominoEnum;
    public Tile Tiles ;
    public Vector2Int[] Cells {get; private set;}
    public Vector2Int[,] WallKicks {get; private set;}

    public void Initialize()
    {
        this.Cells = Data.Cells[this.TetrominoEnum];
        this.WallKicks = Data.WallKiks[this.TetrominoEnum];
    }
}