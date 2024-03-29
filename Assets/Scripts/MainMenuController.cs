﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	public Slider loadingBar;
	public GameObject startMenu, levelSelectMenu;

    public void StartGame()
	{
		startMenu.SetActive(false);
		levelSelectMenu.SetActive(true);
	}

	public void ReturnToStartMenu()
	{
		levelSelectMenu.SetActive(false);
		startMenu.SetActive(true);
	}

	public void SelectLevel(string levelName)
	{
		loadingBar.gameObject.SetActive(true);
		levelSelectMenu.SetActive(false);
        StartCoroutine(StartLoadLevel(levelName));
	}

	public void ExitGame ()
	{
        Application.Quit();
	}

	IEnumerator StartLoadLevel(string scene)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			loadingBar.value = progress;
			
			yield return null;
		}
	}
}
