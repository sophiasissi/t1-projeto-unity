public static class GameSession
{
    public static int totalScore = 0;
    public static int totalCoffee = 0;

    public static int lastRunScore = 0;
    public static int lastRunCoffee = 0;
    public static string lastPlayedScene = "";

    public static void Reset()
    {
        totalScore = 0;
        totalCoffee = 0;

        lastRunScore = 0;
        lastRunCoffee = 0;
        lastPlayedScene = "";
    }

    public static void AddLevelResult(int score, int coffee)
    {
        totalScore += score;
        totalCoffee += coffee;
    }

    public static void SaveGameOverState(int score, int coffee, string sceneName)
    {
        lastRunScore = totalScore + score;
        lastRunCoffee = totalCoffee + coffee;
        lastPlayedScene = sceneName;
    }
}