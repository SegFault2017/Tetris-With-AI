using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allShapes;


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


    // Spawn shape
    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        if (shape)
        {
            return shape;
        }
        Debug.Log("WARNING! Invalid shape in spawner!");
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector2 originalVec = new Vector2(4.3f, 1.3f);
        Vector2 newVec = Vectorf.Round(originalVec);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
