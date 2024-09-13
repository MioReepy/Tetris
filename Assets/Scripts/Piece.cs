using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public Vector3Int Position { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    
    public int rotationIndex { get; private set; }

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        Board = board;
        Position = position;
        this.data = data;
        rotationIndex = 0;
        
        if (Cells == null)
        {
            Cells = new Vector3Int[data.Cells.Length];
        }

        for (int i = 0; i < data.Cells.Length; i++)
        {
            Cells[i] = (Vector3Int)data.Cells[i];
        }
    }

    private void Update()
    {
        Board.Clear(this);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        Board.Set(this);
    }

    private void HardDrop()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
    }
    
    private bool Move(Vector3Int position)
    {
         Vector3Int newPosition = this.Position;
         newPosition.x += position.x;
         newPosition.y += position.y;

         bool valid = Board.IsValidatePosition(this, newPosition);

         if (valid)
         {
             Position = newPosition;
         }

         return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotationIndex = this.rotationIndex;
        
        rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Vector3 cell = this.Cells[i];

            int x, y;

            switch (this.data.TetrominoEnum)
            {
                case Tetromino.I:
                case Tetromino.J:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) +
                                        (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) +
                                        (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) +
                                         (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) +
                                         (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.Cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
