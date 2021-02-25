using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    Board m_gameBoard;
    Spawner m_spawner;
    // keep track of the current falling Shape
    Shape m_activeShape;

    public GameObject m_gameOverPanel;

    bool m_gameOvered = false;
    //Drop/Key Intervals
    float m_dropInterval = 0.9f;
    float m_time2Drop;


    float m_time2NextKeyLR;
    [Range(0.02f, 1f)]
    public float m_keyRateLR = 0.15f;
    float m_time2NextKeyD;
    [Range(0.01f, 1f)]
    public float m_keyRateD = 0.01f;
    float m_time2NextKeyR;
    [Range(0.02f, 1f)]
    public float m_keyRateR = 0.25f;

    // Start is called before the first frame update
    void Start()
    {

        m_time2NextKeyD = Time.time + m_time2NextKeyD;
        m_time2NextKeyLR = Time.time + m_time2NextKeyLR;
        m_time2NextKeyR = Time.time + m_time2NextKeyR;

        // m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        // m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        // check existence of game board
        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        // check existence of spawner
        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no game spawner defined!");
        }
        else
        {
            if (!m_activeShape)
            {
                m_activeShape = m_spawner.SpawnShape();
            }
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }

    }

    void PlayerInput()
    {
        // User Input movements
        if (Input.GetKey(KeyCode.RightArrow) && (Time.time > m_time2NextKeyLR) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_activeShape.MoveRight();
            m_time2NextKeyLR = Time.time + m_keyRateLR;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && (Time.time > m_time2NextKeyLR) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_activeShape.MoveLeft();
            m_time2NextKeyLR = Time.time + m_keyRateLR;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) && (Time.time > m_time2NextKeyR))
        {
            m_activeShape.RotateRight();
            m_time2NextKeyR = Time.time + m_keyRateR;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) && (Time.time > m_time2NextKeyR) || (Time.time > m_time2Drop))
        {
            m_time2Drop = Time.time + m_dropInterval;
            m_time2NextKeyD = Time.time + m_keyRateD;

            if (m_activeShape)
            {
                m_activeShape.MoveDown();
                // if the current shape hits the bottem boundaries
                if (!m_gameBoard.IsValidPosition(m_activeShape))
                {
                    //If the current shape is overlimit
                    if (m_gameBoard.IsOverLimit(m_activeShape))
                    {
                        GameOver();
                    }
                    else
                    {
                        LandSpace();
                    }
                }
            }
        }

    }


    //When it's GameOver
    void GameOver()
    {
        m_activeShape.MoveUp();
        m_gameOvered = true;
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }


    void LandSpace()
    {
        m_time2NextKeyD = Time.time;
        m_time2NextKeyLR = Time.time;
        m_time2NextKeyR = Time.time;
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnShape();
        m_gameBoard.ClearAllRows();
    }


    // Update is called once per frame
    void Update()
    {
        // if we don't have a spawner or gameBoard just don't run the gameBoard
        if (!m_gameBoard || !m_spawner || m_gameOvered)
        {
            return;
        }
        PlayerInput();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}