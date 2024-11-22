using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Button rebelLv1Button;
    [SerializeField] Button rebelLv2Button;
    [SerializeField] Button rebelLv3Button;

    [SerializeField] Button magicianLv1Button;
    [SerializeField] Button magicianLv2Button;
    [SerializeField] Button magicianLv3Button;

    [SerializeField] Button innocentLv1Button;
    [SerializeField] Button innocentLv2Button;
    [SerializeField] Button innocentLv3Button;

    [SerializeField] Button explorerLv1Button;
    [SerializeField] Button explorerLv2Button;
    [SerializeField] Button explorerLv3Button;

    int priceLv1 = 500;
    int priceLv2 = 1000;
    int priceLv3 = 2000;


    private void OnEnable()
    {
        UpdateState();
    }

    [ContextMenu("UPDATE STATE")]
    public void UpdateState()
    {
        switch (GameManager.Instance.RebelLevel)
        { 
            case 0: 
                rebelLv1Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv1); break;
            case 1: 
                rebelLv1Button.interactable = false;
                rebelLv1Button.GetComponent<Image>().color = Color.green;
                rebelLv2Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv2); 
                break;
            case 2:
                rebelLv1Button.interactable = false;
                rebelLv2Button.interactable = false;
                rebelLv1Button.GetComponent<Image>().color = Color.green;
                rebelLv2Button.GetComponent<Image>().color = Color.green;
                rebelLv3Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv3); 
                break;
            case 3:
                rebelLv1Button.GetComponent<Image>().color = Color.green;
                rebelLv2Button.GetComponent<Image>().color = Color.green;
                rebelLv3Button.GetComponent<Image>().color = Color.green;
                rebelLv1Button.interactable = false;
                rebelLv2Button.interactable = false;
                rebelLv3Button.interactable = false;
                break;
        }

        switch (GameManager.Instance.MagicianLevel)
        {
            case 0:
                magicianLv1Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv1); break;
            case 1:
                magicianLv1Button.interactable = false;
                magicianLv1Button.GetComponent<Image>().color = Color.green;
                magicianLv2Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv2);
                break;
            case 2:
                magicianLv1Button.interactable = false;
                magicianLv2Button.interactable = false;
                magicianLv1Button.GetComponent<Image>().color = Color.green;
                magicianLv2Button.GetComponent<Image>().color = Color.green;
                magicianLv3Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv3);
                break;
            case 3:
                magicianLv1Button.GetComponent<Image>().color = Color.green;
                magicianLv2Button.GetComponent<Image>().color = Color.green;
                magicianLv3Button.GetComponent<Image>().color = Color.green;
                magicianLv1Button.interactable = false;
                magicianLv2Button.interactable = false;
                magicianLv3Button.interactable = false;
                break;
        }

        switch (GameManager.Instance.InnocentLevel)
        {
            case 0:
                innocentLv1Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv1); break;
            case 1:
                innocentLv1Button.interactable = false;
                innocentLv1Button.GetComponent<Image>().color = Color.green;
                innocentLv2Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv2);
                break;
            case 2:
                innocentLv1Button.interactable = false;
                innocentLv2Button.interactable = false;
                innocentLv1Button.GetComponent<Image>().color = Color.green;
                innocentLv2Button.GetComponent<Image>().color = Color.green;
                innocentLv3Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv3);
                break;
            case 3:
                innocentLv1Button.GetComponent<Image>().color = Color.green;
                innocentLv2Button.GetComponent<Image>().color = Color.green;
                innocentLv3Button.GetComponent<Image>().color = Color.green;
                innocentLv1Button.interactable = false;
                innocentLv2Button.interactable = false;
                innocentLv3Button.interactable = false;
                break;
        }

        switch (GameManager.Instance.ExplorerLevel)
        {
            case 0:
                explorerLv1Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv1); break;
            case 1:
                explorerLv1Button.interactable = false;
                explorerLv1Button.GetComponent<Image>().color = Color.green;
                explorerLv2Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv2);
                break;
            case 2:
                explorerLv1Button.interactable = false;
                explorerLv2Button.interactable = false;
                explorerLv1Button.GetComponent<Image>().color = Color.green;
                explorerLv2Button.GetComponent<Image>().color = Color.green;
                explorerLv3Button.interactable = (GameManager.Instance.ExperiencePoints >= priceLv3);
                break;
            case 3:
                explorerLv1Button.GetComponent<Image>().color = Color.green;
                explorerLv2Button.GetComponent<Image>().color = Color.green;
                explorerLv3Button.GetComponent<Image>().color = Color.green;
                explorerLv1Button.interactable = false;
                explorerLv2Button.interactable = false;
                explorerLv3Button.interactable = false;
                break;
        }
    }


    public void OnRebelLv1()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv1) return;

        GameManager.Instance.AddArchetypeLevel(0, priceLv1);

        rebelLv1Button.interactable = false;
        rebelLv2Button.interactable = true;

        UpdateState();
    }

    public void OnRebelLv2()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv2) return;

        GameManager.Instance.AddArchetypeLevel(0, priceLv2);

        rebelLv2Button.interactable = false;
        rebelLv3Button.interactable = true;

        UpdateState();
    }

    public void OnRebelLv3()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv3) return;

        GameManager.Instance.AddArchetypeLevel(0, priceLv3);

        rebelLv3Button.interactable = false;

        UpdateState();
    }

    public void OnMagicianLv1()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv1) return;

        GameManager.Instance.AddArchetypeLevel(1, priceLv1);

        magicianLv1Button.interactable = false;
        magicianLv2Button.interactable = true;

        UpdateState();
    }

    public void OnMagicianLv2()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv2) return;

        GameManager.Instance.AddArchetypeLevel(1, priceLv2);

        magicianLv2Button.interactable = false;
        magicianLv3Button.interactable = true;

        UpdateState();
    }

    public void OnMagicianLv3()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv3) return;

        GameManager.Instance.AddArchetypeLevel(1, priceLv3);

        magicianLv3Button.interactable = false;

        UpdateState();
    }

    public void OnInnocentLv1()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv1) return;

        GameManager.Instance.AddArchetypeLevel(2, priceLv1);

        innocentLv1Button.interactable = false;
        innocentLv2Button.interactable = true;

        UpdateState();
    }

    public void OnInnocentLv2()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv2) return;

        GameManager.Instance.AddArchetypeLevel(2, priceLv2);

        innocentLv2Button.interactable = false;
        innocentLv3Button.interactable = true;

        UpdateState();
    }

    public void OnInnocentLv3()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv3) return;

        GameManager.Instance.AddArchetypeLevel(2, priceLv3);

        innocentLv3Button.interactable = false;

        UpdateState();
    }

    public void OnExplorerLv1()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv1) return;

        GameManager.Instance.AddArchetypeLevel(3, priceLv1);

        explorerLv1Button.interactable = false;
        explorerLv2Button.interactable = true;

        UpdateState();
    }

    public void OnExplorerLv2()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv2) return;

        GameManager.Instance.AddArchetypeLevel(3, priceLv2);

        explorerLv2Button.interactable = false;
        explorerLv3Button.interactable = true;

        UpdateState();
    }

    public void OnExplorerLv3()
    {
        if (GameManager.Instance.ExperiencePoints < priceLv3) return;

        GameManager.Instance.AddArchetypeLevel(3, priceLv3);

        explorerLv3Button.interactable = false;

        UpdateState();
    }
}
