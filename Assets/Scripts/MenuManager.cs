using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuManager : MonoBehaviour
{

    public Text nameText;
    public Text scoreText;
    
    void Start() {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            scoreText.text = data.score.ToString();
            nameText.text = data.name;
        }
    }

    public void StartNew() {
        SceneManager.LoadScene(1);
    }

    public void Exit() {
    
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void SetInputName(string name) {
        SaveData data = new SaveData();
        data.name = name;
        data.score = 0;       
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);        
    }

    [System.Serializable]
    class SaveData {
        public string name;
        public int score;
    }

}