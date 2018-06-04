using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MusicListener {

    [Range(0.1f, 10f)]
    public float levelSelectDelay = 2f;

    [Tooltip("Buttons for starting levels, MUST be in sequential order, i.e Level1, Level2, etc.")]
    public GameObject[] levelButtons;

    public enum CurrentMenu { Splash = 0, LevelSelect = 1}
    private CurrentMenu currentMenu = CurrentMenu.Splash;

    public GameObject splash;
    public GameObject levelSelect;

    protected override void Start()
    {
        base.Start();
        List<int> completedLevels = SaveData.Load();
        if (completedLevels.Count == 0)
            SaveData.Save(new List<int>());
        // levels are in range 1-3, levelButtons indices are in range 0-2
        levelButtons[0].SetActive(true);
        foreach (int level in completedLevels)
        {
            levelButtons[level-1].SetActive(true);
            var color = levelButtons[level - 1].GetComponent<Image>().color;
            color.a = 0.5f;
            levelButtons[level - 1].GetComponent<Image>().color = color;
            // TODO change this to unlock by row instead of one-by-one
            if (level < levelButtons.Length)
                levelButtons[level].SetActive(true);
        }
    }

    public void SwapMenus()
    {
        if (currentMenu == CurrentMenu.Splash)
        {
            splash.SetActive(false);
            levelSelect.SetActive(true);
        } else
        {
            splash.SetActive(true);
            levelSelect.SetActive(false);
        }
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(LoadSceneAsync(level));
    }

    IEnumerator LoadSceneAsync(int level)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(levelSelectDelay - 0.1f);
        musicManager.StopNote(2);
        musicManager.StopNote(6);
        yield return new WaitForSeconds(0.1f);
        asyncLoad.allowSceneActivation = true;
    }

    public override void OnNotePlayed(int note)
    {
    }

    public override void OnNoteStopped(int note)
    {
    }

    public void ClearSaveData()
    {
        SaveData.Save(new List<int>());
    }
}
