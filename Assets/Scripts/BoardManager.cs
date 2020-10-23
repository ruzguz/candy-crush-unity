using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // General vars
    public static BoardManager sharedInstance;
    public List<Sprite> prefabs = new List<Sprite>();
    public GameObject currentCandy;
    public int xSize, ySize;

    private GameObject[,] candies;

    public bool isShifting { get; set; }



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

        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                // Intantiating new candy
                GameObject newCandy = Instantiate(currentCandy,
                                                  new Vector3(startX+(offset.x*x), 
                                                          startY+(offset.y*y),
                                                          0),
                                                  currentCandy.transform.rotation
                                                  );
                newCandy.name = string.Format("Candy[{0}][{1}]", x, y);
                candies[x, y] = newCandy;
            }
        }


    }
}
