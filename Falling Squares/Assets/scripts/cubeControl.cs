using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class cubeControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    private float timer = 0;
    private static float normalized = 0;

    public Text plus1;
    public Menu gameOverMenu;
    public Menu gameMenu;
    public MenuManager _menuManager;

    /// <summary>
    /// cubes
    /// </summary>
    public GameObject Cube0;
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;
    public GameObject cubeHolder;
    private Renderer[] render = new Renderer[4];
    private Material[] mat = new Material[4];
    public string[] currentColor { get; set; }
    public int tapThisCube
    {
        get;set;
    }
    public string tapThisColor
    {
        get;set;
    }
    public bool goFlag
    {
        get;set;
    }
    public bool readyFlag
    {
        get;set;
    }
    public int score
    {
        get;set;
    }
    public bool wrongCube
    {
        get;set;
    }
    public bool gameOverFlag
    {
        get;set;
    }
    public bool resetting { get; set; }
    /// <summary>
    /// properties
    /// </summary>
    private float speed = -5;

    private float topOfScreen = 5.9f;
    private float bottomOfScreen = -5.9f;
    /// <summary>
    /// game flags
    /// </summary>
    private bool gameStarted = false;
    private bool gameRunning = false;
    private bool firstCubeDrop = false;
    private bool clickedBoxAtLeastOnce = false;


    /// <summary>
    /// RNG items
    /// </summary>
    private int[] fourRandomNums = { -1, -1, -1, -1 };
    public Color[] fourRandomColors { get; set; }
    // Update is called once per frame
    void Update ()
    {
        resetting = false;
        if (gameStarted == true)
        {
            initialize();
        }
        if(gameRunning == true)
        {
            timer += Time.deltaTime;
            normalized = (float)Math.Floor(timer % 60);
            if (cubeHolder.GetComponent<Rigidbody2D>().position.y < bottomOfScreen && clickedBoxAtLeastOnce == true)
            {
                respawn();
            }
            else if(cubeHolder.GetComponent<Rigidbody2D>().position.y < bottomOfScreen && clickedBoxAtLeastOnce == false)
            {
                gameOverFlag = true;
                wrongCube = false;
            }
            if (normalized >= 1 && readyFlag == true)
            {
                readyFlag = false;
                goFlag = true;
            }
            if (normalized >= 2 && firstCubeDrop == false)
            {
                goFlag = false;

                firstCubeDrop = true;
                respawn();
            }
            checkForClicks();
            endCubeHitAnimation();
        }
        if (gameOverFlag == true)
        {
            //_menuManager.CloseThisMenu(gameMenu);

            _menuManager.ShowMenu(gameOverMenu);
            cubeHolder.GetComponent<Rigidbody2D>().position = new Vector2(0f, topOfScreen);
            cubeHolder.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
        
	}
    public void respawn()
    {
        resetting = true;
        cubeHolder.GetComponent<Rigidbody2D>().position = new Vector2(0f, topOfScreen);
        cubeHolder.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, speed);
        clickedBoxAtLeastOnce = false;
        RNGsetup();
        speed = speed * 1.05f;
    }

    public void startGame()
    {
        gameStarted = true;
    }

    void initialize()
    {
        score = 0;
        gameRunning = true;
        readyFlag = true;
        goFlag = false;
       
        cubeHolder.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

        render[0] = Cube0.GetComponent<Renderer>();
        render[1] = Cube1.GetComponent<Renderer>();
        render[2] = Cube2.GetComponent<Renderer>();
        render[3] = Cube3.GetComponent<Renderer>();


        mat[0] = render[0].material;
        mat[1] = render[1].material;
        mat[2] = render[2].material;
        mat[3] = render[3].material;

        currentColor = new string[4];

        RNGsetup();

        gameStarted = false;
    }

    void RNGsetup()
    {

        fourRandomNums = randNumArray();
        fourRandomColors = randColorArray(fourRandomNums);
        setColors(mat, fourRandomColors);
        chooseRandomCubeToTap();
    }

    int[] randNumArray()
    {
        int[] fourRandomNums = new int[4];
        System.Random randy = new System.Random();
        int newRand = randy.Next(0, 8);
        fourRandomNums[0] = newRand;
        while (true)
        {
            newRand = randy.Next(0, 8);
            if (newRand != fourRandomNums[0])
            {
                fourRandomNums[1] = newRand;
                break;
            }
        }
        while (true)
        {
            newRand = randy.Next(0, 8);
            if (newRand != fourRandomNums[0] && newRand != fourRandomNums[1])
            {
                fourRandomNums[2] = newRand;
                break;
            }
        }
        while (true)
        {
            newRand = randy.Next(0, 8);
            if (newRand != fourRandomNums[0] && newRand != fourRandomNums[1] && newRand != fourRandomNums[2])
            {
                fourRandomNums[3] = newRand;
                break;
            }
        }

        //Debug.Log("first: " + fourRandomNums[0]+ "\tsecond: " + fourRandomNums[1] + "\tthird: " + fourRandomNums[2] + "\tfourth: "+ fourRandomNums[3]);
        return fourRandomNums;

    }

    Color[] randColorArray(int[] fourRandomNums)
    {
        Color[] returnArray = new Color[4];
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("iteration: " + i);
            if (fourRandomNums[i] == 0)
            {
                currentColor[i] = "Blue";
                returnArray[i] = Color.blue;
            }
            else if (fourRandomNums[i] == 1)
            {
                currentColor[i] = "Green";
                returnArray[i] = Color.green;
            }
            else if (fourRandomNums[i] == 2)
            {
                currentColor[i] = "Magenta";
                returnArray[i] = Color.magenta;
            }
            else if (fourRandomNums[i] == 3)
            {
                currentColor[i] = "Black";
                returnArray[i] = Color.black;
            }
            else if (fourRandomNums[i] == 4)
            {
                currentColor[i] = "Yellow";
                returnArray[i] = Color.yellow;
            }
            else if (fourRandomNums[i] == 5)
            {
                currentColor[i] = "Red";
                returnArray[i] = Color.red;
            }
            else if (fourRandomNums[i] == 6)
            {
                currentColor[i] = "Grey";
                returnArray[i] = Color.grey;
            }
            else
            {
                currentColor[i] = "Cyan";
                returnArray[i] = Color.cyan;
            }
        }
        return returnArray;
    }

    void setColors(Material[] mat, Color[] fourRandomColors)
    {
        mat[0].SetColor("_Color", fourRandomColors[0]);
        mat[1].SetColor("_Color", fourRandomColors[1]);
        mat[2].SetColor("_Color", fourRandomColors[2]);
        mat[3].SetColor("_Color", fourRandomColors[3]);
    }

    void chooseRandomCubeToTap()
    {
        System.Random randy = new System.Random();
        tapThisCube = randy.Next(0, 4);
        tapThisColor = currentColor[tapThisCube];
    }

    void checkForClicks()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    char[] parsingChars = { '(', ')' };
                    string[] parsedName = hit.collider.name.Split(parsingChars);
                    int cubeHit = -1;
                    cubeHit = Int32.Parse(parsedName[1]);
                    

                    if (cubeHit == tapThisCube && clickedBoxAtLeastOnce == false)
                    {
                        clickedBoxAtLeastOnce = true;
                        //Debug.Log("hit!");
                        startCubeHitAnimation(clickPosition);

                    }
                    else if (cubeHit != tapThisCube)
                    {
                       
                        gameOverFlag = true;
                        wrongCube = true;
                        //Debug.Log("wrong cube");
                    }
                }
                else
                {
                    //Debug.Log("no hit" + hit.ToString());
                }
            }
        }
        else
        {

        }
    }


    void checkForTaps()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    char[] parsingChars = { '(', ')' };
                    string[] parsedName = hit.collider.name.Split(parsingChars);
                    int cubeHit = -1;
                    cubeHit = Int32.Parse(parsedName[1]);


                    if (cubeHit == tapThisCube && clickedBoxAtLeastOnce == false)
                    {
                        clickedBoxAtLeastOnce = true;
                        //Debug.Log("hit!");
                        startCubeHitAnimation(touchPosition);

                    }
                    else if (cubeHit != tapThisCube)
                    {

                        gameOverFlag = true;
                        wrongCube = true;
                        //Debug.Log("wrong cube");
                    }
                }
                else
                {
                    //Debug.Log("no hit" + hit.ToString());
                }
            }
        else
        {

        }
    }

    void startCubeHitAnimation(Vector3 clickPosition)
    {
        plus1.GetComponent<Rigidbody2D>().position = new Vector2(clickPosition.x, clickPosition.y);
        plus1.GetComponent<Rigidbody2D>().velocity = new Vector2(2.5f - clickPosition.x, 4.5f - clickPosition.y);
    }

    void endCubeHitAnimation()
    {
        if (plus1.GetComponent<Rigidbody2D>().position.y >= 4.5 || plus1.GetComponent<Rigidbody2D>().position.x >= 3.5)
        {
            plus1.GetComponent<Rigidbody2D>().position = new Vector2(-4.5f, 0f);
            plus1.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            score++;
        }
    }

    public void tryAgain()
    {
        resetting = true;
        gameStarted = true;
        gameRunning = false;
        firstCubeDrop = false;
        clickedBoxAtLeastOnce = false;
        goFlag = false;
        readyFlag = true;
        wrongCube = false;
        gameOverFlag = false;
        timer = 0;
        speed = -5;
    }

    public void changeMode()
    {
        gameStarted = false;
        gameRunning = false;
        firstCubeDrop = false;
        clickedBoxAtLeastOnce = false;
        goFlag = false;
        readyFlag = true;
        wrongCube = false;
        gameOverFlag = false;
        timer = 0;
        speed = -5;
    }

    public void test()
    {
        Debug.Log("test");
    }
}
