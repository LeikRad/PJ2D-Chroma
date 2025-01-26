using System;
using UnityEngine;

public class Splat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum SplatLocation
    {
        Foreground,
        Background
    }
    
    Color[] colors = new Color[]
    {
        new Color(1, 0, 0, 1), // Red
        new Color(0, 1, 0, 1), // Green
        new Color(0.3f, 0.6f, 1, 1), // Blue
        new Color(1, 1, 0, 1), // Yellow
        new Color(1, 0, 1, 1)  // Pink
    };
    
    public float _minSizeMod = 0.8f;
    public float _maxSizeMod = 1.5f;
    public Sprite[] _sprites;
    private SplatLocation _splatLocation;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // get material with name "Sprites-Unlit-Default"
    }

    public void Initialize(SplatLocation splatLocation)
    {
        this._splatLocation = splatLocation;
        SetSprite();
        SetSize();
        SetRotation();
        SetLocationProperties();
    }
    
    private void SetSprite()
    {
        _spriteRenderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Length)];
    }
    
    private void SetSize()
    {
        float sizeMod = UnityEngine.Random.Range(_minSizeMod, _maxSizeMod);
        transform.localScale *= sizeMod;
    }
    
    private void SetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
    }
    
    private void SetLocationProperties()
    {
        if (_splatLocation == SplatLocation.Foreground)
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            _spriteRenderer.color = RandomColor();  
            _spriteRenderer.sortingOrder = 1;
        }
    }
    
    private Color RandomColor()
    {
        return colors[UnityEngine.Random.Range(0, colors.Length)];
    }
}
