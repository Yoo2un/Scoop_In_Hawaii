using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class GameManager : MonoBehaviour
{
    int day = 1;
    int[] time = new int[2] { 11, 55 };
    List<IceCream> client_Ice = null;
    int margin = 0;
    int money = 1000;
    bool gameStart = false;

    IceCream currentOrder;

    GameObject obj_Day = null;
    GameObject obj_Num = null;
    GameObject client = null;
    GameObject chat = null;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("DayScene"))
        {
            return;
        }

        obj_Day = GameObject.Find("Day_Text_Day");
        obj_Num = GameObject.Find("Day_Text_Num");
        obj_Day.SetActive(false);
        obj_Num.SetActive(false);
        text_Day = obj_Day.GetComponent<TextMeshProUGUI>();
        text_Num = obj_Num.GetComponent<TextMeshProUGUI>();
        text_hour = GameObject.Find("Time_Text_Hour").GetComponent<TextMeshProUGUI>();
        day_RectTransform = obj_Day.GetComponent<RectTransform>();
        num_RectTransform = obj_Num.GetComponent<RectTransform>();

        client = GameObject.Find("Client");
        client_transform = client.GetComponent<Transform>();
        client_view_front = client.GetComponent<View_Front>();

        chat = GameObject.Find("UI_Chat");
        text_chat = GameObject.Find("Text_Chat").GetComponent<TextMeshProUGUI>();
        chat.SetActive(false);

        time[0] = 11;
        time[1] = 55;

        gameStart = true;
        Invoke("Text_Day_Function", 2.0f);
        Invoke("Text_Num_Function", 3.0f);

        StartCoroutine(StartShrink(5.0f));
        StartCoroutine(StartTextPosReset(5.0f));
        StartCoroutine(TimePasses());
        Invoke("VisitClient", 5.0f);
    }

    void VisitClient()
    {
        StartCoroutine(StartWalk(0f));
        StartCoroutine(StartWalk(3f));
        StartCoroutine(StartWalk(6f));
        StartCoroutine(StartViewFront(9f));
        StartCoroutine(StartChat(10f));
    }

    public IEnumerator StartViewFront(float delay)
    {
        yield return new WaitForSeconds(delay);
        client_view_front.Active_View_Front();
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
        ClientWalk(2f);
    }

    public void ClientWalk(float duration)
    {
        if (walkCoroutine != null) StopCoroutine(walkCoroutine);

        walkCoroutine = StartCoroutine(ClientWalkProcess(duration));
    }

    private IEnumerator ClientWalkProcess(float duration)
    {
        Vector3 start_Pos = client_transform.position;
        Vector3 dest_Pos = new Vector3(client_transform.position.x + 3, client_transform.position.y + 0.5f, 0);
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

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
}