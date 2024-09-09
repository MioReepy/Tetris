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

internal struct TetrominoData
{
    public Tetromino tetromino;
    public Tile Tiles;
}