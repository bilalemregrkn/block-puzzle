using MyGrid.Code;
using UnityEngine;

public class MyTile : TileController
{
    public Movable Movable { get; private set; }
    public MyTile OnMyTile; //{ get; set; }

    private Collider2D _collider;

    private void Start()
    {
        Movable = GetComponent<Movable>();
        _collider = GetComponent<Collider2D>();
    }

    public void SetActiveCollider(bool active)
    {
        _collider.enabled = active;
    }
}