[System.Serializable]
public class LevelSaveData
{
    public string levelName;
    public LevelStats stats;
    
    public LevelSaveData(string levelName, LevelStats stats)
    {
        this.levelName = levelName;
        this.stats = stats;
    }
}