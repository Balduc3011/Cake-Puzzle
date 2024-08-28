using System.Collections.Generic;
using JetBrains.Annotations;
using SDK;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace _BaseGame.ScriptableObjects.MapData
{
    [CreateAssetMenu(fileName = "MapCakeConfigs", menuName = "GlobalConfigs/MapCakeConfigs", order = 0)]
    [GlobalConfig("Assets/Resources/GlobalConfigs/")]
    public class MapCakeConfigs : GlobalConfig<MapCakeConfigs>
    {
        public string googleSheetURL;
        public string sheetName;
        public List<MapCakeConfigsData> mapCakeConfigs = new List<MapCakeConfigsData>();
        [OnValueChanged("UpdateSelectedMapCakeConfigsData")]
        public int SelectedLevel;
        [ShowIf("@SelectedLevel > 0 && SelectedMapCakeConfigsData != null")]
        public MapCakeConfigsData SelectedMapCakeConfigsData;
        
        private void UpdateSelectedMapCakeConfigsData()
        {
            SelectedMapCakeConfigsData = GetMapCakeConfigsData(SelectedLevel);
            SelectedMapCakeConfigsData.editTable = true;
        }
#if UNITY_EDITOR
        [Button]
        public void FetchDataFromGoogleSheet()
        {
            Static.GetCSVDataFromGoogleSheet(googleSheetURL,sheetName).ContinueWith(task =>
            {
                List<MapCakeConfigsData> newData = new List<MapCakeConfigsData>();
                foreach (var record in task.Result)
                {
                    MapCakeConfigsData mapCakeConfigsData = new MapCakeConfigsData();
                    mapCakeConfigsData.level = int.Parse(record["Level"]);
                    var oldData = GetMapCakeConfigsData(mapCakeConfigsData.level);
                    
                    mapCakeConfigsData.orderCakeTime = int.Parse(record["OrderCakeTime"]);
                    mapCakeConfigsData.minAmountPerOrder = int.Parse(record["MinAmountPerOrder"]);
                    mapCakeConfigsData.maxAmountPerOrder = int.Parse(record["MaxAmountPerOrder"]);
                    mapCakeConfigsData.cakeOrders.Clear();
                    if (oldData != null)
                    {
                        mapCakeConfigsData.board = oldData.board;
                    }
                    newData.Add(mapCakeConfigsData);
                }
                mapCakeConfigs = newData;
            });
        }
#endif
        public MapCakeConfigsData GetMapCakeConfigsData(float level)
        {
            int levelTemp = level > mapCakeConfigs.Count ? (int)(level % mapCakeConfigs.Count) : (int)level;
            //Debug.Log("Level "+ levelTemp+" " +level);
            if (levelTemp == 0)
                return mapCakeConfigs[mapCakeConfigs.Count - 1];
            return mapCakeConfigs.Find(m => m.level == levelTemp);
        }

#if UNITY_EDITOR
        [Button]
        public void GenerateCakeRequired()
        {
            foreach (MapCakeConfigsData t in mapCakeConfigs)
            {
                t.GenerateCakeRequired();
            }
        }
        
        [Button]
        public void SaveData()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
        public bool IsLastOrderOfLevel(int level, int currentOrderIndex)
        {
            //Debug.Log("Last order is: "+ GetMapCakeConfigsData(level).cakeOrders.Count / 3);
            //Debug.Log($"Current order index: {currentOrderIndex}");
            //Debug.Log("Is last order of level: " + (GetMapCakeConfigsData(level).cakeOrders.Count / 3 == currentOrderIndex));
            return (GetMapCakeConfigsData(level).cakeOrders.Count/3 == currentOrderIndex);
        }
    }
    
    [System.Serializable]
    public class MapCakeConfigsData
    {
        public int level;
        public int orderCakeTime;
        public int minAmountPerOrder;
        public int maxAmountPerOrder;
        public bool editTable;
        
        [field: SerializeField]public List<CakeOrder> cakeOrders = new List<CakeOrder>();
        [field: SerializeField] public BoardCake board = new BoardCake(5, 4);   
        
        public void GenerateCakeRequired()
        {
            cakeOrders.Clear();
            while (cakeOrders.Count<orderCakeTime)
            {
                int type = Random.Range(0, 6);
                int amount = Random.Range(minAmountPerOrder, maxAmountPerOrder+1);
                cakeOrders.Add(new CakeOrder {type = type, amount = amount});
            }
        }
        
        [ShowInInspector, DoNotDrawAsReference]
        [ShowIf("@editTable == true")]
        [TableMatrix(HorizontalTitle = "Board Cell", SquareCells = true, RowHeight = 30, Transpose = true, DrawElementMethod = "DrawColoredEnumElement")]
        public int[,] BoardTransposed {
            get
            {
                int[,] result = new int[board.row, board.col];
                for (int i = 0; i < board.row; i++)
                {
                    for (int j = 0; j < board.col; j++)
                    {
                        result[i, j] = board.Get(i, j).value;
                    }
                }
                return result;
                
            }
            set
            {
                for (int i = 0; i < board.row; i++)
                {
                    for (int j = 0; j < board.col; j++)
                    {
                        board.Set(i, j, value[i, j]);
                    }
                }
            }
        }

#if UNITY_EDITOR
        static int DrawColoredEnumElement(Rect rect, int value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                switch (Event.current.button)
                {
                    case 1:
                        value -= 1;
                        break;
                    case 0:
                        value += 1;
                        break;
                }

                if (value > 6) value = 0;
                if (value < 0) value = 6;
                GUI.changed = true;
                Event.current.Use();
            }

            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value > 0 ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));
            UnityEditor.EditorGUI.LabelField(rect, value.ToString(), new GUIStyle()
            {
                fontSize = 32,
                alignment = TextAnchor.MiddleCenter
            });

            return value;
        }
