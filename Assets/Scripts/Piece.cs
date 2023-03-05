using UnityEngine;

public class Piece : MonoBehaviour
{
    private Slot _slot;

    public void OnSetToGrid()
    {
        _slot.Release();
    }

    public void OnSpawn(Slot slot)
    {
        _slot = slot;
    }
}