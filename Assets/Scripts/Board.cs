using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public Piece ActivePiece { get; private set; }
    public TetrominoData[] TetrominoDatas;
    public Vector3Int SpawnPosition;  
    public Vector2Int BoardSize = new Vector2Int(10, 20);

    private RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-BoardSize.x / 2, -BoardSize.y / 2);
            return new RectInt(position, BoardSize);
        }
    }

    private void Awake()
    {
        Tilemap = GetComponentInChildren<Tilemap>();
        ActivePiece = GetComponentInChildren<Piece>();
        
        for (int i = 0; i < TetrominoDatas.Length; i++)
        {
            TetrominoDatas[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    private void SpawnPiece()
    {
        int random = Random.Range(0, this.TetrominoDatas.Length);
        TetrominoData data = TetrominoDatas[random];
        
        ActivePiece.Initialize(this, SpawnPosition, data);

        if (IsValidatePosition(ActivePiece, SpawnPosition))
        {
            Set(ActivePiece);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Tilemap.ClearAllTiles();
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, piece.data.Tiles);
        }
    }
    
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, null);
        }
    }
    
    public bool IsValidatePosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
            
            if (this.Tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }
        
        return true;
    }

    // public void ClearLines()
    // {
    //     RectInt bounds = this.Bounds;
    //     int row = bounds.yMax;
    // }
}

