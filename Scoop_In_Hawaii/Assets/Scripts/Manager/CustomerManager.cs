using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    // 싱글톤
    public static CustomerManager Instance;

    // 현재 손님이 방문 중인지 여부
    public bool isClientVisiting = false;

    // 손님 목록
    [SerializeField]
    private List<Client> clientList;

    // 현재 방문한 손님 정보
    private Client currentClient;

    // 손님 스프라이트 변경용 Renderer
    private SpriteRenderer clientSpriteRenderer;

    // 현재 손님의 주문 정보
    public IceCream currentOrder;

    // 손님 오브젝트
    private GameObject client;

    // 손님 Transform
    private Transform client_transform;

    // 손님 방향 전환 스크립트
    private View_Front client_view_front;

    // 손님 이동 코루틴
    private Coroutine walkCoroutine;

    // 손님의 초기 위치
    private Vector3 clientStartPos;

    // 주문 말풍선 UI
    private GameObject chat;

    // 주문 텍스트 UI
    private TextMeshProUGUI text_chat;

    // 손님 등장 최소 대기 시간
    [SerializeField]
    private float minClientDelay = 5f;

    // 손님 등장 최대 대기 시간
    [SerializeField]
    private float maxClientDelay = 10f;

    //손님 인내심 슬라이더
    [SerializeField]
    private GameObject patienceSliderPrefab;

    // 인내심 Slider가 생성될 Canvas
    [SerializeField]
    private Transform canvasTransform;

    // 현재 생성된 인내심 Slider
    private Slider patienceSlider;

    // 인내심 감소 코루틴
    private Coroutine patienceCoroutine;

    private float patienceDrainMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPatienceDrainMultiplier(float multiplier)
    {
        patienceDrainMultiplier = multiplier;
    }

    /// <summary>
    /// 손님 관련 오브젝트와 UI를 초기화한다.
    /// </summary>
    private void Initialize()
    {
        client = GameObject.Find("Client");
        client_transform = client.GetComponent<Transform>();
        client_view_front = client.GetComponent<View_Front>();
        clientSpriteRenderer = client.GetComponent<SpriteRenderer>();

        clientStartPos = client_transform.position;
        isClientVisiting = false;
    }

    /// <summary>
    /// 장사를 시작하고 첫 손님 방문을 예약한다.
    /// </summary>
    public void StartBusiness()
    {
        Initialize();

        Invoke(nameof(VisitClient), 5f);
    }

    public void StopBusiness()
    {
        CancelInvoke();
        StopAllCoroutines();

        if (patienceSlider != null)
        {
            Destroy(patienceSlider.gameObject);
            patienceSlider = null;
        }

        patienceCoroutine = null;

        isClientVisiting = false;
    }

    /// <summary>
    /// 새로운 손님을 생성하고
    /// 입장 및 주문 과정을 시작한다.
    /// </summary>
    private void VisitClient()
    {
        if (isClientVisiting)
            return;

        if (MachineModifier.Instance.IsAnyMachineBroken())
        {
            StartCoroutine(WaitUntilMachineFixed());
            return;
        }

        isClientVisiting = true;

        Client randomClient = clientList[UnityEngine.Random.Range(0, clientList.Count)];
        currentClient = randomClient;

        currentClient.currentPatience = currentClient.maxPatience;

        // 인내심 초기화
        clientSpriteRenderer.sprite = currentClient.sideSprite;

        StartCoroutine(StartWalk(0f));
        StartCoroutine(StartWalk(1f));
        StartCoroutine(StartWalk(2f));
        StartCoroutine(StartViewFront(3f));
        StartCoroutine(StartChat(4.5f));

    }

    private void CreatePatienceSlider()
    {
        if (patienceSlider != null)
        {
            Destroy(patienceSlider.gameObject);
        }

        GameObject sliderObject =
            Instantiate(patienceSliderPrefab, canvasTransform);

        patienceSlider = sliderObject.GetComponent<Slider>();

        patienceSlider.minValue = 0f;
        patienceSlider.maxValue = 1f;
        patienceSlider.value = 1f;
    }

    private IEnumerator PatienceProcess()
    {
        while (currentClient.currentPatience > 0f)
        {
            currentClient.currentPatience -= Time.deltaTime * patienceDrainMultiplier;

            patienceSlider.value =
                currentClient.currentPatience / currentClient.maxPatience;

            yield return null;
        }

        currentClient.currentPatience = 0f;

        patienceSlider.value = 0f;

        PatienceOver();
    }

    private void PatienceOver()
    {
        StartCoroutine(PatienceOverProcess());
    }

    private IEnumerator PatienceOverProcess()
    {
        // 주문 말풍선 변경
        text_chat.text = "너무 오래 걸리네요... \n 그냥 갈게요.";

        // 기존 말풍선이 꺼져 있을 가능성을 대비
        chat.SetActive(true);

        // Slider 제거
        if (patienceSlider != null)
        {
            Destroy(patienceSlider.gameObject);
            patienceSlider = null;
        }

        // 대사를 2초 동안 보여줌
        yield return new WaitForSeconds(2f);

        // 손님 퇴장
        LeaveWalk(3f);
    }

    /// <summary>
    /// 기계가 모두 수리될 때까지 대기한 후
    /// 일정 시간이 지나면 다시 손님을 호출한다.
    /// </summary>
    private IEnumerator WaitUntilMachineFixed()
    {
        yield return new WaitUntil(() => !MachineModifier.Instance.IsAnyMachineBroken());

        float randomDelay = UnityEngine.Random.Range(5f, 10f);
        yield return new WaitForSeconds(randomDelay);

        VisitClient();
    }

    /// <summary>
    /// 일정 시간이 지난 후
    /// 손님을 정면 방향으로 변경한다.
    /// </summary>
    public IEnumerator StartViewFront(float delay)
    {
        yield return new WaitForSeconds(delay);

        clientSpriteRenderer.sprite = currentClient.frontSprite;

        //client_view_front.Active_View_Front();
    }

    /// <summary>
    /// 일정 시간이 지난 후
    /// 손님의 주문 대화를 시작한다.
    /// </summary>
    public IEnumerator StartChat(float delay)
    {
        yield return new WaitForSeconds(delay);
        Chat();
    }

    /// <summary>
    /// 손님의 주문을 생성하고
    /// 주문 내용을 화면에 표시한다.
    /// </summary>
    private void Chat()
    {
        currentOrder = OrderGenerator.GenerateOrder();

        text_chat.text = OrderDialog.GenerateText(currentOrder);

        chat.SetActive(true);

        // 인내심 초기화
        currentClient.currentPatience = currentClient.maxPatience;

        // 인내심 Slider 생성
        CreatePatienceSlider();

        // 인내심 감소 시작
        patienceCoroutine = StartCoroutine(PatienceProcess());
    }

    /// <summary>
    /// 주문 말풍선의 내용을 변경한다.
    /// </summary>
    public void SetChatText(string message)
    {
        text_chat.text = message;
    }

    /// <summary>
    /// 일정 시간이 지난 후
    /// 손님 이동을 시작한다.
    /// </summary>
    public IEnumerator StartWalk(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClientWalk(1f);
    }

    /// <summary>
    /// 손님의 이동 코루틴을 실행한다.
    /// </summary>
    public void ClientWalk(float duration)
    {
        if (walkCoroutine != null) StopCoroutine(walkCoroutine);

        walkCoroutine = StartCoroutine(ClientWalkProcess(duration));
    }

    /// <summary>
    /// 손님이 입장 위치까지 이동하는 애니메이션을 수행한다.
    /// </summary>
    private IEnumerator ClientWalkProcess(float duration)
    {
        Vector3 start_Pos = client_transform.position;
        Vector3 dest_Pos = new Vector3(client_transform.position.x + 3, client_transform.position.y, 0);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            client_transform.position = Vector3.Lerp(start_Pos, dest_Pos, elapsed / duration);
            yield return null;
        }

        client_transform.position = dest_Pos;

        start_Pos = dest_Pos;
        dest_Pos = new Vector3(dest_Pos.x, dest_Pos.y - 0.5f, 0);
        elapsed = 0f;

        while (elapsed < (duration / 2))
        {
            elapsed += Time.deltaTime;
            client_transform.position = Vector3.Lerp(start_Pos, dest_Pos, elapsed / (duration / 2));
            yield return null;
        }

        client_transform.position = dest_Pos;
    }

    /// <summary>
    /// 손님의 퇴장 코루틴을 실행한다.
    /// </summary>
    public void LeaveWalk(float duration)
    {
        if (walkCoroutine != null) StopCoroutine(walkCoroutine);

        walkCoroutine = StartCoroutine(ClientLeaveProcess(duration));
    }

    /// <summary>
    /// 손님을 퇴장시키고
    /// 일정 시간이 지난 뒤 다음 손님을 호출한다.
    /// </summary>
    private IEnumerator ClientLeaveProcess(float duration)
    {
        chat.SetActive(false);

        if (patienceCoroutine != null)
        {
            StopCoroutine(patienceCoroutine);
            patienceCoroutine = null;
        }

        if (patienceSlider != null)
        {
            Destroy(patienceSlider.gameObject);
            patienceSlider = null;
        }

        Vector3 start_Pos = client_transform.position;

        Vector3 dest_Pos = new Vector3(client_transform.position.x - 10, client_transform.position.y, 0);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            client_transform.position = Vector3.Lerp(start_Pos, dest_Pos, elapsed / duration);

            yield return null;
        }

        client_transform.position = dest_Pos;

        isClientVisiting = false;

        float randomDelay = UnityEngine.Random.Range(minClientDelay, maxClientDelay);

        //Debug.Log($"minClientDelay: {minClientDelay:F1}초, maxClientDelay: {maxClientDelay:F1}초");
        Debug.Log($"다음 손님까지 {randomDelay:F1}초");

        yield return new WaitForSeconds(randomDelay);

        client_transform.position = clientStartPos;

        VisitClient();
    }

    /// <summary>
    /// 손님 생성 대기 시간을 변경한다.
    /// </summary>
    public void SetClientSpawnDelay(float min, float max)
    {
        minClientDelay = min;
        maxClientDelay = max;
    }

    /// <summary>
    /// 손님 생성 대기 시간을 기본값으로 초기화한다.
    /// </summary>
    public void ResetClientSpawnDelay()
    {
        minClientDelay = 10f;
        maxClientDelay = 12f;
    }

    public void SetUI(GameObject chat, TextMeshProUGUI textChat)
    {
        this.chat = chat;
        this.text_chat = textChat;
    }
}
