using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public enum SceneType
{
    _titleMovie = 0,
    _title,
    _main,
    _result
}

public class SceneLoadMgr : MonoBehaviour {

    const string TITLE_NAME = "title";
    const string TITLE_MOVIE_NAME = "titlemovie";
    const string MAIN_NAME = "main";
    const string RESULT_NAME = "Result";

    public SceneType type;

    bool isFirst = false;

    float timer;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		switch(SceneManager.GetActiveScene().name)
        {
            case TITLE_MOVIE_NAME:
                TitleMovieScene();
                break;

            case TITLE_NAME:
                TitleScene();
                break;

            case MAIN_NAME:
                break;

            case RESULT_NAME:
                break;
        }
	}

    void TitleMovieScene()
    {
        if (SplashScreen.isFinished)
        {
            if((Input.GetButtonDown("Fire1") && isFirst == false) 
                || IsWaitTimer(25.0f))
            {
                StartCoroutine(Delay());
                type = SceneType._title;
                FadeManager.Instance.LoadScene(TITLE_NAME, 1.5f,false);
            }
        }
    }

    void TitleScene()
    {
        if(Input.GetButtonDown("Fire1") && isFirst == false)
        {
            StartCoroutine(Delay());
            type = SceneType._main;
            FadeManager.Instance.LoadSpriteScene(MAIN_NAME, 1.5f,false);
        }
    }

    void MainScene()
    {

    }

    void ResultScene()
    {

    }

    IEnumerator Delay()
    {
        isFirst = true;
        yield return new WaitForSeconds(1.5f);
        isFirst = false;
    }

   bool IsWaitTimer(float waitTime)
    {
        timer += Time.deltaTime;

        if(timer >= waitTime)
        {
            timer = 0;
            return true;
        }

        return false;
    }
}
