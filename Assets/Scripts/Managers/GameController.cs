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

    //Sound manager 
    SoundManager m_soundManager;

    //Score Manager
    ScoreManager m_scoreManager;

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

    //Icon
    public IconToggle m_rotIconToggle;
    bool m_clockwise = true;

    //Pause Menu
    bool m_isPaused = false;
    public GameObject m_pausePanel;

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;
        if (!m_rotIconToggle)
        {
            return;
        }
        m_rotIconToggle.ToggleIcon(m_clockwise);
    }

    public void TogglePause()
    {
        if (!m_pausePanel || m_gameOvered)
        {
            return;
        }
        m_isPaused = !m_isPaused;
        m_pausePanel.SetActive(m_isPaused);
        if (m_soundManager)
        {
            m_soundManager.m_musicVolume = (m_isPaused) ?
            m_soundManager.m_musicVolume * 0.25f : m_soundManager.m_musicVolume / 0.25f;
        }
        Time.timeScale = (m_isPaused) ? 0 : 1;



    }

    void PlayFXSound(AudioClip clip, float volMultiplier)
    {
        if (m_soundManager.m_fxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position,
            Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));

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
                PlayFXSound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlayFXSound(m_soundManager.m_moveSound, 0.5f);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && (Time.time > m_time2NextKeyLR) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_activeShape.MoveLeft();
            m_time2NextKeyLR = Time.time + m_keyRateLR;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                PlayFXSound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlayFXSound(m_soundManager.m_moveSound, 0.5f);
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) && (Time.time > m_time2NextKeyR))
        {
            m_activeShape.RotateClockwise(m_clockwise);
            m_time2NextKeyR = Time.time + m_keyRateR;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateClockwise(!m_clockwise);
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
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }


    //When it's GameOver

    void LandSpace()
    {
        m_time2NextKeyD = Time.time;
        m_time2NextKeyLR = Time.time;
        m_time2NextKeyR = Time.time;
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        PlayFXSound(m_soundManager.m_dropSound, 0.75f);
        m_activeShape = m_spawner.SpawnShape();
        m_gameBoard.ClearAllRows();
        if (m_gameBoard.m_completedRows > 0)
        {
            m_scoreManager.ScoreLines(m_gameBoard.m_completedRows);
            if (m_scoreManager.m_didLvlUp)
            {
                PlayFXSound(m_soundManager.m_lvlUpVocalClip, 1f);
            }
            else
            {
                int n = m_soundManager.m_vocalClips.Length;
                if (m_gameBoard.m_completedRows > 3)
                {
                    PlayFXSound(m_soundManager.m_vocalClips[n - 1], 0.5f);
                }
                else if (m_gameBoard.m_completedRows > 2)
                {
                    PlayFXSound(m_soundManager.m_vocalClips[n - 2], 0.5f);
                }
                else if (m_gameBoard.m_completedRows > 1)
                {
                    PlayFXSound(m_soundManager.m_vocalClips[n - 3], 0.5f);
                }
                else
                {
                    PlayFXSound(m_soundManager.m_vocalClips[n - 4], 0.5f);
                }
            }

            PlayFXSound(m_soundManager.m_clearRowSound, 0.5f);

        }
    }


    // Update is called once per frame
    void Update()
    {
        // if we don't have a spawner or gameBoard just don't run the gameBoard
        if (!m_gameBoard || !m_spawner || m_gameOvered || !m_soundManager || !m_scoreManager)
        {
            return;
        }
        PlayerInput();
    }

    void GameOver()
    {
        m_activeShape.MoveUp();
        m_gameOvered = true;
        PlayFXSound(m_soundManager.m_gameOverSound, 5f);
        PlayFXSound(m_soundManager.m_gameOverVocalClip, 5f);
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();


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

        if (!m_soundManager)
        {
            Debug.LogWarning("WARNING! There is no sound manager defined!");
        }

        if (!m_scoreManager)
        {
            Debug.LogWarning("WARNING! There is no score manager defined!");
        }

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }

    }
}