using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusicListener : MonoBehaviour {

    protected MusicManager musicManager;

	protected virtual void Start () {
        musicManager = FindObjectOfType<MusicManager>();
        musicManager.onNotePlayedEvent += OnNotePlayed;
        musicManager.onNoteStoppedEvent += OnNoteStopped;
	}

    public abstract void OnNotePlayed(int note);
    public abstract void OnNoteStopped(int note);
}
