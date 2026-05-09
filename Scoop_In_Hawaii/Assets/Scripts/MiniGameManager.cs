using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    int day = 1;
    int[] time = new int[2] { 11, 55 };
    List<Icecream> client_Ice = null;
    int margin = 0;
    int money = 1000;
    bool gameStart = false;

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

        string han_type = "", han_mat1 = "", han_mat2 = "", han_mat3 = "", han_to1 = "", han_to2 = "";
        // Random 변수 선언
        System.Random rnd = new System.Random();
        // 타입은 1 ~ 2 까지
        int type = rnd.Next(0, 2);

        // 아이스크림 리스트 선언
        client_Ice = new List<Icecream>();

        // 타입 1은 바, 2는 콘
        if (type == 0)
        {
            client_Ice.Add(new Icecream());
            han_type = "바";
        }
        else if (type == 1)
        {
            client_Ice.Add(new Icecream_Cone());
            han_type = "콘";
        }
        client_Ice[0].type = (E_Icecream_Type)type;

        // 맛1은 1 ~ 3 까지
        int taste1 = rnd.Next(0, 3);
        // 맛을 리스트로 선언 후 추가
        client_Ice[0].taste = new List<E_Icecream_Taste> { (E_Icecream_Taste)taste1 };
        han_mat1 = ((E_Icecream_Taste)taste1).ToString();

        // 콘은 50% 확률로 2번째 맛도 정함.
        if (type == 1)
        {
            int _2 = rnd.Next(1, 3);
            if (_2 == 2)
            {
                int taste2 = rnd.Next(0, 3);
                client_Ice[0].taste.Add((E_Icecream_Taste)taste2);
                han_mat2 = ((E_Icecream_Taste)taste2).ToString();

                // 50% 확률로 3번째 맛도 정함.
                int _3 = rnd.Next(1, 3);
                if (_3 == 2)
                {
                    int taste3 = rnd.Next(0, 3);
                    client_Ice[0].taste.Add((E_Icecream_Taste)taste3);
                    han_mat3 = ((E_Icecream_Taste)taste3).ToString();
                }
            }
        }

        int _t_1 = rnd.Next(1, 3);
        // 50% 확률로 토핑도 올림.
        if (_t_1 == 1)
        {
            client_Ice[0].topping = new HashSet<E_Icecream_Topping> { 0 };
        }
        else if (_t_1 == 2)
        {
            int topping1 = rnd.Next(1, 3);
            client_Ice[0].topping = new HashSet<E_Icecream_Topping> { (E_Icecream_Topping)topping1 };
            han_to1 = ((E_Icecream_Topping)topping1).ToString();

            int _t_2 = rnd.Next(1, 3);
            if (_t_2 == 2)
            {
                E_Icecream_Topping[] excludes = { 0, (E_Icecream_Topping)topping1 }; // 제외할 목록

                var availableItems = Enum.GetValues(typeof(E_Icecream_Topping))
                                         .Cast<E_Icecream_Topping>()
                                         .Where(i => !excludes.Contains(i))
                                         .ToList();

                E_Icecream_Topping result = availableItems[new System.Random().Next(availableItems.Count)];

                client_Ice[0].topping.Add(result);
                han_to2 = result.ToString();
            }
        }

        if (type == 1)
        {

            // 콘은 50% 확률로 2번째 콘도 쌓음.
            int _2 = rnd.Next(1, 3);
            if (_2 == 1)
            {
                ((Icecream_Cone)client_Ice[0]).cone = 1;
            }
            else if (_2 == 2)
            {
                ((Icecream_Cone)client_Ice[0]).cone = 2;
            }

            //콘은 50% 확률로 시럽도 추가함.
            int _s = rnd.Next(1, 3);
            if (_s == 1)
            {
                ((Icecream_Cone)client_Ice[0]).syrup_Taste = 0;
            }
            if (_s == 2)
            {
                int _s_2 = rnd.Next(1, 3);
                ((Icecream_Cone)client_Ice[0]).syrup_Taste = (E_Syrup_Taste)_s_2;
            }
        }



        if (type == 0)
        {
            // 바
            if (!han_to1.Equals(""))
            {
                text_chat.text = $"아이스크림 {han_type}로 맛은 {han_mat1} (으)로 주시고, 토핑은 {han_to1}{(!han_to2.Equals("") ? ", " + han_to2 : "")} (으)로 주세요.";
            }
            else
            {
                text_chat.text = $"아이스크림 {han_type}로 맛은 {han_mat1} (으)로 주시고, 토핑은 안 주셔도 돼요.";
            }
        }
        else if (type == 1)
        {
            // 콘
            if (!han_to1.Equals(""))
            {
                text_chat.text = $"아이스크림 {han_type}으로 맛은 1층부터 {han_mat1}{(!han_mat2.Equals("") ? ", " + han_mat2 : "")}{(!han_mat3.Equals("") ? ", " + han_mat3 : "")} 순으로 주시고, 토핑은 {han_to1}{(!han_to2.Equals("") ? ", " + han_to2 : "")} (으)로 주세요. 아, 그리고 콘은 {((Icecream_Cone)client_Ice[0]).cone}개로 주시고, 시럽은 {((Icecream_Cone)client_Ice[0]).syrup_Taste.ToString()} (으)로 할게요.";
            }
            else
            {
                text_chat.text = $"아이스크림 {han_type}으로 맛은 1층부터 {han_mat1}{(!han_mat2.Equals("") ? ", " + han_mat2 : "")}{(!han_mat3.Equals("") ? ", " + han_mat3 : "")} 순으로 주시고, 토핑은 안 주셔도 돼요. 아, 그리고 콘은 {((Icecream_Cone)client_Ice[0]).cone}개로 주시고, 시럽은 {((Icecream_Cone)client_Ice[0]).syrup_Taste.ToString()} (으)로 할게요.";
            }
        }

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

        while (elapsed < (duration / 2 ))
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
