using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject credits;

    bool playing = false;
    bool quitting = false;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (credits.activeSelf)
            {
                buttons.SetActive(true);
                credits.SetActive(false);
            }
        }
    }

    public void OnPlay()
    {
        if (!playing)
        {
            StartCoroutine(PlayCO());
            playing = true;
        }
       
    }

    public void OnCredits()
    {
        buttons.SetActive(false);
        credits.SetActive(true);
    }

    public void OnQuit()
    {
        if (!quitting)
        {
            StartCoroutine(QuitCO());
            quitting = true;
        }
    }

    IEnumerator PlayCO()
    {
        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }

        SceneManager.LoadScene(1);
    }

    IEnumerator QuitCO()
    {
        while (image.color.a < 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }

       Application.Quit();
    }
}
