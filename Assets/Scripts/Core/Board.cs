using UnityEngine;
using System.Collections;


public class Board : MonoBehaviour
{
    // Use this for initialization
    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;
    public int m_header = 8;
    public int m_completedRows = 0;
    public ParticlePlayer[] m_rowsGlowFx = new ParticlePlayer[4];

    Transform[,] m_grid;
    void ClearRowFX(int idx, int y)
    {
        if (m_rowsGlowFx[idx])
        {
            m_rowsGlowFx[idx].transform.position = new Vector3(0, y, -2f);
            m_rowsGlowFx[idx].Play();
        }
    }


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


    //check whether the board is empty or not
    bool IsCompleted(int y)
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (!m_grid[x, y])
            {
                return false;
            }
        }
        return true;
    }

    void ClearRow(int y)
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y])
            {
                Destroy(m_grid[x, y].gameObject);
            }
            m_grid[x, y] = null;
        }
    }

    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y])
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    void ShiftRowsDown(int startY)
    {
        for (int i = startY; i < m_height; ++i)
        {
            ShiftOneRowDown(i);
        }
    }

    public IEnumerator ClearAllRows()
    {
        m_completedRows = 0;
        for (int y = 0; y < m_height; ++y)
        {
            if (IsCompleted(y))
            {
                ClearRowFX(m_completedRows, y);
                m_completedRows++;

            }
        }

        yield return new WaitForSeconds(0.5f);
        for (int y = 0; y < m_height; ++y)
        {
            if (IsCompleted(y))
            {
                ClearRow(y);
                ShiftRowsDown(y + 1);
                yield return new WaitForSeconds(0.3f);
                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (m_height - m_header - 1))
            {
                return true;
            }
        }
        return false;
    }

}


