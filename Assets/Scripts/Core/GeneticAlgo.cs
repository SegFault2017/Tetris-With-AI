using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Genome
{
    public int m_movesTaken = 0;
    public int m_moveLimit;

    public float m_id;
    public float m_rowCleared;
    public float m_weightedHeight;
    public float m_cumulativeHeight;
    public float m_relativeHeight;
    public float m_holes;
    public float m_roughness;


    public Genome()
    {
        m_moveLimit = 500;
        m_id = Random.Range(0f, 50f);
        m_rowCleared = Random.Range(0f, 1f) - 0.5f;
        m_weightedHeight = Random.Range(0f, 1f) - 0.5f;
        m_cumulativeHeight = Random.Range(0, 1f) - 0.5f;
        m_roughness = Random.Range(0f, 1f) - 0.5f;
    }


}

//Class archive saves info about list of genomes
class Archive
{
    public int m_populationSize;
    public int m_currentPopulation = 0;

    public float m_mutationRate;
    public float m_mutationStep;

    public List<Genome> m_elites;
    public Genome[] m_genomes;

    public Archive(int populationSize, float mutationRate, float mutationStep)
    {
        m_populationSize = populationSize;
        m_mutationRate = mutationRate;
        m_mutationStep = mutationStep;
        m_elites = new List<Genome>();
        m_genomes = new Genome[m_populationSize];
    }

    public class GeneticAlgo : MonoBehaviour
    {
        // Start is called before the first frame update

        public Genome[] m_genomes;
        int m_currentGenome = 0;
        int m_generation = 0;

        void evolve()
        {
            Debug.Log("Generation" + m_generation + " evaluated.");
            m_currentGenome = 0;
            m_generation++;

        }

        //Evaluates the next genome in the population. If there is None, evolves 
        // in the population.
        void evaluateNextGame()
        {
            if (m_currentGenome == m_genomes.Length)
            {
                //evolve
            }
        }

        void createInitialPopulation()
        {
            Archive archive = new Archive(50, 0.05f, 0.02f);
            m_genomes = new Genome[archive.m_populationSize];
            for (int i = 0; i < archive.m_populationSize; i++)
            {
                m_genomes[i] = new Genome();
            }

        }
        void Start()
        {
            createInitialPopulation();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
