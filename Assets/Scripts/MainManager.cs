using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text NameText;
    public Text ScoreText;
    public Text Score;

    public GameObject GameOverText;
    
    private bool m_Started = false;
    public int m_Points;
    public int m_Highscore;
    public string m_Name;
    
    private bool m_GameOver = false;    

    void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighscore();
    }

    
    // Start is called before the first frame update
    void Start()
    {

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        NameText.text = "Name : " + m_Name;
        ScoreText.text = "Score : " + m_Highscore;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            SaveHighscore();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        Score.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;        
        GameOverText.SetActive(true);        
    }

    [System.Serializable]
    class SaveData {
        public string name;
        public int score;
    }

    public void SaveHighscore() {
        SaveData data = new SaveData();
        data.name = m_Name;
        data.score = m_Points;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighscore() {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            m_Name = data.name;
            m_Highscore = data.score;
        }
    }

}