#endif


        [Button]
        [ShowIf("@editTable == true")]
        public void ClearBoard()
        {
            board.ClearBoard();
        }

        public CakeOrder GetCakeOrder(int currentOrderIndex, int slotOrderIndex) {
            return cakeOrders[currentOrderIndex * 3 + slotOrderIndex];
        }
        List<CakeOrder> cakeOrdersTemp = new();
        public List<CakeOrder> GetListCakeOrder(int currentOrderIndex) {
            cakeOrdersTemp.Clear();
            for (int i = currentOrderIndex * 3; i < (currentOrderIndex + 1) *3; i++)
            {
                cakeOrdersTemp.Add(cakeOrders[i]);
            }
            return cakeOrdersTemp;
        }
    }
    
    [System.Serializable]
    public class CakeOrder
    {
        public int type;
        public int amount;
    }

    [System.Serializable]
    public class BoardCake
    {
        [field: SerializeField]public List<BoardCakeData> Board { get; set; } = new List<BoardCakeData>();
        [field: SerializeField]public int row;
        [field: SerializeField]public int col;
        
        public BoardCake()
        {
            Board = new List<BoardCakeData>();
        }
        public BoardCake(int row, int col)
        {
            Init(row, col);
        }
        public void Init(int row, int col)
        {
            this.row = row;
            this.col = col;
            for (int i = 0; i < row; i++)
            {
                List<BoardCakeData> list = new List<BoardCakeData>();
                for (int j = 0; j < col; j++)
                {
                    list.Add(new BoardCakeData()
                    {
                        row = i,
                        col = j,
                        value = 0
                    });
                }
                Board.AddRange(list);
            }
        }

        public void ClearBoard()
        {
            foreach (BoardCakeData boardCakeData in Board)
            {
                boardCakeData.value = 0;
            }
        }
        public BoardCakeData Get(int row, int col)
        {
            return Board.Find(b => b.row == row && b.col == col);
        }
        public void Set(int row, int col, int value)
        {

            BoardCakeData boardCakeData = Board.Find(b => b.row == row && b.col == col);
            if (boardCakeData != null && boardCakeData.value != value)
            {
                Debug.Log("Set row " + row + " col " + col + " value " + value);
                boardCakeData.value = value;
            }
        }
    }
    [System.Serializable]
    public class BoardCakeData
    {
        public int row;
        public int col;
        public int value;
    }
}