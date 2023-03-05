using MyGrid.Code;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private GridManager _manager;
    private Slot _slot;

    private void Start()
    {
        _manager = GetComponent<GridManager>();
    }

    public void OnSetToGrid()
    {
        _slot.Release();
    }

    public void OnSpawn(Slot slot)
    {
        _slot = slot;
    }

    private void SetActiveOutline(bool enable)
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;
            myTile.SetActiveOutline(enable);
        }
    }

    private bool AllowSetToGrid()
    {
        var allowSetToGrid = true;
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;

            var myTile = (MyTile)tile;
            var hit = myTile.Movable.Hit();
            if (!hit)
            {
                allowSetToGrid = false;
                break;
            }

            //OnMyTile
            var baseTile = hit.transform.GetComponent<MyTile>();
            if (baseTile.OnMyTile)
            {
                allowSetToGrid = false;
                break;
            }
        }

        return allowSetToGrid;
    }

    private void SetPositionAll()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;
            myTile.Movable.SetPositionToHit();
        }
    }

    private void BackHomeAll()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;
            myTile.Movable.BackHome();
        }
    }

    public void OnPointerDown()
    {
        SetActiveOutline(true);
    }

    public void OnPointerUp()
    {
        SetActiveOutline(false);

        var allowSetToGrid = AllowSetToGrid();

        if (allowSetToGrid)
        {
            SetPositionAll();

            OnSetToGrid();
            Spawner.Instance.Check();

            BaseGrid.Instance.CheckGrid();
        }
        else
        {
            BackHomeAll();
        }
    }
}