
using System.Collections;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity = 3f; //글자가 얼마나 크게 흔들릴지
    [SerializeField] private float shakeSpeed = 8f; //흔들림 속도

    private Coroutine shakeCoroutine;

    // 흔들림 시작
    public void StartShakeText(TextMeshProUGUI tmpText)
    {
        //이미 흔들리는 중이라면 중단하고 텍스트 원상복구
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            RestoreText(tmpText);
        }

        shakeCoroutine = StartCoroutine(CoShakeText(tmpText));
    }

    // 흔들림 종료
    public void StopShakeText(TextMeshProUGUI tmpText)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        RestoreText(tmpText);
    }

    private IEnumerator CoShakeText(TextMeshProUGUI tmpText)
    {
        // 1. TMP 메시 정보 갱신
        // 글자의 모양을 표현하는 정점들의 집합
        tmpText.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpText.textInfo;

        // 2. 원래 정점 정보 저장
        //[메시의 개수][각 메시를 구성하는 정점들의 좌표]
        Vector3[][] originalVertices = new Vector3[textInfo.meshInfo.Length][];

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = (Vector3[])textInfo.meshInfo[i].vertices.Clone();
        }

        // 3. 흔들림 반복
        while (true)
        {
            // 매 프레임 원래 정점 위치로 복구
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                System.Array.Copy(
                    originalVertices[i],
                    textInfo.meshInfo[i].vertices,
                    originalVertices[i].Length
                );
            }

            // 4. 각 글자에 흔들림 적용
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                int vi = charInfo.vertexIndex;
                int mi = charInfo.materialReferenceIndex;

                Vector3[] verts = textInfo.meshInfo[mi].vertices;

                float x = (Mathf.PerlinNoise(
                    i * 0.1f,
                    Time.unscaledTime * shakeSpeed
                ) - 0.5f) * 2f;

                float y = (Mathf.PerlinNoise(
                    i * 0.1f + 100f,
                    Time.unscaledTime * shakeSpeed
                ) - 0.5f) * 2f;

                Vector3 offset = new Vector3(x, y, 0f) * shakeIntensity;

                // 글자의 정점 4개 이동
                for (int j = 0; j < 4; j++)
                {
                    verts[vi + j] += offset;
                }
            }

            // 5. 변경된 정점 정보를 TMP에 적용
            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            yield return null;
        }
    }

    // 원래 텍스트 위치로 복구
    private void RestoreText(TextMeshProUGUI tmpText)
    {
        tmpText.ForceMeshUpdate();
        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
