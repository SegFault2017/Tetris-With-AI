using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIShape : MonoBehaviour
{

    //Attributes
    public float m_heighWeight;
    public float m_linesWeight;
    public float m_holesWeight;
    public float m_bumpinessWeight;

    Dictionary<string, int> m_bestMoves;

    Shape m_AIShape = null;
    bool m_hitBottom = false;
    public Color m_color = new Color(1f, 1f, 1f, 0.2f);

    public void DrawAI(Shape originalShape, Board gameBoard)
    {
        if (!m_AIShape)
        {
            m_AIShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
            m_AIShape.gameObject.name = "AIShape";

            SpriteRenderer[] allRenderers = m_AIShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer r in allRenderers)
            {
                r.color = m_color;
            }
        }
        else
        {
            SpriteRenderer[] allRenderers = m_AIShape.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer r in allRenderers)
            {
                r.color = m_color;
            }
            m_AIShape.transform.position = originalShape.transform.position;
            m_AIShape.transform.rotation = originalShape.transform.rotation;
            m_AIShape.transform.localScale = Vector3.one;

        }

        m_hitBottom = false;

        while (!m_hitBottom)
        {
            m_AIShape.MoveDown();
            if (!gameBoard.IsValidPosition(m_AIShape))
            {
                m_AIShape.MoveUp();
                m_hitBottom = true;
            }
        }

    }

    public Dictionary<string, int> BestMoves(Board gameBoard, Shape shape)
    {
        if (!m_AIShape)
        {
            m_AIShape = Instantiate(shape, shape.transform.position, shape.transform.rotation) as Shape;
            m_AIShape.gameObject.name = "AIShape";
        }
        float bestScore = 0f;
        m_bestMoves = new Dictionary<string, int>();
        m_hitBottom = false;

        Vector2[] originalPos = m_AIShape.GetPos();
        int num_rotations = 0;
        int num_translation = 0;
        m_bestMoves["rotations"] = num_rotations;
        m_bestMoves["translations"] = num_translation;

        // foreach (Vector2 pos in originalPos)
        // {
        //     Debug.Log("x:" + pos.x.ToString() + " y:" + pos.y.ToString());
        // }

        for (int rotation = 0; rotation < 4; rotation++)
        {
            num_rotations++;
            num_translation = 0;
            m_AIShape.RotateRight();
            while (gameBoard.IsValidPosition(m_AIShape))
            {
                m_AIShape.MoveLeft();
            }



            while (gameBoard.IsValidPosition(m_AIShape))
            {
                m_hitBottom = false;
                while (!m_hitBottom)
                {
                    m_AIShape.MoveDown();
                    if (!gameBoard.IsValidPosition(m_AIShape))
                    {
                        m_AIShape.MoveUp();
                        m_hitBottom = true;
                    }
                }
                m_AIShape.MoveDown();
                Vector2[] mutatedPos = m_AIShape.GetPos();
                float score = 0f;
                gameBoard.StoreShapeInGrid(m_AIShape);
                score = -m_heighWeight * gameBoard.AggreateHeight()
                                    + m_linesWeight * gameBoard.GetLines()
                                    - m_holesWeight * gameBoard.GetHoles()
                                    - m_bumpinessWeight * gameBoard.GetBumpiness();

                Debug.Log("Score" + score.ToString());
                if (score > bestScore)
                {
                    bestScore = score;
                    m_bestMoves.Remove("rotations");
                    m_bestMoves.Remove("translations");
                    m_bestMoves["rotations"] = num_rotations;
                    m_bestMoves["translations"] = num_translation;
                }
                for (int i = 0; i < mutatedPos.Length; i++)
                {
                    gameBoard.DestroyCell((int)mutatedPos[i].x, (int)mutatedPos[i].y);
                }
                m_AIShape.MoveRight();
                num_translation++;
            }
            Reset();
        }
        return m_bestMoves;

    }

    public void Reset()
    {
        if (m_AIShape)
        {
            Destroy(m_AIShape.gameObject);
        }
    }

}
