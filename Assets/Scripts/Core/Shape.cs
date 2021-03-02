using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour
{

    // turn this property off if you don't want the shape to rotate (Shape O)
    public bool m_canRotate = true;

    // small offset to shift position while in queue
    public Vector3 m_queueOffset;

    GameObject[] m_glowSquareFx;
    public string glowSquareTag = "LandShapeFx";

    Renderer[] m_shapeRenderers;
    public bool m_IsVisible = true;


    void Start()
    {
        m_shapeRenderers = this.GetComponentsInChildren<Renderer>();

        if (glowSquareTag != "")
        {
            m_glowSquareFx = GameObject.FindGameObjectsWithTag(glowSquareTag);
        }
    }

    void Update()
    {

    }

    public void LandShapeFX()
    {
        int i = 0;

        foreach (Transform child in gameObject.transform)
        {
            if (m_glowSquareFx[i])
            {
                m_glowSquareFx[i].transform.position = new Vector3(child.position.x, child.position.y, -2f);
                ParticlePlayer particlePlayer = m_glowSquareFx[i].GetComponent<ParticlePlayer>();

                if (particlePlayer)
                {
                    particlePlayer.Play();
                }
            }

            i++;
        }

    }

    // general move method
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    //public methods for moving left, right, up and down, respectively
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }


    //public methods for rotating right and left
    public void RotateRight()
    {
        if (m_canRotate)
            transform.Rotate(0, 0, -90);
    }
    public void RotateLeft()
    {
        if (m_canRotate)
            transform.Rotate(0, 0, 90);
    }

    public void RotateClockwise(bool clockwise)
    {
        if (clockwise)
        {
            RotateRight();
        }
        else
        {
            RotateLeft();
        }
    }

    public void ToggleVisibility()
    {
        m_IsVisible = !m_IsVisible;
        if (!m_IsVisible)
        {
            foreach (Renderer rend in m_shapeRenderers)
            {
                rend.enabled = false;
            }
        }
        else
        {
            foreach (Renderer rend in m_shapeRenderers)
            {
                rend.enabled = true;
            }
        }
    }

    public Vector2[] GetPos()
    {
        Vector2[] shapePos = new Vector2[4];
        int i = 0;
        foreach (Transform child in transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            shapePos[i] = pos;
            i++;
        }

        return shapePos;
    }

    public void Hide()
    {
        foreach (Renderer rend in m_shapeRenderers)
        {
            rend.enabled = false;
        }
    }


    public void ClonedPos(Vector2[] pos)
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.position = pos[i];
        }
    }
}
