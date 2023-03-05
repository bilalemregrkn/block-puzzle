using System;
using System.Threading.Tasks;
using MyGrid.Code;
using UnityEngine;

public class MyTile : TileController
{
    public Movable Movable { get; private set; }
    public MyTile OnMyTile { get; set; }

    private SpriteRenderer _outline;
    private Collider2D _collider;

    private void Start()
    {
        Movable = GetComponent<Movable>();
        _collider = GetComponent<Collider2D>();
        _outline = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void SetActiveCollider(bool active)
    {
        _collider.enabled = active;
    }

    public void SetActiveOutline(bool enable)
    {
        _outline.enabled = enable;
    }

    public async void Destroy(float delay)
    {
        await Scale(Vector3.zero, .5f, delay);
        Destroy(gameObject);
    }

    private async Task Scale(Vector3 target, float duration, float delay)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        var init = transform.localScale;
        var passed = 0f;
        while (passed < duration)
        {
            passed += Time.deltaTime;
            var normalize = passed / duration;
            var current = Vector3.Lerp(init, target, normalize);
            transform.localScale = current;
            await Task.Yield();
        }
    }
}