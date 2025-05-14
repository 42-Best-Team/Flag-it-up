namespace FlagItUpApp.Models
{
    public class Game
    {
        public string Mode { get; set; }  // Режим гри (наприклад, Europe, USA)
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int CurrentFlagId { get; set; }
        public List<int> UsedFlagIds { get; set; }  // Список використаних прапорів

        public Game()
        {
            UsedFlagIds = new List<int>();
        }
    }
}
