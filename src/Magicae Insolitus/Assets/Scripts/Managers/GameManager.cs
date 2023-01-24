using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class GameManager : InstancedBehavior<GameManager>
{
    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD
    }

    [Serializable]
    public class DifficultySetting
    {
        public Difficulty difficulty;
        public int minRooms => maxRooms / 2;
        public int maxRooms;
    }

    [SerializeField] private List<DifficultySetting> _difficultySettings = new List<DifficultySetting>();
    public Difficulty difficulty;

    public Tuple<int, int> GetDifficultySettings()
    {
        var diff = _difficultySettings.FirstOrDefault(x => x.difficulty == difficulty);
        
        return new Tuple<int, int>(diff.minRooms, diff.maxRooms);
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
        base.Awake();
    }
}
