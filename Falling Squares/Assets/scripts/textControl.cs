using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class textControl : MonoBehaviour {


    // Use this for initialization
    public Text scoreG;
    public Text highScoreG;
    public Text clickThisColor;
    public Text scoreGO;
    public Text highScoreGO;
    public Text gameOverText;

    public enum gameMode { Easy,  Hard, NotSet };
    public gameMode _gameMode = gameMode.NotSet;
    private int score = 0;
    private int highScore = 0;

    public cubeControl _cubeControl = new cubeControl();
    private string tapThisColor;
    private int fakeCube;
    private int actualCube;


    private bool gameStarted = false;

	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (gameStarted == true)
        {
            
            tapThisColor = _cubeControl.tapThisColor;
            //clickThisColor.text = tapThisColor;
            if (_cubeControl.readyFlag == true)
            {
                clickThisColor.color = Color.black;
                clickThisColor.text = "Ready?";
            }
            else if (_cubeControl.goFlag == true)
            {
                clickThisColor.text = "GO!";
            }
            else
            {
                if (_gameMode == gameMode.Hard)
                {
                    hardModeControls();
                }
                else
                {
                    clickThisColor.text = tapThisColor;
                }
            }
            score = _cubeControl.score;
            if (score > highScore)
            {
                highScore = score;
            }
        }
        if(_cubeControl.gameOverFlag == true)
        {
            if (_cubeControl.wrongCube == true)
            {
                gameOverText.text = "Wrong Cube Hit\nGAME OVER";
            }
            else
            {
                gameOverText.text = "No Cube Hit\nGAME OVER";
            }
        }
        scoreG.text = "Score: " + score + "";
        scoreGO.text = scoreG.text;
        highScoreG.text = "High Score: " + highScore;
        highScoreGO.text = highScoreG.text;
        Debug.Log(scoreG.text);
    }

    public void initTexts()
    {
        gameStarted = true;
        scoreG.text = "Score: 0";
        scoreGO.text = scoreG.text;
        highScoreG.text = "High Score: 0";
        highScoreGO.text = highScoreG.text;
        clickThisColor.text = "";
    }


    public void setGameMode(gameMode mode)
    {
        _gameMode = mode;
    }

    public void setGameModeEasy()
    {
        _gameMode = gameMode.Easy;
    }
    public void setGameModeHard()
    {
        _gameMode = gameMode.Hard;
    }

    void hardModeControls()
    {
        if (_cubeControl.resetting == true)
        {
            actualCube = _cubeControl.tapThisCube;
            System.Random rand = new System.Random();
            fakeCube = (actualCube + rand.Next(0, 4)) % 4;
            //Debug.Log("actual: " + actualCube + "\tfake: " + fakeCube);
            clickThisColor.text = _cubeControl.currentColor[fakeCube];
            clickThisColor.color = _cubeControl.fourRandomColors[actualCube];
        }
    }
}
