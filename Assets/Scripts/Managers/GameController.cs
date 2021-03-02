using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    // --------------------------------- Managers
    // reference to our board
    Board m_gameBoard;

    // reference to our spawner 
    Spawner m_spawner;

    // reference to our soundManager
    SoundManager m_soundManager;

    // reference to our scoreManager
    ScoreManager m_scoreManager;


    // ------------------------------- Properties
    // currently active shape
    Shape m_activeShape;

    // ghost for visualization
    Ghost m_ghost;

    Holder m_holder;

    AIShape m_AIShape;


    //-------------------------------- Drop Time Control
    // starting drop interval value
    public float m_dropInterval = 0.05f;

    // drop interval modified by level
    float m_dropIntervalModded;


    // the next time that the active shape can drop
    float m_timeToDrop;

    // the next time that the active shape can move left or right
    float m_timeToNextKeyLeftRight;

    // the time window we can move left and right
    [Range(0.02f, 1f)]
    public float m_keyRepeatRateLeftRight = 0.25f;

    // the next time that the active shape can move down
    float m_timeToNextKeyDown;

    // the time window we can move down
    [Range(0.01f, 0.5f)]
    public float m_keyRepeatRateDown = 0.01f;

    // the time window we can rotate 
    float m_timeToNextKeyRotate;

    // the time window we can rotate the shape
    [Range(0.02f, 1f)]
    public float m_keyRepeatRateRotate = 0.25f;


    // the panel that displays when our game is over
    public GameObject m_gameOverPanel;

    // whether we have reached the game over condition
    bool m_gameOver = false;

    // toggles the rotation direction icon
    public IconToggle m_rotIconToggle;

    public IconToggle m_modeIconToggle;


    // whether we are rotating clockwise or not when we click the up arrow
    bool m_clockwise = true;

    // whether we are paused
    public bool m_isPaused = false;

    // the panel that display when we Pause
    public GameObject m_pausePanel;

    public ParticlePlayer m_gameOverFx;

    //! -------------------------------------------------- AI Part 

    bool m_humanMode = true;
    float m_AIDropInterval = 0.1f;




    // Use this for initialization
    void Start()
    {

        // find spawner and board with GameObject.FindWithTag plus GetComponent; make sure you tag your objects correctly
        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        // find spawner and board with generic version of GameObject.FindObjectOfType, slower but less typing
        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        m_ghost = GameObject.FindObjectOfType<Ghost>();
        m_holder = GameObject.FindObjectOfType<Holder>();
        m_AIShape = GameObject.FindObjectOfType<AIShape>();



        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING!  There is no game board defined!");
        }

        if (!m_soundManager)
        {
            Debug.LogWarning("WARNING!  There is no sound manager defined!");
        }

        if (!m_scoreManager)
        {
            Debug.LogWarning("WARNING!  There is no score manager defined!");
        }

        if (!m_spawner)
        {
            Debug.LogWarning("WARNING!  There is no spawner defined!");
        }
        else
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);

            if (!m_activeShape)
            {
                m_activeShape = m_spawner.SpawnShape();
            }


        }

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }

        m_dropIntervalModded = Mathf.Clamp(m_dropInterval - ((float)m_scoreManager.m_level * 0.1f), 0.05f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // if we are missing a spawner or game board or active shape, then we don't do anything
        if (!m_spawner || !m_gameBoard || !m_activeShape || m_gameOver || !m_soundManager || !m_scoreManager)
        {
            return;
        }

        if (m_humanMode)
        {
            PlayerInput();

        }
        else
        {
            AIInput();
        }
    }

    void LateUpdate()
    {
        if (m_ghost && m_humanMode & m_AIShape)
        {
            m_ghost.DrawGhost(m_activeShape, m_gameBoard);
        }

        if (m_AIShape && !m_humanMode)
        {
            // m_AIShape.DrawAI(m_activeShape, m_gameBoard);
        }
    }

    void PlayerInput()
    {
        // example of NOT using the Input Manager
        //if (Input.GetKey ("right") && (Time.time > m_timeToNextKey) || Input.GetKeyDown (KeyCode.RightArrow)) 

        if ((Input.GetKey(KeyCode.RightArrow) && (Time.time > m_timeToNextKeyLeftRight)) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);
            }

        }
        else if ((Input.GetKey(KeyCode.LeftArrow) && (Time.time > m_timeToNextKeyLeftRight)) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);

            }

        }
        else if ((Input.GetKey(KeyCode.UpArrow) && (Time.time > m_timeToNextKeyRotate)))
        {
            //m_activeShape.RotateRight();
            m_activeShape.RotateClockwise(m_clockwise);

            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                //m_activeShape.RotateLeft();
                m_activeShape.RotateClockwise(!m_clockwise);

                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);

            }

        }

        else if ((Input.GetKey(KeyCode.DownArrow) && (Time.time > m_timeToNextKeyDown)) || (Time.time > m_timeToDrop))
        {
            m_timeToDrop = Time.time + m_dropIntervalModded;

            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

            m_activeShape.MoveDown();

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                if (m_gameBoard.IsOverLimit(m_activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRotDirection();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Hold();
        }

    }


    void AIInput()
    {
        Dictionary<string, int> bestMoves = m_AIShape.BestMoves(m_gameBoard, m_activeShape);
        Debug.Log("rotations: " + bestMoves["rotations"].ToString() + " , translation" + bestMoves["translations"]);

        if (Time.time > m_timeToDrop)
        {
            m_timeToDrop = Time.time + m_AIDropInterval;

            m_activeShape.MoveDown();

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                if (m_gameBoard.IsOverLimit(m_activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }

        }
    }


    // shape lands
    void LandShape()
    {
        if (m_activeShape)
        {
            // move the shape up, store it in the Board's grid array
            m_activeShape.MoveUp();
            m_gameBoard.StoreShapeInGrid(m_activeShape);

            m_activeShape.LandShapeFX();

            if (m_ghost)
            {
                m_ghost.Reset();
            }

            if (m_AIShape)
            {
                m_AIShape.Reset();
            }

            if (m_holder)
            {
                m_holder.m_canRelease = true;
            }
            // spawn a new shape
            m_activeShape = m_spawner.SpawnShape();

            // set all of the timeToNextKey variables to current time, so no input delay for the next spawned shape
            m_timeToNextKeyLeftRight = Time.time;
            m_timeToNextKeyDown = Time.time;
            m_timeToNextKeyRotate = Time.time;

            // remove completed rows from the board if we have any 
            m_gameBoard.StartCoroutine("ClearAllRows");


            PlaySound(m_soundManager.m_dropSound);

            if (m_gameBoard.m_completedRows > 0)
            {
                m_scoreManager.ScoreLines(m_gameBoard.m_completedRows);

                if (m_scoreManager.didLevelUp)
                {
                    m_dropIntervalModded = Mathf.Clamp(m_dropInterval - ((float)m_scoreManager.m_level * 0.05f), 0.05f, 1f);
                    PlaySound(m_soundManager.m_levelUpVocalClip);
                }
                else
                {
                    if (m_gameBoard.m_completedRows > 1)
                    {
                        AudioClip randomVocal = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                        PlaySound(randomVocal);
                    }
                }

                PlaySound(m_soundManager.m_clearRowSound);
            }
        }

    }

    // triggered when we are over the board's limit
    void GameOver()
    {
        // move the shape one row up
        m_activeShape.MoveUp();

        StartCoroutine(GameOverRoutine());

        // play the failure sound effect
        PlaySound(m_soundManager.m_gameOverSound, 5f);

        // play "game over" vocal
        PlaySound(m_soundManager.m_gameOverVocalClip, 5f);

        // set the game over condition to true
        m_gameOver = true;
    }

    IEnumerator GameOverRoutine()
    {
        if (m_gameOverFx)
        {
            m_gameOverFx.Play();
        }

        yield return new WaitForSeconds(0.3f);

        // turn on the Game Over Panel
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }
    // reload the level
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // plays a sound with an option volume multiplier
    void PlaySound(AudioClip clip, float volMultiplier = 1.0f)
    {
        if (m_soundManager.m_fxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;
        if (m_rotIconToggle)
        {
            m_rotIconToggle.ToggleIcon(m_clockwise);
        }
    }

    public void TogglePause()
    {
        m_isPaused = !m_isPaused;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);

            if (m_soundManager)
            {
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0.25f : m_soundManager.m_musicVolume;
            }

            Time.timeScale = (m_isPaused) ? 0 : 1;
        }
    }

    public void ToggleMode()
    {
        m_humanMode = !m_humanMode;
        if (m_modeIconToggle)
        {
            m_modeIconToggle.ToggleIcon(m_humanMode);
        }
    }

    public void Hold()
    {

        // if the holder is empty...
        if (!m_holder.m_heldShape)
        {
            // catch the current active Shape
            m_holder.Catch(m_activeShape);

            // spawn a new Shape
            m_activeShape = m_spawner.SpawnShape();

            // play a sound
            PlaySound(m_soundManager.m_holdSound);

        }
        // if the holder is not empty and can release…
        else if (m_holder.m_canRelease)
        {
            // set our active Shape to a temporary Shape
            Shape shape = m_activeShape;

            // release the currently heldShape 
            m_activeShape = m_holder.Release();

            // move the released Shape back to the spawner position
            m_activeShape.transform.position = m_spawner.transform.position;

            // catch the temporary Shape
            m_holder.Catch(shape);

            // play a sound 
            PlaySound(m_soundManager.m_holdSound);

        }
        // the holder is not empty but cannot release yet
        else
        {
            //Debug.LogWarning("HOLDER WARNING:  Wait for cool down");

            // play an error sound
            PlaySound(m_soundManager.m_errorSound);

        }

        // reset the Ghost every time we tap the Hold button
        if (m_ghost)
        {
            m_ghost.Reset();
        }

    }


}
