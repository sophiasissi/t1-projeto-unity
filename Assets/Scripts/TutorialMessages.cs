using UnityEngine;
using TMPro;

public class TutorialMessages : MonoBehaviour
{
    public TMP_Text tutorialText;

    private float timer = 0f;

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer < 4f)
        {
            tutorialText.text = "Pressione ESPAÇO para pular!";
        }
        else if (timer < 8f)
        {
            tutorialText.text = "Colete cafés para ganhar pontos!";
        }
        else if (timer < 12f)
        {
            tutorialText.text = "Desvie das mochilas e cones!";
        }
        else if (timer < 16f)
        {
            tutorialText.text = "Não se atrase para a aula!";
        }
        else
        {
            tutorialText.text = "";
        }
    }
}