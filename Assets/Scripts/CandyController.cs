using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                    if (CanSwap()) {
                        SwapCandies(_previusSelected);
                        _previusSelected.FindAllMatches();
                        _previusSelected.DeselectCandy();
                        FindAllMatches(); 

                        StopCoroutine(BoardManager.sharedInstance.FindNullCandies());
                        StartCoroutine(BoardManager.sharedInstance.FindNullCandies());

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
        
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position,direction);

        while (hit.collider != null && hit.collider.GetComponent<CandyController>().id == this.id) {
            matchingCandies.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }

        return matchingCandies;
    }

    private bool ClearMatch(Vector2[] directions) {
        List<GameObject> matchingCandies = new List<GameObject>();

        foreach(Vector2 direction in directions) {
            matchingCandies.AddRange(FindMatch(direction));
        }

        if (matchingCandies.Count() >= BoardManager.MIN_CANDIES_TO_MATCH) {
            foreach(GameObject candy in matchingCandies) {
                candy.GetComponent<SpriteRenderer>().sprite = null;
                candy.GetComponent<Animator>().enabled = false;
            }
            return true;
        }

        return false;
    }

    public void FindAllMatches() {

        if (_spriteRenderer.sprite == null) {
            return;
        }

        bool hMatch = ClearMatch(new Vector2[2]{
            Vector2.left, 
            Vector2.right
        });

        bool vMatch = ClearMatch(new Vector2[2]{
            Vector2.up,
            Vector2.down
        });

        if (hMatch || vMatch) {
            this._spriteRenderer.sprite = null;
            this._animator.enabled = false;
        }
    }



}
