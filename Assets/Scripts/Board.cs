using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }
    public Piece ActivePiece { get; private set; }
    public TetrominoData[] TetrominoDatas;
    public Vector3Int SpawnPosition;  
 
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
        Set(ActivePiece);
    } 

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.Cells.Length; i++)
        {
            Vector3Int tilePosition = piece.Cells[i] + piece.Position;
            Tilemap.SetTile(tilePosition, piece.Data.Tiles);
        }
    }
    
    
}

