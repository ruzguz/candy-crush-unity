using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyController : MonoBehaviour
{
    // CONSTs
    const string STATE_IS_SELECTED = "isSelected";
    const int MIN_CANDIES_TO_MATCH = 2;


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
                    if (CanSwap()) {
                        SwapCandies(_previusSelected);
                        _previusSelected.DeselectCandy();
                    } else {
                        _previusSelected.DeselectCandy();
                        SelectCandy();
                    }
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
                                                             "AnimationController") as RuntimeAnimatorController;
    }

    // Funciton to swap candies
    public void SwapCandies(CandyController newCandy) {
        // Check if the candies are the same
        if (this._spriteRenderer.sprite == newCandy._spriteRenderer.sprite) {
            return;
        }

        // Swapping animator 
        RuntimeAnimatorController auxAnimator = this._animator.runtimeAnimatorController;
        this._animator.runtimeAnimatorController = newCandy._animator.runtimeAnimatorController;
        newCandy._animator.runtimeAnimatorController = auxAnimator;

        // Swapping ids
        int auxID = this.id;
        this.id = newCandy.id;
        newCandy.id = auxID;


    }

    // Function to get a neightbor
    private GameObject GetNeightbor(Vector2 direction) {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position,
                                             direction);

        if (hit.collider != null) {
            return hit.collider.gameObject;
        } else {
            return null;
        }
    }

    // Function to get all neightbors
    private List<GameObject> GetAllNeightbors() {
        List<GameObject> neightbors = new List<GameObject>();
        foreach( Vector2 direction in adjacentDirections) {
            neightbors.Add(GetNeightbor(direction));
        }

        return neightbors;
    }

    bool CanSwap() {
        return GetAllNeightbors().Contains(_previusSelected.gameObject);
    }

    // Function to find match
    private List<GameObject> FindMatch(Vector2 direction, bool isFirstCandy = true) {
        List<GameObject> matchingCandies = new List<GameObject>();
        
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, 
                                             direction);
        
        // Check if it doesn't have a neightbor or if the neightbor is a different candy
        if (hit.collider == null || hit.collider.GetComponent<CandyController>().id == this.id) {
            if (!isFirstCandy) 
                matchingCandies.Add(this.gameObject);
            return matchingCandies;
        }

        // recursive call in each direction
        matchingCandies.AddRange(hit.collider.GetComponent<CandyController>().FindMatch(direction, false));

        return matchingCandies;
    }
}
