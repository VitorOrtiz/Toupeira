using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScoreControl : MonoBehaviour {
    private int lastposition;
    private Transform playerposition;
    public static int Score;
    private float Gametime;
    public Text scoreText;

    public CanvasGroup fadeImage;

    public GameObject endGameScreen;
    Animator endgameanim;
    bool Resetalready = false;
	// Use this for initialization
	void Start () {
        endgameanim = endGameScreen.GetComponent<Animator>();
        playerposition = Player.player.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Gametime += Time.deltaTime;
        Score += (int)playerposition.position.x - lastposition;
        lastposition = (int)playerposition.position.x;
        scoreText.text = "" +Score;

        if (Player.player.Dead && !Resetalready)
        {
            Resetalready = true;
            StartCoroutine(EndGame());
        }
        else
        {
            if (Score > 300 && Score <= 500)
                TrackMaker.maker.Difficulty = 2;
            else if (Score > 500)
            {
                TrackMaker.maker.Difficulty = 3;

            }
        }
        }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(.5f);
        endgameanim.SetBool("Rest", true);
        
        
        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        endgameanim.SetBool("Rest", false);
        StartCoroutine(Fade(1));
        yield return new WaitForSeconds(1);
        TrackMaker.maker.Difficulty = 1;
        TrackMaker.maker.ResetAll();
        Player.player.ResetAll();
        CameraControl.control.ResetAll();
        StartCoroutine(Fade(0));
        yield return new WaitForSeconds(1);
        Player.player.Activate();
        Resetalready = false;
    }
    IEnumerator Fade(int In)
    {
        float delta;
        float start = 0;
        float end = 1;
        if (In == 0){
            start = 1;
            end = 0;
        }
        delta = start;
        float time = 0;
        while (delta !=end)
        {
            time += Time.deltaTime;
            delta = Mathf.Lerp(start, end, time);
            fadeImage.alpha = delta;
            yield return null;
        }
        fadeImage.alpha = end;
    }
}
