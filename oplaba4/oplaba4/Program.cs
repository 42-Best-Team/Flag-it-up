using System;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Repository<Player> playerRepository = new Repository<Player>();

        using (var flagUI = new FlagUI(playerRepository))
        {
            flagUI.ShowDialog();
        }

        using (var adminUI = new AdminUI(playerRepository))
        {
            adminUI.ShowDialog();
        }

        using (var playerUI = new PlayerUI(playerRepository))
        {
            playerUI.ShowDialog();
        }
    }
}
