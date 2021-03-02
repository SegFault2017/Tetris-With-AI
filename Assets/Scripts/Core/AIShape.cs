using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Elite
{
    public int m_rotations;
    public Vector2[] m_tranlations;

    public Elite()
    {
        m_rotations = 0;
        m_tranlations = new Vector2[4];
    }
}


public class AIShape : MonoBehaviour
{

    //Attributes
    public float m_heighWeight;
    public float m_linesWeight;
    public float m_holesWeight;
    public float m_bumpinessWeight;


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

    public Elite BestMoves(Board gameBoard, Shape shape)
    {
        Elite bestMoves = new Elite();
        if (!m_AIShape)
        {
            m_AIShape = Instantiate(shape, shape.transform.position, shape.transform.rotation) as Shape;
            m_AIShape.gameObject.name = "AIShape";
        }
        float bestScore = -Mathf.Infinity;
        m_hitBottom = false;

        Vector2[] originalPos = m_AIShape.GetPos();
        int num_rotations = 0;

        for (int rotation = 0; rotation < 4; rotation++)
        {
            num_rotations++;
            m_AIShape.RotateRight();
            while (gameBoard.IsValidPosition(m_AIShape))
            {
                m_AIShape.MoveLeft();
            }

            m_AIShape.MoveRight();

            while (gameBoard.IsValidPosition(m_AIShape))
            {
                Vector2[] posBeforeDrop = m_AIShape.GetPos();
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

                Vector2[] mutatedPos = m_AIShape.GetPos();
                float score = 0f;
                gameBoard.StoreShapeInGrid(m_AIShape);
                float aH = gameBoard.AggreateHeight();
                float lines = gameBoard.GetLines();
                float holes = gameBoard.GetHoles();
                float bumpiness = gameBoard.GetBumpiness();
                score = -m_heighWeight * aH + m_linesWeight * lines - m_holesWeight * holes
                - m_bumpinessWeight * bumpiness;

                // Debug.Log("Score" + score.ToString());
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMoves.m_rotations = rotation;
                    bestMoves.m_tranlations = posBeforeDrop;
                }
                for (int i = 0; i < mutatedPos.Length; i++)
                {
                    gameBoard.DestroyCell((int)mutatedPos[i].x, (int)mutatedPos[i].y);
                }
                m_AIShape.MoveRight();
            }
            Reset();
        }


        return bestMoves;

    }

    public void Reset()
    {
        if (m_AIShape)
        {
            Destroy(m_AIShape.gameObject);
        }
    }

}
