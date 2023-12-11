using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Map;
using System;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public class EvenetStage : MonoBehaviour
{
    MapPlayerTracker mapPlayerTracker;
    public List<EvenetStage> otherEventStages;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public Image image;

    public Sprite restSprite;
    public Sprite augmentSprite;
    public Sprite patternSprite;


    public Augment currentAugment;
    public Augment augmentInfinity;
    public Pattern currentPattern;
    public Button button;
    public EventType eventType;
    public AugmentNode augmentNode;
    public PatternNode patternNode;
    public int healthAmount = 10;

    public float scaleMultiplier = 1.5f;
    public float transitionSpeed = 1f;
    public GameObject objectToScale;

    private Vector3 originalScale;
    public bool isMouseOver = false;
    bool mouseLock = false;

    Transform parent;
    public enum EventType
    {
        rest,
        agument,
        pattern
    }

    private void OnEnable()
    {
        int stageLevel = GameStateManager.Instance.stageLevel;
        isMouseOver = false;
        switch (eventType)
        {
            case EventType.agument:
                var augments = GameStateManager.Instance.aguments;
                if (augments.Count > 0)
                {
                    Augment augment = augments[Random.Range(0, augments.Count)];
                    currentAugment = augment;
                    name.text = currentAugment.name;
                    description.text = currentAugment.description;
                }
                else
                {
                    currentAugment = augmentInfinity;
                    // ����Ʈ�� ��� �ִ� ����� ó��
                }
                GameStateManager.Instance.aguments.Remove(currentAugment);
                GameStateManager.Instance.aguments.RemoveAll(item => item == null);
                break;
            case EventType.pattern:
                var patterns = GameStateManager.Instance.patterns;
                if (patterns.Count > 0)
                {
                    Pattern pattern = patterns[Random.Range(0, patterns.Count)];
                    currentPattern = pattern;
                    name.text = ConvertToLevelFormat(currentPattern.sprite.name);
                    description.text = "������ �ϼ��ϸ� "+  + pattern.damage + " �������� ������ " +pattern.level + " ������ ��ų�Դϴ�.";
                }
                GameStateManager.Instance.patterns.Remove(currentPattern);
                break;
            case EventType.rest:
                healthAmount = Random.Range(10 + stageLevel * 10 , Math.Min(20 + stageLevel * 15, 60) );
                name.text = (healthAmount > 40) ? "���� �޽�" : "�޽�";
                description.text = (healthAmount > 40) ? "���� ���� �޽��� ��ȸ�� ������ϴ�." : "���� �޽��� ��ȸ�� ������ϴ�. " + healthAmount + "�� ü���� ȸ���մϴ�.";
                break;
        }
    }
    
    public void SetUp()
    {
        switch (eventType)
        {
            case EventType.rest:
                image.sprite = restSprite;
                break;
            case EventType.agument:
                image.sprite = augmentSprite;
                break;
            case EventType.pattern:
                image.sprite = patternSprite;
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        SetUp();
        mapPlayerTracker = FindAnyObjectByType<MapPlayerTracker>();
        parent = transform.parent.parent;

        transform.parent.GetComponentsInChildren<EvenetStage>(otherEventStages);
        otherEventStages.Remove(this);

        button = GetComponent<Button>();
        button.onClick.AddListener(ActiveEvenet);
        augmentNode = FindAnyObjectByType<AugmentNode>();
        patternNode = FindAnyObjectByType<PatternNode>();
        originalScale = button.transform.localScale;
        button.onClick.AddListener(OnClick);
    }
    
    public void ActiveEvenet()
    {
        switch (eventType)
        {
            case EventType.rest:
                Rest(healthAmount);
                break;
            case EventType.agument:
                SelectAugment();
                break;
            case EventType.pattern:
                SelectPattern();
                break;
        }
    }
    public void SelectAugment()
    {
        augmentNode.GetNewAugment(currentAugment);

    }
    public void SelectPattern()
    {
        patternNode.GetNewPattern(currentPattern);

    }
    public void Rest(int _healthAmount)
    {
        GameStateManager.Instance.health += _healthAmount;
        
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetScale = isMouseOver ? originalScale * scaleMultiplier : originalScale;
        button.transform.localScale = Vector3.Lerp(button.transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        if(!mouseLock)
        isMouseOver = true;
    }

    public void OnPointerExit(BaseEventData eventData)
    {
        if (!mouseLock)
            isMouseOver = false;
    }
    string ConvertToLevelFormat(string input)
    {
        // ���� ǥ������ ����Ͽ� ���ڸ� ������ �κа� ���� �κ��� ã���ϴ�.
        var match = Regex.Match(input, @"(\D+)(\d+)");

        if (match.Success)
        {
            // �׷� 1�� ���ڸ� ������ �κ�, �׷� 2�� ���� �κ��Դϴ�.
            string prefix = match.Groups[1].Value;
            string number = match.Groups[2].Value;
            return $"{prefix} Lv.{number}";
        }

        // ��ġ�Ǵ� ������ ������ ���� ���ڿ��� ��ȯ�մϴ�.
        return input;
    }

    void OnClick()
    {
        StartCoroutine(ScaleAndDisable(3f));
    }

    IEnumerator ScaleAndDisable(float delay)
    {

        if (!image.isActiveAndEnabled) yield return null;
        button.enabled = false;
        mouseLock = true;
        var _image = parent.GetComponent<Image>();
        Color temp = image.color;
        Color black = Color.black;
        black.a = 0.98f;
        _image.color = black;

        //GetComponent<Image>().enabled = false;
        
        foreach(var a in otherEventStages)
        {
            if(eventType== EventType.agument)
            GameStateManager.Instance.aguments.Add(a.currentAugment);

            a.mouseLock = true;

            var i = a.GetComponent<Image>();
            i.enabled = false;
            var texts = a.GetComponentsInChildren<TextMeshProUGUI>();
            foreach(var t in texts)
            {
                t.enabled = false;
            }
          
            
        }

        yield return new WaitForSeconds(delay);
        if(eventType == EventType.rest) StartCoroutine(GameStateManager.Instance.UpdateHealthBar());
        image.color = temp;
        mouseLock = false;
        button.enabled = true;
        GetComponent<Image>().enabled = true;
        
        foreach (var a in otherEventStages)
        {
   

            a.mouseLock = false;
            var i = a.GetComponent<Image>();
            var texts = a.GetComponentsInChildren<TextMeshProUGUI>();
            i.enabled = true;
            foreach (var t in texts)
            {
                t.enabled = true;
            }


        }
        mapPlayerTracker.SelectUIOff();

    }
}

