using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameRecorder : MonoBehaviour
{
    public static GameRecorder instance;
    bool recording = false;
    private string levelName;
    int frameNumber = 0;
    int everyNthFrame;
    Transform playerTf;
    List<Vector3> playerPositions = new List<Vector3>();
    List<Quaternion> playerRotations = new List<Quaternion>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        everyNthFrame = PlaybackManager.instance.everyNthFrame;
    }
    void Start()
    {
        playerTf = FindObjectOfType<Player>().transform.GetChild(0);
        levelName = LevelSelect.Instance.LevelToLoad.name;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (recording)
        {
            Record();
            frameNumber++;
        }
        
    }
    void Record()
    {
        if (frameNumber % everyNthFrame == 0)
        {
            playerPositions.Add(playerTf.position);
            playerRotations.Add(playerTf.rotation);

        }
        frameNumber++;
    }
    public void SaveRecording()
    {
        string pathName = Path.Combine(Application.persistentDataPath, levelName);
        string fileName = Path.Combine(PlayerPrefs.GetString("PlayerName", "Player") + ".dat");
        string pathAndFile = Path.Combine(pathName, fileName);
        if (!Directory.Exists(pathName)) { Directory.CreateDirectory(pathName); }

        if (!File.Exists(pathAndFile))
        {
            FileStream fileToCreate = File.Create(pathAndFile);
            fileToCreate.Close();
        }
        if (!File.Exists(pathAndFile))
        {
            Debug.LogError("File " + pathAndFile + " was NOT created!");
        }
        string stringToWrite = "";
        for (int i = 0; i < playerPositions.Count; i++)
        {
            stringToWrite += playerPositions[i].ToString() + "\t" + playerRotations[i].ToString() + "\n";
        }
        File.WriteAllText(pathAndFile, stringToWrite);
        Destroy(this.gameObject);
    }
    public bool IsRecording() { return recording; }
    public void StopRecording() {  recording = false; }
    public void StartRecording() { recording = true; }
}
