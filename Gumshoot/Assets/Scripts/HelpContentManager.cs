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
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "Stick" }
                    }
                }
            },
            {
                "Level 2",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "Stick" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "Throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "Jump" }
                    }
                }
            },
            {
                "Level 3",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "Stick" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "Throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "Jump" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "Zoom in/out" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "Flying Ability - Move" }
                    }
                }
            },
            {
                "Level 4",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "Stick" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "Throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "Jump" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "Zoom in/out" },
                        new HelpRule { icon = Instance.checkPointIcon, text = "Checkpoint" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "Flying Ability - Move" },
                        new HelpRule { icon = Instance.keyboardSpaceIcon, text = "Shooting Ability - Shoot (destroys blue walls and hurts enemies)" },
                    }
                }
            },
            {
                "Level 5",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "Stick" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "Throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "Jump / Jumping Ability - Jump" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "Zoom in/out" },
                        new HelpRule { icon = Instance.checkPointIcon, text = "Checkpoint" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "Flying Ability - Move" },
                        new HelpRule { icon = Instance.keyboardSpaceIcon, text = "Shooting Ability - Shoot (destroys blue walls and hurts enemies)" },
                    }
                }
            },
            {
                "Level 6",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "Stick" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "Throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "Jump / Jumping Ability - Jump" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "Zoom in/out" },
                        new HelpRule { icon = Instance.checkPointIcon, text = "Checkpoint" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "Flying Ability - Move" },
                        new HelpRule { icon = Instance.keyboardSpaceIcon, text = "Shooting Ability - Shoot (destroys blue walls and hurts enemies)" },
                    }
                }
            },
            {
                "Level 7",
                new LevelHelpData
                {
                    rules = new List<HelpRule>
                    {
                        new HelpRule { icon = Instance.mouseLeftIcon, text = "Stick" },
                        new HelpRule { icon = Instance.mouseRightIcon, text = "Throw" },
                        new HelpRule { icon = Instance.keyboardEIcon, text = "Jump / Jumping Ability - Jump" },
                        new HelpRule { icon = Instance.keyboardMIcon, text = "Zoom in/out" },
                        new HelpRule { icon = Instance.checkPointIcon, text = "Checkpoint" },
                        new HelpRule { icon = Instance.keyboardArrowKeysIcon, text = "Flying Ability - Move" },
                        new HelpRule { icon = Instance.keyboardSpaceIcon, text = "Shooting Ability - Shoot (destroys blue walls and hurts enemies)" },
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

