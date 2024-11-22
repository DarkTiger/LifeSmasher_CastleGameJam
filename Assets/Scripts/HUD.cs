using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Image archetypeImage;
    [SerializeField] TextMeshProUGUI archetypeName;
    [SerializeField] TextMeshProUGUI txtExperience;
    [SerializeField] TextMeshProUGUI txtEventText;
    [SerializeField] Image imgEventTextBack;
    [SerializeField] TextMeshProUGUI txtTimeText;
    [SerializeField] TextMeshProUGUI txtWavesText;
    [SerializeField] Slider healthbar;
    [SerializeField] GameObject shop;
    [SerializeField] Image instructionImage;

    public static HUD Instance;


    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(19f);

        instructionImage.gameObject.SetActive(false);
    }

    public void SetArchetype(Sprite sprite, string archetypeName)
    {
        archetypeImage.sprite = sprite;
        this.archetypeName.text = archetypeName;
    }

    public void UpdateExperienceText(int value)
    {
        txtExperience.text = value.ToString();
    }

    public void SetEventText(string text, float duration, float backHeight, bool opaque = false, Color color = default(Color))
    {
        StartCoroutine(SetEventTextCO(text, duration, backHeight, opaque, color));
    }

    IEnumerator SetEventTextCO(string text, float duration, float backHeight, bool opaque, Color color)
    {
        if (color == default(Color))
        {
            color = Color.white;
        }

        txtEventText.text = text;
        txtEventText.color = color;
        imgEventTextBack.rectTransform.sizeDelta = new Vector2(imgEventTextBack.rectTransform.sizeDelta.x, backHeight);
        imgEventTextBack.color = new Color(imgEventTextBack.color.r, imgEventTextBack.color.g, imgEventTextBack.color.b, opaque? 1f : 0.745f);
        imgEventTextBack.enabled = true;

        yield return new WaitForSeconds(duration);

        txtEventText.text = "";
        txtEventText.color = Color.white;
        imgEventTextBack.enabled = false;
    }

    public void SetTimeText(string text)
    {
        txtTimeText.text = text;
    }

    public void SetWaveText(int wave)
    {
        txtWavesText.text = "WAVE: " + wave.ToString();
    }

    public void SetHealth(float health)
    {
        healthbar.value = health;
    }

    public void OpenShop()
    {
        shop.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseShop()
    {
        shop.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
