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

    private MachineModifier machineModifier;
    private void Start()
    {
        machineModifier = FindAnyObjectByType<MachineModifier>();

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
        successZonePivot.rotation = Quaternion.Euler(0f, 0f, angle);
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

        if (successCount >= maxSuccess)
        {
            machineModifier.RepairComplete();
            RepairMiniGamePanel.SetActive(false);
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

        if (life <= 0)
        {
            machineModifier.IncreaseBreakChance(0.05f);
            machineModifier.RepairComplete();
            RepairMiniGamePanel.SetActive(false);
        }
    }

    public void InitMiniGame()
    {
        successCount = 0;
        life = 5;

        successText.text = "0/" + maxSuccess;

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = heartSprite;
        }

        RandomSuccessZone();
    }
}
