using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackPlayer : MonoBehaviour
{
    int frameNumber = 0;
    int listIndex = 0;
    int everyNthFrame;
    [SerializeField] bool playback;
    List<Vector3> ghostPositions = new List<Vector3>();
    List<Quaternion> ghostRotations = new List<Quaternion>();
    string levelName;
    Transform ghostTf;
    PlaybackManager playbackManager;
    Timer timer;
    private void Awake()
    {
        playbackManager = PlaybackManager.instance;
        ghostTf = GameObject.Find("Ghost").transform;
        if (playback) { playbackManager.LoadRecording(); }
        if (!playbackManager.IsPlaying()) { ghostTf.gameObject.SetActive(false); }
        ghostPositions = playbackManager.GetGhostPositions();
        ghostRotations = playbackManager.GetGhostRotations();
        everyNthFrame = playbackManager.everyNthFrame;
        timer = FindObjectOfType<Timer>();
        if (timer == null) { Debug.LogError("Timer was NULL!"); }
        
    }
    private void FixedUpdate()
    {
        if (playback) 
        { 
            UpdateGhost();
            frameNumber++;
        }
        
    }
    void UpdateGhost()
    {
        if (listIndex < ghostPositions.Count)
        {
            if (frameNumber % everyNthFrame == 0)
            {
                ghostTf.position = ghostPositions[listIndex];
                ghostTf.rotation = ghostRotations[listIndex];
            }
            else
            {
                if (listIndex < ghostPositions.Count - 1)
                {
                    float t = ((float)frameNumber - (float)listIndex * (float)everyNthFrame) / (float)everyNthFrame;
                    ghostTf.position = Vector3.Lerp(ghostPositions[listIndex], ghostPositions[listIndex + 1], t);
                    ghostTf.rotation = Quaternion.Lerp(ghostRotations[listIndex], ghostRotations[listIndex + 1], t);
                    if (frameNumber % everyNthFrame == everyNthFrame - 1) { listIndex++; }
                }
            }
        }
        else
        {
            playback = false;
            ghostTf.gameObject.SetActive(false);
        }
        
    }
    public void StopPlayback()
    {
        playback = false;
    }
    public void StartPlayback()
    {
        playback = true;
    }

}
