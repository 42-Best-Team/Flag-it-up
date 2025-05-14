namespace FlagItUpApp.Models
{
    public class Game
    {
        public string Mode { get; set; }  // ����� ��� (���������, Europe, USA)
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int CurrentFlagId { get; set; }
        public List<int> UsedFlagIds { get; set; }  // ������ ������������ �������

        public Game()
        {
            UsedFlagIds = new List<int>();
        }
    }
}
