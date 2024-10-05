using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityModManagerNet;

namespace BohSearch
{

    static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;

        // 默认快捷键：F1
        private static Key toggleUIKey = Key.F8;


        // UI 显示开关
        private static bool showUI = false;
        private static GameObject canvasObj; // Canvas 对象引用

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

            mod.OnToggle = OnToggle;
            mod.OnUpdate = OnUpdate;
            return true;
        }

        // 启用或禁用 Mod 时调用
        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            if (enabled)
            {
                CreateUI(); // 创建 UI 组件
            }
            else
            {
                DestroyUI(); // 删除 UI 组件
            }
            return true;
        }

        // 每帧更新
        private static void OnUpdate(UnityModManager.ModEntry modEntry, float deltaTime)
        {
            if (enabled && Keyboard.current[toggleUIKey].wasPressedThisFrame)
            {
                showUI = !showUI; // 切换 UI 显示状态
                if (canvasObj != null)
                {
                    canvasObj.SetActive(showUI);
                }
            }
        }


        // 创建自定义 UI 组件
        private static void CreateUI()
        {
            // 创建 Canvas
            canvasObj = new GameObject("MyModCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; // 设置为屏幕 UI
            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.AddComponent<GraphicRaycaster>();

            // 创建 Panel（UI 面板背景）
            GameObject panelObj = new GameObject("Panel");
            panelObj.transform.SetParent(canvasObj.transform);
            RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 300); // 设置 Panel 大小
            rectTransform.localPosition = Vector3.zero;
            Image panelImage = panelObj.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.7f); // 设置背景颜色

            // 创建 Button（关闭按钮）
            GameObject buttonObj = new GameObject("CloseButton");
            buttonObj.transform.SetParent(panelObj.transform);
            RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(100, 50); // 设置按钮大小
            buttonRect.localPosition = new Vector3(0, -100, 0); // 设置按钮位置
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = Color.white;
            Button button = buttonObj.AddComponent<Button>();

            // 添加 Button 交互事件
            button.onClick.AddListener(() =>
            {
                showUI = false;
                canvasObj.SetActive(false);
            });

            // 创建 Text（按钮文字）
            GameObject textObj = new GameObject("ButtonText");
            textObj.transform.SetParent(buttonObj.transform);
            Text text = textObj.AddComponent<Text>();
            text.text = "Close";
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.black;
            text.fontSize = 20;
            text.rectTransform.sizeDelta = buttonRect.sizeDelta; // 设置 Text 的大小与按钮一致

            // 设置 Canvas 的 UI 初始状态为隐藏
            canvasObj.SetActive(false);
        }

        // 删除自定义 UI 组件
        private static void DestroyUI()
        {
            if (canvasObj != null)
            {
                GameObject.Destroy(canvasObj);
            }
        }
    }
}
