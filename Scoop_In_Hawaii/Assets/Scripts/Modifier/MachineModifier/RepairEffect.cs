using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RepairEffect : MonoBehaviour
{
    [SerializeField] private Image FixManImage;
    [SerializeField] private TMP_Text completeText;

    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] MachineModifier machineModifier;

    private void Awake()
    {
        FixManImage.gameObject.SetActive(true);

        Color imageColor = FixManImage.color;
        imageColor.a = 0;
        FixManImage.color = imageColor;
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float time = 0f;

        Color color = FixManImage.color;
        color.a = startAlpha;
        FixManImage.color = color;

        while (time < fadeTime)
        {
            time += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, endAlpha, time / fadeTime);
            FixManImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        FixManImage.color = color;
    }

    private IEnumerator FadeOutText()
    {
        float time = 0f;

        Color color = completeText.color;
        color.a = 1f;
        completeText.color = color;

        while (time < fadeTime)
        {
            time += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, time / fadeTime);
            completeText.color = color;

            yield return null;
        }

        color.a = 0f;
        completeText.color = color;
    }

    public IEnumerator PlayRepairEffect()
    {
        Debug.Log("PlayRepairEffect");

        // 수리기사 등장
        yield return StartCoroutine(FadeImage(0f, 1f));

        yield return new WaitForSeconds(2f);

        // 수리기사 퇴장
        yield return StartCoroutine(FadeImage(1f, 0f));

        completeText.gameObject.SetActive(true);

        // 수리 완료 표시
        completeText.text = "수리 완료!";

        yield return new WaitForSeconds(1f);

        // 텍스트만 사라짐
        yield return StartCoroutine(FadeOutText());

        GameManager.Instance.subtractMoney(500);
        machineModifier.RepairComplete();
    }
}
