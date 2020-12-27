using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // General vars
    public static BoardManager sharedInstance;
    public List<Sprite> sprites = new List<Sprite>();
    private string[] _animators = { "Sun", "Boat", "Wave", "Watermelon", "Conch", "Fish" };
    public GameObject currentCandy;
    public int xSize, ySize;

    private GameObject[,] candies;

    private CandyController _selectedCandy;

    public bool isShifting { get; set; }

    public const int MIN_CANDIES_TO_MATCH = 2;



    // Start is called before the first frame update
    void Start()
    {
        // Singleton validation
        if (sharedInstance ==  null) {
            sharedInstance = this;
        } else {
            Destroy(this);
        }

        // Create game board
        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Init the board
    private void CreateInitialBoard(Vector2 offset)
    {
        candies = new GameObject[xSize,ySize];

        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        int idx = -1;

        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                // Intantiating new candy
                GameObject newCandy = Instantiate(currentCandy,
                                                  new Vector3(startX+(offset.x*x), 
                                                          startY+(offset.y*y),
                                                          0),
                                                  currentCandy.transform.rotation
                                                  );

                // Check candy is not the same as neighbour
                do {
                    idx = Random.Range(0, sprites.Count);
                } while( 
                    (x > 0 && idx == candies[x-1,y].GetComponent<CandyController>().id) ||
                    (y > 0 && idx == candies[x,y-1].GetComponent<CandyController>().id)
                    );

                // Setting new candy values
                Sprite s = sprites[idx];
                string candyName = _animators[idx];
                newCandy.GetComponent<CandyController>().SetGraphycs(s,candyName);
                newCandy.GetComponent<CandyController>().id = idx;

                newCandy.name = string.Format("Candy[{0}][{1}]", x, y);

                newCandy.transform.parent = this.transform;
                candies[x, y] = newCandy;
            }
        }
    }

    public IEnumerator FindNullCandies() {
        for (int x = 0; x < xSize ; x++) {
            for (int y = 0; y < ySize ; y++) {
                if (candies[x,y].GetComponent<Animator>().enabled == false) {
                    yield return StartCoroutine(MakeCandiesFall(x,y));
                    break;
                }
            }
        }
    }

    private IEnumerator MakeCandiesFall(int x, int yStart, float shiftDelay = 0.03f) {
        isShifting = true;

        for (int y = yStart; y < ySize; y++) {
            CandyController clearedCandy = candies[x,y].GetComponent<CandyController>();
            if (candies[x,y].GetComponent<Animator>().enabled == false) {
                // If the candy is cleared we gonna make other candies fall
                for (int i = y+1; i < ySize; i++) {
                    yield return new WaitForSeconds(shiftDelay);
                    CandyController fallingCandy = candies[x,i].GetComponent<CandyController>();
                    if (candies[x,i].GetComponent<Animator>().enabled == true) {
                        // set animator for cleared candy
                        clearedCandy.SetGraphycs(null, _animators[fallingCandy.id]);
                        clearedCandy.GetComponent<Animator>().enabled = true;
                        // disable animator for falling candy
                        fallingCandy.GetComponent<Animator>().enabled = false;
                        break;
                    }
                }
            }
        }

        isShifting = false;

    }
}
