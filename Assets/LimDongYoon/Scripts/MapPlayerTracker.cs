using System;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;
        public MySceneManager sceneManager;

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
            if (mapNode.Node.nodeType == NodeType.Boss)
            {
                mapManager.GenerateNewMap();
                mapManager.SaveMap();
            }
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
                    Debug.Log(GameStateManager.Instance.point);
                    GameStateManager.Instance.mapNode = view.GetNode(GameStateManager.Instance.point);
                    SelectNode(view.GetNode(GameStateManager.Instance.point));
                     mapManager.SaveMap();
                    
                    
                    break;
                case BattleResult.Lose:
                    Debug.Log("False");
                    break;
            }
            
        }

        private static void EnterNode(MapNode mapNode)
        {
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // we have access to blueprint name here as well

            
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            switch (mapNode.Node.nodeType)
            {
                case NodeType.Boss:
                case NodeType.MinorEnemy:
                case NodeType.EliteEnemy:
                    Debug.Log("zz");
                    DontDestroyOnLoad(mapNode.gameObject);
                    GameStateManager.Instance.point = mapNode.Node.point;//new Point(x: mapNode.Node.point.x, y:mapNode.Node.point.y);
                    MySceneManager.Instance.LoadNextScene();
                    break;
                case NodeType.RestSite:
                    
                    Debug.Log("휴식");
                    break;
                
                case NodeType.Store:
                    Debug.Log("마법");
                    break;
                case NodeType.Mystery:
                    Debug.Log("증강체");
                    break;
                
                case NodeType.Treasure:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}