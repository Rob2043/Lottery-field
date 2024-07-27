using System.Collections;
using CustomEventBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadActivait : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private GameObject mainmenu;
    [SerializeField] private Slider LoadingSlider;
    private float elapsedTime = 0f;

    private void Awake()
    {
        if (mainmenu == null)
        {
            LoadLevel("Menu");
        }
    }

    private void OnEnable() => EventBus.LodingScene += LoadLevel;

    private void OnDisable() => EventBus.LodingScene -= LoadLevel;

    public void LoadLevel(string levelToLoad)
    {
        if (mainmenu != null)
            mainmenu.SetActive(false);
        LoadingScreen.SetActive(true);

        StartCoroutine(LoadSceneAsync(levelToLoad));
    }

    IEnumerator LoadSceneAsync(string levelToLoad)
    {
        float randomDelay = Random.Range(2f, 3f);
        while (elapsedTime < randomDelay)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / randomDelay);
            LoadingSlider.value = progress;
            yield return null;
        }
        SceneManager.LoadSceneAsync(levelToLoad);
    }
}
