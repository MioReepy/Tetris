using UnityEngine;

public class Board : MonoBehaviour
{
    public TetrominoData[] TetrominoDatas;

    private void Awake()
    {
        for (int i = 0; i < TetrominoDatas.Length; i++)
        {
            TetrominoDatas[i].Initialize();
        }
    }
}

