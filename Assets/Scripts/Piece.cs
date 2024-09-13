using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public Vector3Int Position { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        Board = board;
        Position = position;
        this.data = data;
        rotationIndex = 0;
        
        stepTime = Time.time + stepDelay;
        lockTime = 0f;
        
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
        
        lockTime += Time.deltaTime;

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
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if (Time.time > stepTime)
        {
            Step();
        }
        
        Board.Set(this);
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;
        Move(Vector2Int.down);

        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        Board.Set(this);
        Board.ClearLines();
        Board.SpawnPiece();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        
        Lock();
    }
    
    private bool Move(Vector2Int position)
    {
         Vector3Int newPosition = this.Position;
         newPosition.x += position.x;
         newPosition.y += position.y;

         bool valid = Board.IsValidatePosition(this, newPosition);

         if (valid)
         {
             Position = newPosition;
             lockTime = 0f;
         }

         return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotationIndex = this.rotationIndex;
        
        rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);
        
        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotationIndex;
        }
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

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.WallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.WallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        
        return false;
    }
    
    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }
        
        return Wrap(wallKickIndex, 0, data.WallKicks.GetLength(0));
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
