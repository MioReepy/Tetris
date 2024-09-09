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
    public Tetromino tetrominoEnum;
    public Tile Tiles;
    public Vector2Int[] Cells;

    public void Initialize()
    {
        this.Cells = Data.Cells[this.tetrominoEnum];
    }
}