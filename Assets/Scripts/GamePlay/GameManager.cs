using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<int> scoreList;

    private int score;

    private string dataPath;

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/leader.board.json";
        scoreList=GetScoreListData();

        if(instance == null)
             instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
        EventHandler.GetPointEvent += OnGetPointEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }

    private void OnGetPointEvent(int point)
    {
        score = point;
    }

    private void OnGameOverEvent()
    {
        //TODO:在list里添加新的分数，排序
        if (!scoreList.Contains(score))
        {
            scoreList.Add(score);
        }

        //foreach(var item in scoreList)
        //{
        //    Debug.Log(item.ToString());
        //}

        scoreList.Sort();
        scoreList.Reverse();

        File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));
    }

    public List<int> GetScoreListData()
    {
        if (File.Exists(dataPath))
        {
            string jsonData=File.ReadAllText(dataPath);
            return JsonConvert.DeserializeObject<List<int>>(jsonData);
        }
        return new List<int>();
    }
}
