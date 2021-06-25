using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockLogic : MonoBehaviour
{
    private float lastTime;
    private float second = 0.85f;
    public static int width = 10;
    public static int height = 21;

    private Vector2 startTouchPosition, endTouchPosition;
    public float SWIPE_THRESHOLD = 100f;

    public int lines = 0;
    public bool gameOver = false;

    public Vector3 rotationPoint;
 
    private static Transform[,] grid = new Transform[width, height];

    void Update()
    {
        if (Time.time - lastTime > second)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            if (!CanMove() && gameOver == false && Time.timeScale == 1)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                FillGrid();
                CheckGridLine2();
                Debug.Log(lines);
                FindObjectOfType<ButtonMethods>().UpdateScore(lines);
                lines = 0;
                this.enabled = false;
                FindObjectOfType<SpawnBlocks>().Spawn();
            }
            lastTime = Time.time;
        }

        if (gameOver)
        {
            Time.timeScale = 0;
            FindObjectOfType<ButtonMethods>().CheckForHighScore();
            FindObjectOfType<ButtonMethods>().ActivatePanel();
        }

        #region BlockMovement
        if (Time.timeScale == 1)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(0).position;

                float distanceX = Mathf.Abs(endTouchPosition.x - startTouchPosition.x);
                float distanceY = Mathf.Abs(endTouchPosition.y - startTouchPosition.y);

                //MOVE LEFT
                if (endTouchPosition.x < startTouchPosition.x && distanceX > SWIPE_THRESHOLD && distanceX > distanceY)
                {
                    FindObjectOfType<Music>().SwipeSound();
                    transform.position = new Vector2(transform.position.x - 1f, transform.position.y);
                    if (!CanMove()) transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
                }
                //MOVE RIGHT
                if (endTouchPosition.x > startTouchPosition.x && distanceX > SWIPE_THRESHOLD && distanceX > distanceY)
                {
                    FindObjectOfType<Music>().SwipeSound();
                    transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
                    if (!CanMove()) transform.position = new Vector2(transform.position.x - 1f, transform.position.y);
                }
                //ROTATE BLOCK
                if (endTouchPosition.y > startTouchPosition.y && distanceY > SWIPE_THRESHOLD && distanceY > distanceX)
                {
                    FindObjectOfType<Music>().SwipeSound();
                    this.transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                    if (!CanMove()) this.transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
                //SPEED UP BLOCK
                if (endTouchPosition.y < startTouchPosition.y && distanceY > SWIPE_THRESHOLD && distanceY > distanceX)
                {
                    FindObjectOfType<Music>().SwipeSound();
                    second = 0.1f;
                }
            }
        }
        #endregion
    }

    //ADD BLOCK TO GRID
    void FillGrid()
    {
        foreach(Transform children in transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);
            
            if(grid[x,y] == false && y < 19)
            {
                grid[x, y] = children;
            }
            else
            {
                gameOver = true;
            }
        }
    }

    public int CheckGridLine2()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, i] == null)
                {
                    break;
                }
                else if (grid[j, i] != null && j == width - 1)
                {
                    DeleteRow(i);
                    RowDown(i);
                    lines += 1;
                    CheckGridLine2();
                }
            }
        }
        return lines;
    }

    void DeleteRow(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j, i] = null;
            FindObjectOfType<Music>().DestroySound();
        }
    }

    void RowDown(int i)
    {
        for(int y = i; y < height; y++)
        {
            for(int j = 0; j < width; j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    //IF MOVE IS VALID
    public bool CanMove()
    {
        foreach(Transform children in transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);

            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return false;
            }

            if(grid[x, y])
            {
                return false;
            }
        }
        return true;
    }
}
