using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Map;

public class EvenetStage : MonoBehaviour
{
    MapPlayerTracker mapPlayerTracker;
    public List<EvenetStage> otherEventStages;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public Augment currentAugment;
    public Augment augmentInfinity;
    public Button button;
    public EventType eventType;
    public AugmentNode augmentNode;
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
        isMouseOver = false;
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
            // 리스트가 비어 있는 경우의 처리
        }
        GameStateManager.Instance.aguments.Remove(currentAugment);
    }
    // Start is called before the first frame update
    void Start()
    {
        mapPlayerTracker = FindAnyObjectByType<MapPlayerTracker>();
        parent = transform.parent.parent;

        transform.parent.GetComponentsInChildren<EvenetStage>(otherEventStages);
        otherEventStages.Remove(this);

        button = GetComponent<Button>();
        button.onClick.AddListener(ActiveEvenet);
        augmentNode = FindAnyObjectByType<AugmentNode>();
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
                break;
        }
    }
    public void SelectAugment()
    {
        var augments = GameStateManager.Instance.aguments;
        if (augments.Count > 0)
        {
            Augment augment = currentAugment;
            augmentNode.GetNewAugment(augment);
        }
        else
        {
            augmentNode.GetNewAugment(augmentInfinity);
            // 리스트가 비어 있는 경우의 처리
        }

    }
    public void SelectPattern()
    {
        var augments = GameStateManager.Instance.aguments;
        int ranNum = Random.Range(0, augments.Count);
        Augment augment = augments[ranNum];
        Debug.Log("삭제전 " +augments.Count);
        GameStateManager.Instance.aguments.RemoveAt(ranNum);
        Debug.Log("삭제후 " + augments.Count + " " + GameStateManager.Instance.aguments.Count);
        augmentNode.GetNewAugment(augment);

    }
    public void Rest(int _healthAmount)
    {
        GameStateManager.Instance.health += _healthAmount;
        GameStateManager.Instance.HealthImageUpdate();
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

    void OnClick()
    {
        StartCoroutine(ScaleAndDisable(3f));
    }

    IEnumerator ScaleAndDisable(float delay)
    {
        mouseLock = true;
        var image = parent.GetComponent<Image>();
        Color temp = image.color;
        Color black = Color.black;
        black.a = 0.98f;
        image.color = black;

        GetComponent<Image>().enabled = false;
        foreach(var a in otherEventStages)
        {
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
        image.color = temp;
        mouseLock = false;

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

