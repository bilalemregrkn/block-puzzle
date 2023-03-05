using MyGrid.Code;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 _offset;
    [SerializeField] private LayerMask mask;

    private Transform _currentMovable;
    private Vector3 _homePosition;
    private GridManager _manager;
    private MyTile _myTile;
    private Piece _piece;

    private void Start()
    {
        _currentMovable = transform.parent;
        _homePosition = transform.position;
        _manager = transform.parent.GetComponent<GridManager>();
        _myTile = GetComponent<MyTile>();
        _piece = transform.parent.GetComponent<Piece>();
    }

    #region Pointer

    public void OnPointerDown(PointerEventData eventData)
    {
        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        _offset = _currentMovable.position - target;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        target += _offset;
        target.z = 0;
        _currentMovable.position = target;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var allowSetToGrid = AllowSetToGrid();

        if (allowSetToGrid)
        {
            SetPositionAll();

            _piece.OnSetToGrid();
            Spawner.Instance.Check();

            BaseGrid.Instance.CheckGrid();
        }
        else
        {
            BackHomeAll();
        }
    }

    #endregion

    #region Manager

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

    #endregion


    private void SetPositionToHit()
    {
        var hit = Hit();
        var baseTile = hit.transform.GetComponent<MyTile>();
        baseTile.OnMyTile = _myTile;
        var target = hit.transform.position;
        target.z = 0.5f;
        transform.position = target;
        _myTile.SetActiveCollider(false);
    }

    private void BackHome()
    {
        transform.position = _homePosition;
    }

    private RaycastHit2D Hit()
    {
        var origin = transform.position;
        return Physics2D.Raycast(origin, Vector3.forward, 10, mask);
    }

    // private void FixedUpdate()
    // {
    //     var hit = Hit();
    //     Debug.Log(hit ? $"hit {hit.transform.name}" : "no hit");
    //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10,
    //         hit ? Color.yellow : Color.white);
    // }
}