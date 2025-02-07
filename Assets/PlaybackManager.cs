using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
//using UnityEngine.WSA;

public class PlaybackManager : MonoBehaviour
{

    [SerializeField] private string levelName;
    [SerializeField] private string playerName;
    [SerializeField] public int everyNthFrame = 5;
    [SerializeField] bool playback = true;
    List<Vector3> ghostPositions = new List<Vector3>();
    List<Quaternion> ghostRotations = new List<Quaternion>();
    public static PlaybackManager instance;
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
    }
    public void LoadRecording()
    {
        ghostPositions = new List<Vector3>();
        ghostRotations = new List<Quaternion>();
        string pathName = Path.Combine(Application.persistentDataPath, levelName);
        if (pathName == null)
        {
            playback = false;
            return;
        }
        string fileName = playerName + ".dat";
        string pathAndFile = Path.Combine(pathName, fileName);
        if (File.Exists(pathAndFile))
        {
            string text = File.ReadAllText(pathAndFile);
            if (text.Length > 0)
            {
                string[] ghostEntries = text.Split("\n");
                List<string> tempListPos = new List<string>();
                List<string> tempListRot = new List<string>();
                for (int i = 0;i < ghostEntries.Length - 1;i++)
                {
                    ghostEntries[i].Replace("\n", "");
                    string[] tempEntry = ghostEntries[i].Split("\t");
                    tempListPos.Add(tempEntry[0].Replace("\t", ""));
                    string[] points = tempListPos[i].Split(",");
                    for (int j = 0; j < points.Length; j++ )
                    {
                        points[j] = points[j].Replace("(", "").Replace(",", "").Replace(")", "");
                    }
                    Vector3 tempGhostPos = new Vector3(float.Parse(points[0]), float.Parse(points[1]), float.Parse(points[2]));
                    ghostPositions.Add(tempGhostPos);
                    
                    tempListRot.Add(tempEntry[1].Replace("\t", ""));
                    string[] rots = tempListRot[i].Split(",");
                    for (int j = 0; j < rots.Length; j++)
                    {
                        rots[j] = rots[j].Replace("(", "").Replace(",", "").Replace(")", "");
                    }
                    Quaternion tempGhostRot = new Quaternion(float.Parse(rots[0]), float.Parse(rots[1]), float.Parse(rots[2]), float.Parse(rots[3]));
                    ghostRotations.Add(tempGhostRot);
                }
                playback = true;
            }
            else
            {
                Debug.LogError("File " +  pathAndFile + " found, but length was 0");
                playback = false;
            }
        }
        else
        {
            Debug.LogError("File " + pathAndFile + " does not exist!");
            playback = false;
        }
    }
    public List<Vector3> GetGhostPositions() { return ghostPositions; }
    public List<Quaternion> GetGhostRotations() {  return ghostRotations; }
    public void Initialize(string _levelName, string _playerName)
    {
        levelName = _levelName;
        playerName = _playerName;
        LoadRecording();
    }
    public void Clear()
    {
        playback = true;
        levelName = null;
        playerName = null;
    }
    public bool IsPlaying() { return playback; }
}
