using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class RepairMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject RepairMiniGamePanel;

    [SerializeField] private RectTransform wrenchPivot;
    [SerializeField] private RectTransform successZonePivot;
    [SerializeField]
    private float rotateSpeed = 180f;

    [SerializeField] private Text successText;
    [SerializeField] private int maxSuccess = 3;
    private int successCount = 0;

    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite brokenHeartSprite;
    private int life = 5;
    private void Start()
    {
        RepairMiniGamePanel.gameObject.SetActive(true);
        successCount = 0;
        successText.text = "0/" + maxSuccess;
        RandomSuccessZone();
    }

    private void Update()
    {
        wrenchPivot.Rotate(0, 0, -rotateSpeed * Time.deltaTime);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckSuccess();
        }
    }
    private void RandomSuccessZone()
    {
        float angle = Random.Range(0f, 360f);
        successZonePivot.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void CheckSuccess()
    {
        float wrenchAngle = wrenchPivot.eulerAngles.z;
        float zoneAngle = successZonePivot.eulerAngles.z;

        float diff = Mathf.Abs(Mathf.DeltaAngle(wrenchAngle, zoneAngle));

        Debug.Log(diff);

        if (diff < 20f)
        {
            Success();
        }
        else
        {
            Fail();
        }
    }

    private void Success()
    {
        successCount++;

        successText.text = successCount + "/" + maxSuccess;

        Debug.Log("성공!");

        if (successCount >= maxSuccess)
        {
            Debug.Log("수리 완료!");
            // TODO : 미니게임 종료
        }
        else
        {
            RandomSuccessZone();
        }
    }

    private void Fail()
    {
        life--;

        heartImages[life].sprite = brokenHeartSprite;

        Debug.Log("실패!");

        if (life <= 0)
        {
            Debug.Log("수리 실패!");
        }
    }

}
