using System.Collections.Generic;
using MyGrid.Code;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private List<Piece> items;
    [SerializeField] private List<Slot> slots;

    private void Start()
    {
        Check();
    }

    public void Check()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty()) continue;

            var index = Random.Range(0, items.Count);
            var item = items[index];
            var piece = Instantiate(item, slot.transform);
            piece.transform.localPosition = Vector3.zero;
            piece.OnSpawn(slot);
            slot.SetPiece(piece);
        }
    }
}