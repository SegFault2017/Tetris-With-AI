using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    Board m_gameBoard;
    Spawner m_spawner;

    float m_dropInterval = 1f;
    float m_time2Drop;

    Shape m_activeShape; // keep track of the current falling Shape

    // Start is called before the first frame update
    void Start()
    {
        // m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        // m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        if (m_spawner)
        {
            if (!m_activeShape)
            {
                m_activeShape = m_spawner.SpawnShape();
            }
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }


        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no game spawner defined!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // if we don't have a spawner or gameBoard just don't run the gameBoard
        if (!m_gameBoard || !m_spawner)
        {
            return;
        }

        if (Time.time > m_time2Drop)
        {
            m_time2Drop = Time.time + m_dropInterval;
            if (m_activeShape)
            {
                m_activeShape.MoveDown();
                // if the current hape hits the bottem boundaries
                if (!m_gameBoard.IsValidPosition(m_activeShape))
                {
                    m_activeShape.MoveUp();
                    m_gameBoard.StoreShapeInGrid(m_activeShape);
                    m_activeShape = m_spawner.SpawnShape();
                }
            }
        }

    }
}
