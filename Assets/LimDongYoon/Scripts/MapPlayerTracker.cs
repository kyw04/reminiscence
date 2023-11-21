using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public enum BattleResult { None, Win, Lose }
        public static BattleResult lastBattleResult = BattleResult.None;

        
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
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }
     
        private void ReturnFromBattle()
        {
            if (lastBattleResult == BattleResult.Lose)
            {
                // 패배 로직 처리
                // 예: 스테이지 재시도, 게임 오버 처리 등
            }
            else if (lastBattleResult == BattleResult.Win)
            {
                // 승리 로직 처리
                // 예: 다음 스테이지로 진행
            }
        }

        // 배틀 씬에서의 결과 설정
        public static void SetBattleResult(BattleResult result)
        {
            lastBattleResult = result;
        }


        private static void EnterNode(MapNode mapNode)
        {
            // 배틀 씬으로 전환하기 전에 결과 초기화
            lastBattleResult = BattleResult.None;
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // we have access to blueprint name here as well

            MySceneManager.Instance.LoadNextScene();
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                    break;
                case NodeType.EliteEnemy:
                    break;
                case NodeType.RestSite:
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Store:
                    break;
                case NodeType.Boss:
                    break;
                case NodeType.Mystery:
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