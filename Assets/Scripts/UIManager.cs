using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager sharedInstance;

    public Text scoreText;
    public Text movesText;
    private int _moveCounter;
    private int _score;

    public int Score 
    {
        get 
        {
            return _score;
        }
        set 
        {
            _score = value;
            scoreText.text = "Score: "+_score;
        }
    }

    public int MoveCounter 
    {
        get
        {
            return _moveCounter;
        }

        set 
        {
            _moveCounter = value;
            movesText.text = "Moves: "+_moveCounter;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        } else {
            Destroy(this);
        }

        _moveCounter = 30;
        movesText.text = "Moves: "+_moveCounter;

        _score = 0;
        scoreText.text = "Score: "+_score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
