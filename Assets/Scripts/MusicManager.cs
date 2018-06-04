using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {


    public enum Note
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6
    }

    public AudioClip[] noteClips;
    private AudioSource[] notes;
    private List<int> playingNotes;

    public delegate void OnNotePlayed(int note);
    public OnNotePlayed onNotePlayedEvent;

    public delegate void OnNoteStopped(int note);
    public OnNoteStopped onNoteStoppedEvent;

    // Use this for initialization
    void Start () {
        notes = new AudioSource[noteClips.Length];
        for (int i = 0; i < noteClips.Length; ++i)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = noteClips[i];
            notes[i] = audioSource;
        }
        playingNotes = new List<int>();
        GoalManager goalManager = FindObjectOfType<GoalManager>();
        if (goalManager != null)
            goalManager.reachedGoalEvent += OnPlayerReachedGoal;
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (var note in playingNotes.ToArray())
            if (!notes[note].isPlaying)
                StopNote(note);
    }

    public void PlayNote(int index)
    {
        if (index < notes.Length)
        {
            notes[index].Play();
            if (onNotePlayedEvent != null)
                onNotePlayedEvent.Invoke(index);
            playingNotes.Add(index);
        }
    }

    public void StopNote(int index)
    {
        if (index < notes.Length)
        {
            playingNotes.Remove(index);
            StartCoroutine(FadeOut(notes[index], 0.1f));
            if (onNotePlayedEvent != null)
                onNoteStoppedEvent.Invoke(index);
        }
    }

    public void OnPlayerReachedGoal()
    {
        PlayNote(0);
        PlayNote(4);
        PlayNote(6);

        List<int> completedLevels = SaveData.Load();
        completedLevels.Add(SceneManager.GetActiveScene().buildIndex);
        SaveData.Save(completedLevels);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /* Thanks to Boris1998 - https://forum.unity.com/threads/fade-out-audio-source.335031/ */
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
