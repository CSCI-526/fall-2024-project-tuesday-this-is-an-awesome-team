using System.Collections.Generic;
using UnityEngine;

public class HelpContentManager : MonoBehaviour
{
    public static HelpContentManager Instance;

    private void Awake()
    {
        Instance = this;
        InitializeLevelHelpContent();
    }

    [System.Serializable]
    public class HelpRule
    {
        public Sprite icon;
        public string text;
    }

    [System.Serializable]
    public class LevelHelpData
    {
        public List<HelpRule> rules = new List<HelpRule>();
    }

    public Dictionary<string, LevelHelpData> levelHelpContent;

    private void InitializeLevelHelpContent()
    {
        levelHelpContent = new Dictionary<string, LevelHelpData>()
        {
            {
                "Level 1",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "stick" }
                    }
                }
            },
            {
                "Level 2",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "stick/grab" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "fall" }
                    }
                }
            },
            {
                "Level 3",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "stick/grab" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "fall" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "switch map view" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "flying enemy-move" }
                    }
                }
            },
            {
                "Level 4",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "stick/grab" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "fall" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "switch map view" },
                        new HelpRule { icon = Instance.keyboardSpaceIcon, text = "shooting enemy-shoot" }
                    }
                }
            },
            {
                "Level 5",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "stick/grab" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "throw" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "switch map view" },
                        new HelpRule { icon = Instance.checkPointIcon, text = "Save level progress" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "fall/jumping enemy-jump" }
                    }
                }
            },
            {
                "Level 6",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "stick/grab" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "throw" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "switch map view" },
                        new HelpRule { icon = Instance.checkPointIcon, text = "Save level progress" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "fall/jumping enemy-jump" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "flying enemy-move" },
                        new HelpRule { icon = Instance.keyboardSpaceIcon, text = "shooting enemy-shoot" },
                    }
                }
            },
            {
                "Level 7",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "stick/grab" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "throw" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "switch map view" },
                        new HelpRule { icon = Instance.checkPointIcon, text = "Save level progress" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "fall/jumping enemy-jump" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "flying enemy-move" },
                        new HelpRule { icon = Instance.keyboardSpaceIcon, text = "shooting enemy-shoot" },
                    }
                }
            }
        };
    }
    public Sprite mouseLeftIcon;
    public Sprite mouseRightIcon;
    public Sprite keyboardEIcon;
    public Sprite keyboardMIcon;
    public Sprite keyboardArrowKeysIcon;
    public Sprite keyboardSpaceIcon;
    public Sprite checkPointIcon;
}

