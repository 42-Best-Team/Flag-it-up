public class Player : BaseEntity
{
    public string Name { get; }

    public Player(string id, string name) : base(id)
    {
        Name = name;
    }
}

public class Admin : Player
{
    public Admin(string id, string name) : base(id, name) { }
}
