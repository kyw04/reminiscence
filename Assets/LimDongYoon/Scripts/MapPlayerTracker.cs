using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;


namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;
        public MySceneManager sceneManager;

        public GameObject selectUI;

        public GameObject augmentUI;
        public GameObject restUI;
        public GameObject patternUI;

        

        public static MapPlayerTracker Instance;
        

        
        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);
          
            if (mapManager.CurrentMap.path.Count == 0)
            {
                
                Debug.Log("Selected node: " + mapNode.Node.point);

                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                Debug.Log("Selected node: " + mapNode.Node.point);
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            
            /*
            switch (mapNode.Node.nodeType)
            {
                case NodeType.Boss:
                case NodeType.MinorEnemy:
                case NodeType.EliteEnemy:
                    break;
                default:
                    mapManager.SaveMap();
                    break;
            }
            */
            mapManager.SaveMap();

            view.SetAttainableNodes();
            view.SetLineColors();
            //mapNode.ShowSwirlAnimation();
            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
            
        }
        
        private void SendPlayerToNode(MapNode mapNode,bool saveMode)
        {
            
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();
            
        }
     
     

       
        private void Start()
        {
            CheckBattleResult();
        }

        private void CheckBattleResult()
        {
            BattleResult battleResult = GameStateManager.Instance.GetBattleResult();
            switch (battleResult)
            {
                case BattleResult.Win:
                    Debug.Log("Win");
                    Debug.Log(GameStateManager.Instance.point);
                    GameStateManager.Instance.HealthImageUpdate();
                    GameStateManager.Instance.mapNode = view.GetNode(GameStateManager.Instance.point);
                    if (GameStateManager.Instance.currentBattlleInfo.isBoss)
                    {
                        mapManager.GenerateNewMap();
                        GameStateManager.Instance.stageLevel += 1;
                    }
                    break;
                case BattleResult.Lose:
                    Debug.Log("Lose");
                    GameStateManager.Instance.InitGame();
                 
                    break;
            }
            
        }

        private void EnterNode(MapNode mapNode)
        {
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // we have access to blueprint name here as well


            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            CurrentBattleEnemyInfo currentBattleEnemyInfo = new CurrentBattleEnemyInfo();

            switch (mapNode.Node.nodeType)
            {

                case NodeType.Boss:
                case NodeType.MinorEnemy:
                case NodeType.EliteEnemy:
                    if (MySceneManager.Instance.sceneLocked && lockAfterSelecting) Locked = false;
                    Debug.Log("zz");
                    UpdateBattleInfo(mapNode);
                    MySceneManager.Instance.LoadElmentalType(mapNode.nodeElementalType);
                    break;
                case NodeType.RestSite:
                    Debug.Log("휴식");
                    selectUI = restUI;
                    SelectUIOn();
                    break;
                case NodeType.Store:
                    Debug.Log("휴식");
                    selectUI = restUI;
                    SelectUIOn();
                    break;
                case NodeType.Mystery:
                    selectUI = augmentUI;
                    Debug.Log("증강체");
                    SelectUIOn();
                    break;
                case NodeType.Treasure:
                    selectUI = patternUI;
                    SelectUIOn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
       
        public void UpdateBattleInfo(MapNode mapNode)
        {
            GameStateManager.Instance.currentBattlleInfo = new CurrentBattleEnemyInfo()
            {
                currentStageLevel = GameStateManager.Instance.stageLevel,
                nodeElementalType = mapNode.nodeElementalType,
                isBoss = (mapNode.Node.nodeType == NodeType.Boss) ? true : false
            };
        }
        
        public void Rest(int healthAmount)
        {
            GameStateManager.Instance.health += healthAmount;
            GameStateManager.Instance.HealthImageUpdate();
        }
        public void RestSelectUIOn()
        {
            restUI.SetActive(true);
            SelectUIOn();
        }
        public void PatternSelectUIOn()
        {
            patternUI.SetActive(true);
            SelectUIOn();
        }
        public void AugmentSelectUIOn()
        {
            augmentUI.SetActive(true);
            SelectUIOn();
        }

        public void SelectUIOn()
        {
            FindObjectOfType<ScrollNonUI>().freezeX = true;
            selectUI.SetActive(true);
        }

        public void SelectUIOff()
        {
            Locked = false;
            FindObjectOfType<ScrollNonUI>().freezeX = false;
            selectUI.SetActive(false);
        }

        public void GetSelectItem()
        {
            
        }
        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}
