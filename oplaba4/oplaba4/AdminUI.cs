public partial class AdminUI : UserUI
{
    public AdminUI(Repository<Player> repository)
        : base(repository, adminMode: true) { }
}
