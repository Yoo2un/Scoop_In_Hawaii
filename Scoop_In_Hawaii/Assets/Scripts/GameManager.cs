using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }

    // 임시 모디파이어 enum
    public enum ModifierType { None, GoodEvent, BadEvent }
    public ModifierType modifier = ModifierType.None;

    public DayState dayState;
    public int day = 1;
    public int profit = 0;
    public int material_cost = 0; // 재료비

    int[] time = new int[2] { 11, 55 };
    List<IceCream> client_Ice = null;
    int margin = 0;
    int money = 0;
    bool gameStart = false;

    //�մ� �湮 ���� - ���� �մ� �湮�� �� ���
    public bool isClientVisiting = false;

    //�մ� ���� ��ġ ����
    Vector3 clientStartPos;

    //�մ� ����Ʈ
    [SerializeField]
    private List<Client> clientList;

    //���� �մ�
    private Client currentClient;

    //�մ� �̹�����
    SpriteRenderer clientSpriteRenderer;

    public IceCream currentOrder;

    GameObject obj_Day = null;
    GameObject obj_Num = null;
    GameObject client = null;
    GameObject chat = null;
    GameObject dayCtrlBtn = null;

    Transform client_transform;
    View_Front client_view_front;

    TextMeshProUGUI text_Day;
    TextMeshProUGUI text_Num;
    TextMeshProUGUI text_hour;
    TextMeshProUGUI text_chat;
    Vector3 day_pre_Position;
    Vector3 num_pre_Position;
    RectTransform day_RectTransform;
    RectTransform num_RectTransform;

    private Coroutine shrinkCoroutine;
    private Coroutine posCoroutine;
    private Coroutine walkCoroutine;
    private Coroutine timePassesCoroutine;

    private MachineModifier machineModifier;

    public TMP_Text moneyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadData(GameData data)
    {
        this.money = data.money;
        moneyText.text = money.ToString();
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
        moneyText.text = money.ToString();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.name.Equals("DayScene"))
        {
            return;
        }
    }

    IEnumerator TimePasses()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            ++time[1];
            while (time[1] >= 60)
            {
                time[1] = 0;
                ++time[0];
            }
            text_hour.text = $"{time[0]}:{time[1]:D2} PM";
            //text_minute.text = $"{time[1]}";
            text_Num.text = $"{day}";
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        CancelInvoke();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("DayScene"))
        {

            machineModifier = FindAnyObjectByType<MachineModifier>();

            obj_Day = GameObject.Find("Day_Text_Day");
            obj_Num = GameObject.Find("Day_Text_Num");
            obj_Day.SetActive(false);
            obj_Num.SetActive(false);
            
            text_Day = obj_Day.GetComponent<TextMeshProUGUI>();
            text_Num = obj_Num.GetComponent<TextMeshProUGUI>();
            GameObject obj_hour = GameObject.Find("Time_Text_Hour");
            text_hour = obj_hour.GetComponent<TextMeshProUGUI>();
            
            day_RectTransform = obj_Day.GetComponent<RectTransform>();
            num_RectTransform = obj_Num.GetComponent<RectTransform>();

            moneyText = GameObject.Find("Money_Text").GetComponent<TextMeshProUGUI>();
            dayState = DayState.Morning;
            Debug.Log($"하루 시작(현재 {day}일차 아침)");

            text_Num.text = $"{day}";
            obj_Day.SetActive(true);
            obj_Num.SetActive(true);

            time[0] = 11;
            time[1] = 55;
            profit = 0;

            text_hour.text = "11:55 PM";
            obj_hour.SetActive(true);

            chat = GameObject.Find("UI_Chat");
            text_chat = GameObject.Find("Text_Chat").GetComponent<TextMeshProUGUI>();
            chat.SetActive(false);

            dayCtrlBtn = GameObject.Find("DayCtrlBtn");
            dayCtrlBtn.GetComponent<Image>().color = new Color(126f, 255f, 109f, 1f);
            dayCtrlBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장사 시작";
            Button _btn = dayCtrlBtn.GetComponent<Button>();
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(DayStart);
            
        }
        else if (scene.name.Equals("Result"))
        {
            ShowResult();
        }
    }

        
    public void DayStart()
    {
        if (dayState == DayState.Morning) {
            Button _btn = dayCtrlBtn.GetComponent<Button>();
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(DayEnd);
            dayCtrlBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장사 종료";
            dayCtrlBtn.GetComponent<Image>().color = new Color(212f, 47f, 41f, 1f);

            profit = 0;
            dayState = DayState.Open;
            Debug.Log("장사 시작");

            obj_Day.SetActive(false);
            obj_Num.SetActive(false);
            text_Day = obj_Day.GetComponent<TextMeshProUGUI>();
            day_RectTransform = obj_Day.GetComponent<RectTransform>();
            num_RectTransform = obj_Num.GetComponent<RectTransform>();

            client = GameObject.Find("Client");
            client_transform = client.GetComponent<Transform>();
            client_view_front = client.GetComponent<View_Front>();
            clientSpriteRenderer = client.GetComponent<SpriteRenderer>();
            clientStartPos = client_transform.position;
            isClientVisiting = false;

            time[0] = 11;
            time[1] = 55;

            gameStart = true;
            Invoke("Text_Day_Function", 2.0f);
            Invoke("Text_Num_Function", 3.0f);

            StartCoroutine(StartShrink(5.0f));
            StartCoroutine(StartTextPosReset(5.0f));

            if (timePassesCoroutine != null)
            {
                StopCoroutine(timePassesCoroutine);
            }
            timePassesCoroutine = StartCoroutine(TimePasses());

            Invoke("VisitClient", 5.0f);
        }
    }

    public void DayEnd()
    {
        if (dayState == DayState.Open)
        {
            dayState = DayState.Result;

            CancelInvoke();
            if (timePassesCoroutine != null)
            {
                StopCoroutine(timePassesCoroutine);
                timePassesCoroutine = null;
            }
            StopAllCoroutines();

            Debug.Log("장사 종료");
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene("Result");
        }
    }

    public void ShowResult()
    {
        Debug.Log("장사 결과 정산");
    }
    public void NextDay()
    {
        day++;
        SceneManager.LoadScene("DayScene");
    }

    public void UseModifier()
    {
        Debug.Log($"useModifier() 호출됨. 현재 발동된 모디파이어: {modifier}");
    }

    void VisitClient()
    {
        if (isClientVisiting)
            return;

        if (machineModifier.IsAnyMachineBroken())
        {
            StartCoroutine(WaitUntilMachineFixed());
            return;
        }

        isClientVisiting = true;

        Client randomClient = clientList[UnityEngine.Random.Range(0, clientList.Count)];
        currentClient = randomClient;
        clientSpriteRenderer.sprite = currentClient.sideSprite;

        StartCoroutine(StartWalk(0f));
        StartCoroutine(StartWalk(1f));
        StartCoroutine(StartWalk(2f));
        StartCoroutine(StartViewFront(3f));
        StartCoroutine(StartChat(4.5f));
    }

    private IEnumerator WaitUntilMachineFixed()
    {
        yield return new WaitUntil(() => !machineModifier.IsAnyMachineBroken());

        float randomDelay = UnityEngine.Random.Range(5f, 10f);
        yield return new WaitForSeconds(randomDelay);

        VisitClient();
    }

    public IEnumerator StartViewFront(float delay)
    {
        yield return new WaitForSeconds(delay);

        clientSpriteRenderer.sprite = currentClient.frontSprite;

        //client_view_front.Active_View_Front();
    }

    public IEnumerator StartChat(float delay)
    {
        yield return new WaitForSeconds(delay);
        Chat();
    }

    private void Chat()
    {
        currentOrder = OrderGenerator.GenerateOrder();

        text_chat.text = OrderDialog.GenerateText(currentOrder);

        chat.SetActive(true);
    }

    public IEnumerator StartWalk(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClientWalk(1f);
    }

    public void ClientWalk(float duration)
    {
        if (walkCoroutine != null) StopCoroutine(walkCoroutine);

        walkCoroutine = StartCoroutine(ClientWalkProcess(duration));
    }

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

    public void LeaveWalk(float duration)
    {
        if (walkCoroutine != null) StopCoroutine(walkCoroutine);

        walkCoroutine = StartCoroutine(ClientLeaveProcess(duration));
    }

    private IEnumerator ClientLeaveProcess(float duration)
    {
        chat.SetActive(false);

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

        //���� ���� �մ� ����
        isClientVisiting = false;

        //���� ��� �ð�
        float randomDelay = UnityEngine.Random.Range(5f, 10f);

        Debug.Log($"다음 손님까지 {randomDelay:F1}초");

        yield return new WaitForSeconds(randomDelay);

        //�մ� ��ġ �ʱ�ȭ
        client_transform.position = clientStartPos;

        VisitClient();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Text_Day_Function()
    {
        day_pre_Position = day_RectTransform.position;

        text_Day.fontSize = 120;
        day_RectTransform.position = GameObject.Find("Pos_Day").GetComponent<RectTransform>().position;
        obj_Day.SetActive(true);
    }

    void Text_Num_Function()
    {
        num_pre_Position = num_RectTransform.position;

        text_Num.fontSize = 120;
        num_RectTransform.position = GameObject.Find("Pos_Num").GetComponent<RectTransform>().position;
        obj_Num.SetActive(true);
    }

    public IEnumerator StartShrink(float delay)
    {
        yield return new WaitForSeconds(delay);
        Shrink(36f, 5.0f);
    }

    public void Shrink(float targetSize, float duration)
    {
        if (shrinkCoroutine != null) StopCoroutine(shrinkCoroutine);

        shrinkCoroutine = StartCoroutine(ShrinkProcess(targetSize, duration));
    }

    private IEnumerator ShrinkProcess(float targetSize, float duration)
    {
        float day_StartSize = text_Day.fontSize;
        float num_StartSize = text_Num.fontSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text_Day.fontSize = Mathf.Lerp(day_StartSize, targetSize, elapsed / duration);
            text_Num.fontSize = Mathf.Lerp(num_StartSize, targetSize, elapsed / duration);
            yield return null;
        }

        text_Day.fontSize = targetSize;
        text_Day.fontSize = targetSize;
    }

    public IEnumerator StartTextPosReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        TextPosReset(3.0f);
    }

    public void TextPosReset(float duration)
    {
        if (posCoroutine != null) StopCoroutine(posCoroutine);

        posCoroutine = StartCoroutine(TextPosResetProcess(duration));
    }

    private IEnumerator TextPosResetProcess(float duration)
    {
        Vector3 day_Start_Pos = day_RectTransform.position;
        Vector3 num_Start_Pos = num_RectTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            day_RectTransform.position = Vector3.Lerp(day_Start_Pos, day_pre_Position, elapsed / duration);
            num_RectTransform.position = Vector3.Lerp(num_Start_Pos, num_pre_Position, elapsed / duration);
            yield return null;
        }

        day_RectTransform.position = day_pre_Position;
        num_RectTransform.position = num_pre_Position;

    }

    public int getMoney()
    {
        return money;
    }

    public void setMoney(int money)
    {
        this.money = money;
        moneyText.text = this.money.ToString();
    }

    public void addMoney(int money)
    {
        this.money += money;
        moneyText.text = this.money.ToString();
    }

    public void subtractMoney(int money)
    {
        this.money -= money;
        moneyText.text = this.money.ToString();
    }
}