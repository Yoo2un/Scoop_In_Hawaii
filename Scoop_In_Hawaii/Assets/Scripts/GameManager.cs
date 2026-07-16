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

    public int profit = 0;
    public int material_cost = 0; // 재료비

    List<IceCream> client_Ice = null;
    int margin = 0;
    int money = 0;
    bool gameStart = false;

    public bool isClientVisiting = false;

    Vector3 clientStartPos;

    [SerializeField]
    private List<Client> clientList;

    private Client currentClient;

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

    public TMP_Text moneyText;

    [SerializeField]
    private float minClientDelay = 5f;

    [SerializeField]
    private float maxClientDelay = 10f;

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

            obj_Day.SetActive(true);
            obj_Num.SetActive(true);

            DayManager.Instance.SetUI(text_hour, text_Num);
            DayManager.Instance.ResetDay();
            DayManager.Instance.StartTime();

            Debug.Log($"하루 시작(현재 {DayManager.Instance.day}일차 아침)");

            profit = 0;

            obj_hour.SetActive(true);

            chat = GameObject.Find("UI_Chat");
            text_chat = GameObject.Find("Text_Chat").GetComponent<TextMeshProUGUI>();
            chat.SetActive(false);

            dayCtrlBtn = GameObject.Find("DayCtrlBtn");
            dayCtrlBtn.GetComponent<Image>().color = new Color32(126, 255, 109, 255);
            dayCtrlBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장사 시작";
            Button _btn = dayCtrlBtn.GetComponent<Button>();
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(DayStart);

            obj_Day.SetActive(false);
            obj_Num.SetActive(false);
            text_Day = obj_Day.GetComponent<TextMeshProUGUI>();
            day_RectTransform = obj_Day.GetComponent<RectTransform>();
            num_RectTransform = obj_Num.GetComponent<RectTransform>();

            Invoke("Text_Day_Function", 2.0f);
            Invoke("Text_Num_Function", 3.0f);

            StartCoroutine(StartShrink(5.0f));
            StartCoroutine(StartTextPosReset(5.0f));
        }
        else if (scene.name.Equals("Result"))
        {
            ShowResult();
        }
    }

        
    public void DayStart()
    {
        if (DayManager.Instance.dayState == DayState.Morning) {
            Button _btn = dayCtrlBtn.GetComponent<Button>();
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(CloseBusiness);
            dayCtrlBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장사 종료";
            dayCtrlBtn.GetComponent<Image>().color = new Color32(212, 47, 41, 255);

            profit = 0;
            DayManager.Instance.SetState(DayState.Open);
            Debug.Log("장사 시작");

            client = GameObject.Find("Client");
            client_transform = client.GetComponent<Transform>();
            client_view_front = client.GetComponent<View_Front>();
            clientSpriteRenderer = client.GetComponent<SpriteRenderer>();
            clientStartPos = client_transform.position;
            isClientVisiting = false;

            gameStart = true;

            Invoke("VisitClient", 5.0f);
        }
    }

    public void CloseBusiness()
    {
        if (DayManager.Instance.dayState == DayState.Morning || DayManager.Instance.dayState == DayState.Open)
        {
            DayManager.Instance.SetState(DayState.Closed);

            CancelInvoke();
            DayManager.Instance.StopTime();
            StopAllCoroutines();

            Debug.Log("장사 종료");

            Button _btn = dayCtrlBtn.GetComponent<Button>();
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(DayEnd);
            dayCtrlBtn.GetComponentInChildren<TextMeshProUGUI>().text = "하루 종료";
            dayCtrlBtn.GetComponent<Image>().color = new Color32(248, 224, 36, 255);
        }
    }

    public void DayEnd()
    {
        if (DayManager.Instance.dayState == DayState.Closed)
        {
            DayManager.Instance.SetState(DayState.Result);

            CancelInvoke();
            DayManager.Instance.StopTime();
            StopAllCoroutines();

            DataPersistenceManager.instance.SaveGame();
            Debug.Log("하루 종료");
            SceneManager.LoadScene("Result");
        }
    }

    public void ShowResult()
    {
        Debug.Log("장사 결과 정산");
    }

    public void UseModifier()
    {
        Debug.Log($"useModifier() 호출됨. 현재 발동된 모디파이어: {modifier}");

        switch (modifier)
        {
            case ModifierType.GoodEvent:
                //SNSViralMdodifier.Instance.SNS_Viral();
                //TrendFlavorModifier.Instance.StartTrendFlavor();
                break;

            case ModifierType.BadEvent:
                MachineModifier.Instance.BreakMachine();
                //SeagullModifier.Instance.FlySeagull();
                break;
        }
    }

    void VisitClient()
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
        clientSpriteRenderer.sprite = currentClient.sideSprite;

        StartCoroutine(StartWalk(0f));
        StartCoroutine(StartWalk(1f));
        StartCoroutine(StartWalk(2f));
        StartCoroutine(StartViewFront(3f));
        StartCoroutine(StartChat(4.5f));
    }

    private IEnumerator WaitUntilMachineFixed()
    {
        yield return new WaitUntil(() => !MachineModifier.Instance.IsAnyMachineBroken());

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

    //갈매기 습격 때 쓸 텍스트 넘겨주기
    public void SetChatText(string message)
    {
        text_chat.text = message;
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
        float randomDelay = UnityEngine.Random.Range(minClientDelay, maxClientDelay);

        //Debug.Log($"minClientDelay: {minClientDelay:F1}초, maxClientDelay: {maxClientDelay:F1}초");
        Debug.Log($"다음 손님까지 {randomDelay:F1}초");

        yield return new WaitForSeconds(randomDelay);

        //�մ� ��ġ �ʱ�ȭ
        client_transform.position = clientStartPos;

        VisitClient();
    }

    public void SetClientSpawnDelay(float min, float max)
    {
        minClientDelay = min;
        maxClientDelay = max;
    }

    public void ResetClientSpawnDelay()
    {
        minClientDelay = 30f;
        maxClientDelay = 60f;
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