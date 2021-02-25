using UnityEngine;
using System.Collections;


public class Board : MonoBehaviour
{

    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;

    public int m_header = 8;


    // Use this for initialization

    Transform[,] m_grid;

    void Awake()
    {
        m_grid = new Transform[m_width, m_height];
    }

    void Start()
    {
        DrawEmptyCells();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // check whether a position is within the board
    bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0 && y < m_height);
    }

    //Check 4 collisions
    bool IsOccupied(int x, int y, Shape shape)
    {
        return (m_grid[x, y] && m_grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            if (!IsWithinBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }
            // check whether pos is occupied or not.
            if (IsOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }
        }

        return true;
    }


    // Draw the entire board with empty sprite
    void DrawEmptyCells()
    {
        // check if emprtSprite is attached to board
        if (m_emptySprite != null)
        {
            for (int y = 0; y < m_height - m_header; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Transform clone;
                    clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = "Board Space ( x = " + x.ToString() + " , y =" + y.ToString() + " )";
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("WARNING!  Please assign the emptySprite object!");
        }

    }

    public void StoreShapeInGrid(Shape shape)
    {
        if (!shape)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            m_grid[(int)pos.x, (int)pos.y] = child;
        }
    }

}
