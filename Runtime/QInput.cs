
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QTool.InputSystem
{
    public class QInput
    {
//        public static bool MouseControl
//        {
//            get
//            {
//                return InputCore.DeviceType== InputDeviceType.键鼠;
//            }
//        }
//        static Vector2 _mousePosition;
//        static Vector2 _lastMousePosition;
//        public static Vector2 MousePosition
//        {
//            get
//            {
                
//                if (MouseControl)
//                {
//                    _lastMousePosition = InputCore.MousePosition;
//                    _mousePosition = _lastMousePosition;
//                }
//                else
//                {
//                    if (_lastMousePosition != InputCore.MousePosition)
//                    {
//                        InputCore.DeviceType = InputDeviceType.键鼠;
//                    }
//                }
//                return _mousePosition;
//            }
//            set
//            {
//                if (value == _mousePosition)
//                {
//                    return;
//                }
//                _mousePosition = value;
//                if (MouseControl)
//                {
//                    InputCore.DeviceType = InputDeviceType.未知;
//                }
              
//            }
//        }
//        public static bool BlockInput = false;
//        public static string InputTag { get
//            {
//                if (TagList.Count == 0)
//                {
//                    return "";
//                }
//                return TagList.First.Value; } }
//        private static LinkedList<string> TagList = new LinkedList<string>();
//        public static void PushTag(string tag)
//        {
//            if (string.IsNullOrWhiteSpace(tag))
//            {
//                return;
//            }
//            RemoveTag(tag);
//            TagList.AddFirst(tag);
//        }
//        public static void PushTag(System.Enum tag)
//        {
//            PushTag(tag.ToString());
//        }
//        public static void RemoveTag(string tag)
//        {
//            if (TagList.Contains(tag))
//            {
//                TagList.Remove(tag);
//            }
//        }
//        public static void RemoveTag(System.Enum tag)
//        {
//            RemoveTag(tag.ToString());
//        }
//        public static void ClearTag()
//        {
//            TagList.Clear();
//        }
//        public QList<string,InputAction> actionList=new QList<string, InputAction>();
//        private static QInput instance;
//        public static bool PosIsUI
//        {
//            get
//            {
//                return MouseControl&&EventSystem.current.IsPointerOverGameObject();
//            }
//        }
//        private QInput()
//        {
//            LoadPlayerPrefs();
//        }
//      //  Dictionary<string, InputAction> tempDic = new Dictionary<string, InputAction>();
//        public static bool ContainsAction(string key)
//        {
//            return instance.actionList.ContainsKey(key);
//        }
//        public static QInput Actions
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new QInput();
//                }
//                return instance;
//            }
//        }
//        public InputAction this[string key]
//        {
//            get
//            {
//                if (actionList.ContainsKey(key))
//                {
//                    return actionList[key];
//                }
//                else
//                {
//                    throw new System.Exception("缺少输入设置[" + key + "]");
//                }
               
//            }
//        }
//        public InputAction this[System.Enum keyEnum]
//        {
//            get
//            {
//                return this[keyEnum.ToString()];
//            }
//        }
//        public const string defaultSaveKey = "PlayerInputSetting";
//        public string DefaltSavePath
//        {
//            get
//            {
//                return Application.dataPath + "/Resources/" + defaultSaveKey+".xml";
//            }
//        }
//        public void SaveDefaultSetting()
//        {
//            FileManager.Save(DefaltSavePath, FileManager.Serialize(actionList));
//            Debug.Log("保存输入设置成功：" + DefaltSavePath);
//#if UNITY_EDITOR
//            UnityEditor.AssetDatabase.Refresh();
//#endif
//        }
//        public void LoadDefaultSetting()
//        {
//            var file = Resources.Load<TextAsset>(defaultSaveKey);
//            if (file != null)
//            {
//                actionList = FileManager.Deserialize<QList<string, InputAction>>(file.text);
//            }
//            else
//            {
//                Debug.LogError("不存在默认输入设置文件" + DefaltSavePath);
//            }
//        }
//        public void SavePlayerPrefs(string saveKey= defaultSaveKey)
//        {
//            PlayerPrefs.SetString(saveKey, FileManager.Serialize(actionList));
//        }
//        public void LoadPlayerPrefs(string saveKey = defaultSaveKey)
//        {
//            if (PlayerPrefs.HasKey(saveKey))
//            {
//                actionList = FileManager.Deserialize<QList<string, InputAction>>(PlayerPrefs.GetString(saveKey));
//            }
//            else
//            {
//                LoadDefaultSetting();
//            }
//            Debug.Log("加载默认输入设置[" + actionList.Count + "]");
//        }
    }
}