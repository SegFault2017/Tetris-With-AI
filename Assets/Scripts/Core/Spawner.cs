using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;
    public Transform[] m_queueXforms = new Transform[3];

    Shape[] m_queuedShapes = new Shape[3];

    float m_queuedScale = 0.6f;

    public ParticlePlayer m_spawnFx;

    // Get Random Shape
    Shape GetRandomShape()
    {
        int id = Random.Range(0, m_allShapes.Length);
        if (m_allShapes[id])
        {
            return m_allShapes[id];
        }
        Debug.Log("WARNING! Invalid shape!");
        return null;
    }


    IEnumerator GrowShape(Shape shape, Vector3 position, float growTime = 0.5f)
    {
        float size = 0f;
        growTime = Mathf.Clamp(growTime, 0.1f, 2f);
        float sizeDelta = Time.deltaTime / growTime;

        while (size < 1f)
        {
            shape.transform.localScale = new Vector3(size, size, size);
            size += sizeDelta;
            shape.transform.position = position;
            yield return null;
        }

        shape.transform.localScale = Vector3.one;
    }

    // Spawn shape
    public Shape SpawnShape()
    {
        Shape shape = null;
        // shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        shape = GetQueuedShape();
        shape.transform.position = transform.position;
        // shape.transform.localScale = Vector3.one;
        StartCoroutine(GrowShape(shape, transform.position, 0.25f));


        if (m_spawnFx)
        {
            m_spawnFx.Play();
        }

        if (shape)
        {
            return shape;
        }
        Debug.Log("WARNING! Invalid shape in spawner!");
        return null;
    }

    void InitQueue()
    {
        for (int i = 0; i < m_queuedShapes.Length; i++)
        {
            m_queuedShapes[i] = null;
        }
        FillQueue();

    }

    void FillQueue()
    {
        for (int i = 0; i < m_queuedShapes.Length; i++)
        {
            if (!m_queuedShapes[i])
            {
                m_queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
                m_queuedShapes[i].transform.position = m_queueXforms[i].position + m_queuedShapes[i].m_queueOffset;
                // if (m_queuedShapes[i].name == "ShapeO(Clone)")
                // {
                //     Vector3 pos = m_queuedShapes[i].transform.position;
                //     m_queuedShapes[i].transform.position = new Vector3(pos.x - 0.25f, pos.y + 0.5f, pos.z);
                // }
                m_queuedShapes[i].transform.localScale = new Vector3(m_queuedScale, m_queuedScale, m_queuedScale);
            }

        }
    }

    Shape GetQueuedShape()
    {
        Shape firstShape = null;
        if (m_queuedShapes[0])
        {
            firstShape = m_queuedShapes[0];
        }

        for (int i = 1; i < m_queuedShapes.Length; i++)
        {
            m_queuedShapes[i - 1] = m_queuedShapes[i];
            m_queuedShapes[i - 1].transform.position = m_queueXforms[i - 1].position + m_queuedShapes[i - 1].m_queueOffset;
        }

        m_queuedShapes[m_queuedShapes.Length - 1] = null;
        FillQueue();
        return firstShape;
    }

    void Awake()
    {
        InitQueue();

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
