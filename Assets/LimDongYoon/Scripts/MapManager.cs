using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        public MapConfig config;
        public MapView view;
        
        public Map CurrentMap { get; private set; }

        private void Start()
        {
            if (PlayerPrefs.HasKey("Map"))
            {
                var mapJson = PlayerPrefs.GetString("Map");
                var map = JsonConvert.DeserializeObject<Map>(mapJson);
                // using this instead of .Contains()
                if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
                {
                    // payer has already reached the boss, generate a new map
                    //NextStage();
                    GenerateNewMap();
                }
                else
                {
                    CurrentMap = map;
                    // player has not reached the boss yet, load the current map
                    view.ShowMap(map);
                }
            }
            else
            {
                GenerateNewMap();
            }
        }

        private void NextStage()
        {
            var mapJson = PlayerPrefs.GetString("Map");
            var map = JsonConvert.DeserializeObject<Map>(mapJson);
            map.level = CurrentMap.level + 1;
            CurrentMap = map;
            view.ShowMap(map);
        }

        public void GenerateNewMap()
        {
           
            var map = MapGenerator.GetMap(config);
            CurrentMap = map;
            Debug.Log(map.ToJson());
            view.ShowMap(map);
        }

       
        public void LoadLevelMap()
        {

            int stageLevel = ((int)FindObjectOfType<Slider>().value);
            if (PlayerPrefs.HasKey("Map"+stageLevel))
            {
                var mapJson = PlayerPrefs.GetString("Map" + stageLevel);
                var map = JsonConvert.DeserializeObject<Map>(mapJson);
                CurrentMap = map;
                view.ShowMap(map);
                Debug.Log("맵이 로드되었습니다. " + stageLevel + " 스테이지");
            }
            else 
            {
                Debug.Log("소환사님, 저장된 맵이 없습니다. " + stageLevel + " 스테이지");
            }
            
        }

        public void SaveLevelMap()
        {
            int stageLevel = ((int)FindObjectOfType<Slider>().value);
            if (CurrentMap == null) return;
            CurrentMap.level = stageLevel;
            var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            PlayerPrefs.SetString("Map"+ stageLevel, json);
            PlayerPrefs.Save();
            Debug.Log("맵이 세이브 되었습니다. " + stageLevel + " 스테이지" );
        }
        public void SaveMap()
        {
            if (CurrentMap == null) return;

            var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            SaveMap();
        }
    }
}
