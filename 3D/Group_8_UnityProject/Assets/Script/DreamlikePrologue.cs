using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DreamlikePrologue : MonoBehaviour
{
    [Header("Text")]
    public List<string> sentences;
    public Text prologueText;

    [Header("Dream effect")]
    public float typeSpeed = 0.07f;
    public float fadeInDuration = 0.5f;
    public float pauseBetweenSentences = 0.2f;

    [Header("Slight fluttering")]
    public bool enableFloat = true;
    public float floatSpeed = 0.5f;
    public float floatDistance = 3f;

    [Header("After the preface - Display Group")]
    public GameObject[] objectsToShow;
    public float delayBeforeShow = 0.5f;

    [Header("After the preface - The Absent Group")]
    public GameObject[] objectsToHide;
    public bool hideImmediately = false;

    [Header("After the preface - Text")]
    public bool fadeOutText = true;
    public float fadeOutDuration = 0.5f;

    private int currentIndex = 0;
    private bool isWaitingForInput = false;
    private Vector2 originalTextPos;

    void Start()
    {
        if (prologueText != null)
        {
            originalTextPos = prologueText.rectTransform.anchoredPosition;
            prologueText.text = "";

            // Make sure that the group of objects is initially hidden when displayed.
            HideShowGroup();

            // Make sure that the objects in the disappearance group are initially displayed.
            ShowHideGroup();

            StartCoroutine(RunPrologue());
        }
    }

    // Hidden/Visible Group (Initially)
    void HideShowGroup()
    {
        if (objectsToShow != null)
        {
            foreach (GameObject obj in objectsToShow)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }

    // Display the "Hide" group (at the beginning)
    void ShowHideGroup()
    {
        if (objectsToHide != null)
        {
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (isWaitingForInput && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            isWaitingForInput = false;
            StartCoroutine(ShowNextSentence());
        }

        if (enableFloat && prologueText.text != "" && !isWaitingForInput)
        {
            float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatDistance;
            prologueText.rectTransform.anchoredPosition = originalTextPos + new Vector2(0, offsetY);
        }
    }

    IEnumerator RunPrologue()
    {
        yield return StartCoroutine(ShowNextSentence());
    }

    IEnumerator ShowNextSentence()
    {
        if (currentIndex >= sentences.Count)
        {
            yield return StartCoroutine(EndPrologue());
            yield break;
        }

        string sentence = sentences[currentIndex];
        currentIndex++;

        yield return StartCoroutine(TypeWithFade(sentence));
        isWaitingForInput = true;
    }

    IEnumerator TypeWithFade(string sentence)
    {
        prologueText.text = "";
        prologueText.color = new Color(prologueText.color.r, prologueText.color.g, prologueText.color.b, 0);

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsed / fadeInDuration);
            prologueText.color = new Color(prologueText.color.r, prologueText.color.g, prologueText.color.b, alpha);
            yield return null;
        }
        prologueText.color = new Color(prologueText.color.r, prologueText.color.g, prologueText.color.b, 1);

        foreach (char c in sentence)
        {
            prologueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(pauseBetweenSentences);
    }

    IEnumerator EndPrologue()
    {
        if (hideImmediately)
        {
            HideObjects();
        }

        yield return new WaitForSeconds(delayBeforeShow);

        if (!hideImmediately)
        {
            HideObjects();
        }

        if (fadeOutText && prologueText != null)
        {
            float elapsed = 0f;
            Color originalColor = prologueText.color;

            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, elapsed / fadeOutDuration);
                prologueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            prologueText.text = "";
        }

        // Display display group objects
        ShowObjects();

        Debug.Log("Preface ends: The hidden group has been hidden, and the displayed group has been displayed.");
    }

    // The Hidden and Disappearing Group
    void HideObjects()
    {
        if (objectsToHide != null)
        {
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    Debug.Log("Hidden:" + obj.name);
                }
            }
        }
    }

    // Display Display Group
    void ShowObjects()
    {
        if (objectsToShow != null)
        {
            foreach (GameObject obj in objectsToShow)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    Debug.Log("It has been shown:" + obj.name);
                }
            }
        }
    }
}