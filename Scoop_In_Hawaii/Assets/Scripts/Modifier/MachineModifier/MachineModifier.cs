using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MachineModifier : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private float breakChance = 0.1f; // 고장 확률 기본 10%

    [SerializeField] private Image coneMachine;
    [SerializeField] private Image barMachine;
    [SerializeField] private Image warningIcon;

    [SerializeField] private GameObject ConeWarningPoint;
    [SerializeField] private GameObject BarWarningPoint;

    private bool coneBroken = false;
    private bool barBroken = false;

    MachineUIManager machineUIManager;
    public bool ConeBroken => coneBroken;
    public bool BarBroken => barBroken;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        machineUIManager = FindAnyObjectByType<MachineUIManager>();
        StartCoroutine(BreakRoutine());
        warningIcon.gameObject.SetActive(false);
    }

    private IEnumerator BreakRoutine()
    {
        while (true)
        {
            // 테스트용 5초 (실제 게임에서는 120초)
            yield return new WaitForSeconds(1f);

            // 손님이 있으면 이번 고장 판정은 건너뜀
            if (gameManager.isClientVisiting)
            {
                Debug.Log("손님이 있어서 판정을 건너뜁니다.");
                continue;
            }
           
            float randomValue = Random.value;

            Debug.Log($"고장 확률 : {breakChance}");
            Debug.Log($"랜덤 값 : {randomValue}");

            // 고장 확률 판정
            if (randomValue <= breakChance)
            {
                Debug.Log("기계 고장 발생!");

                // 0 = 콘, 1 = 바
                int machine = Random.Range(0, 2);

                if (machine == 0)
                {
                    if (!coneBroken)
                    {
                        coneBroken = true;
                        StartCoroutine(BlinkMachine(coneMachine));
                        warningIcon.transform.position = ConeWarningPoint.transform.position;
                        warningIcon.gameObject.SetActive(true);
                        Debug.Log("콘 기계 고장!");
                    }
                    else
                    {
                        Debug.Log("이미 콘 기계가 고장난 상태입니다.");
                    }
                }
                else
                {
                    if (!barBroken)
                    {
                        barBroken = true;
                        StartCoroutine(BlinkMachine(barMachine));
                        warningIcon.transform.position = BarWarningPoint.transform.position;
                        warningIcon.gameObject.SetActive(true);
                        Debug.Log("바 기계 고장!");
                    }
                    else
                    {
                        Debug.Log("이미 바 기계가 고장난 상태입니다.");
                    }
                }
            }
            else
            {
                Debug.Log("고장 발생X");
            }
        }
    }

    // 기계를 빨간색으로 깜빡이게 하는 코루틴
    private IEnumerator BlinkMachine(Image image)
    {
        while (true)
        {
            image.color = new Color(1f, 0.6f, 0.6f, 1f);
            yield return new WaitForSeconds(0.3f);

            image.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void SelfRepair()
    {
        machineUIManager.OpenRepairMiniGame();
    }

    public void RepairComplete()
    {
        if (coneBroken)
        {
            coneBroken = false;
            StopCoroutine(BlinkMachine(coneMachine));
            coneMachine.color = Color.white;
        }
        else if (barBroken)
        {
            barBroken = false;
            StopCoroutine(BlinkMachine(barMachine));
            barMachine.color = Color.white;
        }

        warningIcon.gameObject.SetActive(false);

        Debug.Log("수리 완료!");
    }
}