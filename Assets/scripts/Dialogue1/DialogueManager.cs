using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;



    //settings of dialogue visual
    public float typingSpeed = 0.03f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullText;


    public GameObject dialogueBox;
    public Button continueButton;

    public Transform choicesContainer;
    public Button choiceButtonPrefab;

    private Dialogue1 currentDialogue;
    private int currentIndex = 0;

    public System.Action onDialogueEnd;

    public bool IsDialogueOpen => dialogueBox.activeSelf;

    void Start()
    {
        dialogueBox.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    public void StartDialogue(Dialogue1 dialogue)
    {
        currentIndex = 0;
        dialogueBox.SetActive(true);
        currentDialogue = dialogue;
        currentIndex = 0;

        nameText.text = dialogue.name;
        ShowNode();
    }

    void ShowNode()
    {
       


        ClearChoices();

        DialogueNode node = currentDialogue.nodes[currentIndex];
        currentFullText = node.text;

        Debug.Log($"ShowNode index {currentIndex} / count {currentDialogue.nodes.Count}");
        if (currentIndex < 0 || currentIndex >= currentDialogue.nodes.Count)
        {
            EndDialogue();
            return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(currentFullText));


        if (node.choices != null && node.choices.Count > 0)
        {
            continueButton.gameObject.SetActive(false);
            ShowChoices(node.choices);
        }
        else
        {
            continueButton.gameObject.SetActive(true);
        }
    }





    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void OnContinueClicked()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentFullText;
            isTyping = false;
            return;
        }

        DialogueNode node = currentDialogue.nodes[currentIndex];

        if (node.nextIndex < 0)
        {
            EndDialogue();
            return;
        }

        currentIndex = node.nextIndex;
        ShowNode();
    }





    void ShowChoices(List<DialogueChoice> choices)
    {
        ClearChoices();

        foreach (var choice in choices)
        {
            Button btn = Instantiate(choiceButtonPrefab, choicesContainer);
            btn.gameObject.SetActive(true);

            var tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = choice.text;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                Debug.Log("Choice clicked: " + choice.text);
                Debug.Log("ResultKey: " + choice.resultKey);
                if (!string.IsNullOrEmpty(choice.resultKey))
                {
                    if (choice.resultKey == "advance")
                    {
                        GameState.Instance.AdvanceProgress();
                    }
                }

                if (choice.nextIndex < 0)
                {
                    EndDialogue();
                    return;
                }
                currentIndex = choice.nextIndex;
                ShowNode();
            });

        }
    }



    void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        continueButton.gameObject.SetActive(false);
        ClearChoices();


        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }

    public void Hide()
    {
        dialogueBox.SetActive(false);
    }
}
