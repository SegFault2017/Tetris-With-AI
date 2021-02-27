using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{

    public Transform m_holderXform;
    public Shape m_heldShape = null;
    float m_scale = 0.6f;
    public bool m_canRelease = false;

    public void Catch(Shape shape)
    {
        if (m_heldShape || !shape)
        {
            return;
        }

        if (m_holderXform)
        {
            shape.transform.position = m_holderXform.position + shape.m_queueOffset;
            shape.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            m_heldShape = shape;
            shape.transform.rotation = Quaternion.identity;
        }
    }

    public Shape Release()
    {
        m_heldShape.transform.localScale = Vector3.one;
        Shape shape = m_heldShape;
        m_heldShape = null;
        m_canRelease = false;
        return shape;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
