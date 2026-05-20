public static class GameSession
{
    public static int totalScore = 0;
    public static int totalCoffee = 0;

    public static void Reset()
    {
        totalScore = 0;
        totalCoffee = 0;
    }

    public static void AddLevelResult(int score, int coffee)
    {
        totalScore += score;
        totalCoffee += coffee;
    }
}