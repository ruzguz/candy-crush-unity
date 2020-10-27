using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyController : MonoBehaviour
{

    // General vars
    private static Color _selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static CandyController _previusSelected = null;
    public int id;

    // Component vars
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Aux vars
    private bool _isSelected = false;
    private Vector2[] adjacentDirections = new Vector2[] {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update Sprite and Animator 
    public void SetGraphycs(Sprite s, string candy) {
        _spriteRenderer.sprite = s;
        _animator.runtimeAnimatorController = Resources.Load("Animations/Candies/"+
                                                             candy+
                                                             "/"+
                                                             candy+
                                                             "AnimationController") as RuntimeAnimatorController;;

    }
}
