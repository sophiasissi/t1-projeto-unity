public static class GameSession
{
    public static int totalScore = 0;
    public static int totalCoffee = 0;

    public static int gameOverScore = 0;
    public static int gameOverCoffee = 0;

    public static string lastSceneName = "";

    public static void AddLevelResult(int score, int coffee)
    {
        totalScore += score;
        totalCoffee += coffee;
    }

    public static void SaveGameOverState(int score, int coffee, string sceneName)
    {
        gameOverScore = score;
        gameOverCoffee = coffee;
        lastSceneName = sceneName;
    }

    public static void Reset()
    {
        totalScore = 0;
        totalCoffee = 0;
        gameOverScore = 0;
        gameOverCoffee = 0;
        lastSceneName = "";
    }
}