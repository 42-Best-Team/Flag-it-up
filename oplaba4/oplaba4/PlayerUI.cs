public partial class PlayerUI : UserUI
{
    public PlayerUI(Repository<Player> repository)
        : base(repository, adminMode: false) { }
}
