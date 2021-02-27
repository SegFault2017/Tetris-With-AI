using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    int m_score = 0;
    int m_lines;
    public int m_lvl = 1;
    int m_minLines = 1;
    int m_maxLines = 4;

    public Text m_lineText;
    public Text m_scoreText;
    public Text m_lvlText;
    public bool m_didLvlUp = false;
    public int m_linesPerLvl = 5;

    //Level up FX
    public ParticlePlayer m_lvlFX;


    public void ScoreLines(int n)
    {
        m_didLvlUp = false;
        n = Mathf.Clamp(n, m_minLines, m_maxLines);
        switch (n)
        {
            case 1:
                m_score += 40 * n;
                break;
            case 2:
                m_score += 100 * n;
                break;
            case 3:
                m_score += 300 * n;
                break;
            case 4:
                m_score += 1200 * n;
                break;
        }
        m_lines -= n;
        if (m_lines <= 0)
        {
            LevelUp();
        }
        UpdateUIText();
    }

    public void Reset()
    {
        m_lvl = 1;
        m_lines = m_linesPerLvl * m_lvl;
    }
    string PadZeros(int n, int padDigits)
    {
        string nString = n.ToString();
        while (nString.Length < padDigits)
        {
            nString = "0" + nString;
        }
        return nString;
    }
    void UpdateUIText()
    {
        if (m_lineText)
        {
            m_lineText.text = m_lines.ToString();
        }
        if (m_scoreText)
        {
            m_scoreText.text = PadZeros(m_score, 5);
        }
        if (m_lvlText)
        {
            m_lvlText.text = m_lvl.ToString();
        }
    }

    void LevelUp()
    {
        m_lvl++;
        m_lines += m_linesPerLvl * m_lvl;
        m_didLvlUp = true;
        if (m_lvlFX)
        {
            m_lvlFX.Play();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Reset();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
