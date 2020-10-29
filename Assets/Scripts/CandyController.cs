using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyController : MonoBehaviour
{
    // CONSTs
    const string STATE_IS_SELECTED = "isSelected";


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


    public void SelectCandy() {
        _isSelected = true;
        _animator.SetBool(STATE_IS_SELECTED, true);
        _spriteRenderer.color = _selectedColor;
        _previusSelected = gameObject.GetComponent<CandyController>();
    }

    public void DeselectCandy() {
        _isSelected = false;
        _animator.SetBool(STATE_IS_SELECTED, false);
        _spriteRenderer.color = Color.white;
        _previusSelected = null;
    }

    private void OnMouseDown() {
        if (_spriteRenderer.sprite == null ||
            BoardManager.sharedInstance.isShifting) {
                return;
            }

            if (_isSelected) {
                DeselectCandy();
            } else { 
                if (_previusSelected ==  null) {
                    SelectCandy();
                } else {
                    _previusSelected.DeselectCandy();
                    //SelectCandy();
                }
            }
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
