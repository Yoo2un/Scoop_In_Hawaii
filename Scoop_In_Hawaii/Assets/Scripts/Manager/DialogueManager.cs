using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;

    public int charPerSeconds = 15;

    public TextAsset dialogueJson;

    public static DialogueManager instance;

    private DialogueData dialogueData;

    private void Awake()
    {
        instance = this;

        TextAsset dialogueJson = Resources.Load<TextAsset>("dialogues");

        if (dialogueJson == null)
        {
            Debug.LogError("dialogues.json 파일을 찾을 수 없습니다.");
            return;
        }

        dialogueData = JsonUtility.FromJson<DialogueData>(
            dialogueJson.text
        );
    }

    private void Start()
    {
        StartCoroutine(StartDialogue("MARU_DAY1"));
    }

    public IEnumerator StartDialogue(string dialogueId)
    {
        DialogueJson dialogue = null;

        foreach (var data in dialogueData.dialogues)
        {
            if (data.id == dialogueId)
            {
                dialogue = data;
                break;
            }
        }

        if (dialogue == null)
        {
            Debug.LogError($"대화를 찾을 수 없습니다: {dialogueId}");
            yield break;
        }

        nameText.text = dialogue.character;

        yield return StartCoroutine(ShowDialog(dialogue));
    }

    public IEnumerator ShowDialog(DialogueJson dialogue)
    {
        dialogueBox.SetActive(true);

        foreach (var line in dialogue.lines)
        {
            yield return StartCoroutine(TypeDialog(line));

            yield return new WaitUntil(
                () => Keyboard.current != null &&
                      Keyboard.current.zKey.wasPressedThisFrame
            );
        }

        dialogueBox.SetActive(false);
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";

        foreach (var character in dialog.ToCharArray())
        {
            dialogText.text += character;

            yield return new WaitForSeconds(
                1f / charPerSeconds
            );
        }
    }
}