using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EvenetStage : MonoBehaviour
{
    public Button btn;
    public EventType eventType;

    public int healthAmount = 10;
    public enum EventType
    {
        rest,
        agument,
        pattern
    }
    
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ActiveEvenet);
    }
    
    public void ActiveEvenet()
    {
        switch (eventType)
        {
            case EventType.rest:
                Rest(healthAmount);
                break;
            case EventType.agument:
                break;
            case EventType.pattern:
                break;
        }
    }
    public void Rest(int _healthAmount)
    {
        GameStateManager.Instance.health += _healthAmount;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

