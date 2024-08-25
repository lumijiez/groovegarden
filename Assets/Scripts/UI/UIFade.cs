using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private RawImage videoScreen;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip shrekVideoClip;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private GameObject UIContainer;

    private IEnumerator fadeRoutine;

    private void Start()
    {
        videoPlayer.Prepare();
    }

    public void FadeDark(string transition)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        switch (transition)
        {
            case "MainToShrek":
                BackgroundMusic.Instance.StopAll();
                BackgroundMusic.Instance.Play("gangnam");
                FadeToVideo(shrekVideoClip);
                break;
            default:
                BackgroundMusic.Instance.StopAll();
                FadeToBlack();
                break;
        }
    }

    public void FadeToVideo(VideoClip videoClip)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        UIContainer.SetActive(false);

        StartCoroutine(PlayVideo(videoClip));
    }

    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        UIContainer.SetActive(false);

        fadeScreen.sprite = null;
        fadeScreen.color = new Color(0, 0, 0, 0);

        fadeRoutine = FadeRoutine(1, fadeScreen);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        UIContainer.SetActive(false);

        if (videoPlayer.isPlaying)
        {
            fadeRoutine = FadeOutAndStopVideo();
        }
        else
        {
            fadeRoutine = FadeRoutine(0, fadeScreen);
        }
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator PlayVideo(VideoClip videoClip)
    {
        videoPlayer.clip = videoClip;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoScreen.texture = videoPlayer.targetTexture;
        videoScreen.color = new Color(videoScreen.color.r, videoScreen.color.g, videoScreen.color.b, 0);

        videoPlayer.Play();

        fadeRoutine = FadeRoutine(1, videoScreen);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeOutAndStopVideo()
    {
        yield return FadeRoutine(0, videoScreen);

        videoPlayer.Stop();
        videoScreen.texture = null;
        UIContainer.SetActive(true);
    }

    private IEnumerator FadeRoutine(float targetAlpha, Graphic target)
    {
        while (!Mathf.Approximately(target.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(target.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
            yield return null;
        }
        if (targetAlpha < 0.1)
        {
            UIContainer.SetActive(true);
        }
    }
}
