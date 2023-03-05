using UnityEngine;

public class Slot : MonoBehaviour
{
    private Piece _piece;

    public bool IsEmpty() => _piece == null;
    
    public void Release()
    {
        _piece = null;
    }

    public void SetPiece(Piece current)
    {
        _piece = current;
    }
}