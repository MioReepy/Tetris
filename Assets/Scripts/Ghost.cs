using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board MainBoard;
    public Piece TrackingPiece;
    
    public Tilemap Tilemaps { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public Vector3Int Position { get; private set; }

    private void Awake()
    {
        Tilemaps = GetComponentInChildren<Tilemap>();
        Cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Vector3Int tilePosition = Cells[i] + Position;
            Tilemaps.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Cells[i] = TrackingPiece.Cells[i];
        }
    }
    
    private void Drop()
    {
        Vector3Int tilePosition = TrackingPiece.Position;
        
        int current = tilePosition.y;
        int bottom = -MainBoard.BoardSize.y / 2 - 1;
        
        MainBoard.Clear(TrackingPiece);

        for (int row = current; row >= bottom; row--)
        {
            tilePosition.y = row;

            if (MainBoard.IsValidatePosition(TrackingPiece, tilePosition))
            {
                this.Position = tilePosition;
            }
            else
            {
                break;
            }
        }
        
        MainBoard.Set(TrackingPiece);
    }

    private void Set()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Vector3Int tilePosition = Cells[i] + Position;
            this.Tilemaps.SetTile(tilePosition, tile);
        }
    }
}