using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public Vector3Int Position { get; private set; }
    public TetrominoData Data { get; private set; }
    public Vector3Int[] Cells { get; private set; }

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        Board = board;
        Position = position;
        Data = data;

        if (Cells == null)
        {
            Cells = new Vector3Int[data.Cells.Length];
        }

        for (int i = 0; i < Data.Cells.Length; i++)
        {
            Cells[i] = (Vector3Int)Data.Cells[i];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3Int.right);
        }
    }
    
    private void Move(Vector3Int position)
    {
         Vector3Int newPosition = this.Position;
         newPosition.x += position.x;
         newPosition.y += position.y;
    }
}
