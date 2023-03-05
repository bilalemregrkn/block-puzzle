using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 _offset;
    [SerializeField] private LayerMask mask;

    private Transform _currentMovable;
    private Vector3 _homePosition;
    private MyTile _myTile;
    private Piece _piece;

    private void Start()
    {
        _currentMovable = transform.parent;
        _homePosition = transform.position;
        _myTile = GetComponent<MyTile>();
        _piece = transform.parent.GetComponent<Piece>();
    }

    #region Pointer

    public void OnPointerDown(PointerEventData eventData)
    {
        _piece.OnPointerDown();
        SetOffset(eventData);
    }

    private void SetOffset(PointerEventData eventData)
    {
        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        _offset = _currentMovable.position - target;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Move(eventData);
    }

    private void Move(PointerEventData eventData)
    {
        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        target += _offset;
        target.z = 0;
        _currentMovable.position = target;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _piece.OnPointerUp();
    }

    #endregion


    public void SetPositionToHit()
    {
        var hit = Hit();
        var baseTile = hit.transform.GetComponent<MyTile>();
        baseTile.OnMyTile = _myTile;
        var target = hit.transform.position;
        target.z = 0.5f;
        _myTile.SetActiveCollider(false);

        // transform.position = target;
        Animation(target, .3f);
    }

    public void BackHome()
    {
        // transform.position = _homePosition;
        Animation(_homePosition, .3f);
    }

    public RaycastHit2D Hit()
    {
        var origin = transform.position;
        return Physics2D.Raycast(origin, Vector3.forward, 10, mask);
    }

    private async void Animation(Vector3 target, float duration)
    {
        var init = transform.position;
        var passed = 0f;
        while (passed < duration)
        {
            passed += Time.deltaTime;
            var normalize = passed / duration;
            var current = Vector3.Lerp(init, target, normalize);
            transform.position = current;
            await Task.Yield();
        }
    }
    
}