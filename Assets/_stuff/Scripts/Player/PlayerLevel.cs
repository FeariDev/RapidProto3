using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] int level;
    [SerializeField] float experience;
    public int Level
    {
        get
        {
            return level;
        }
        private set
        {
            level = value;
        }
    }
    public float Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
        }
    }
    [ReadOnly] public float levelUpExpRequirement;
    [ReadOnly] public int levelOverflow;

    [Header("Properties")]
    [SerializeField] float levelOverflowMultiplier = 2f;
    [SerializeField] float[] levelUpExpRequirements;

    public event Action OnLevelUp;
    public event Action OnUpdateExperience;


    
    public void CheckLevel()
    {
        CheckExperience();

        if (levelUpExpRequirement <= Experience)
        {
            int newLevel = Level + 1;
            Level = newLevel;
            OnLevelUp?.Invoke();
        }
    }

    public void CheckExperience()
    {
        levelOverflow = Level - levelUpExpRequirements.Length;
        levelUpExpRequirement = levelOverflow < 0 ? levelUpExpRequirements[Level] : levelUpExpRequirements[levelUpExpRequirements.Length - 1] * (levelOverflow + levelOverflowMultiplier);
    }

    public void UpdateExperience(float value)
    {
        Experience += value;

        OnUpdateExperience?.Invoke();
    }



    public float debugExperience;
    [Button]
    void DebugAddExperience()
    {
        UpdateExperience(debugExperience);
    }



    #region Unity lifecycle

    void Awake()
    {
        OnLevelUp += CheckExperience;
        OnUpdateExperience += CheckLevel;
    }

    #endregion
}
