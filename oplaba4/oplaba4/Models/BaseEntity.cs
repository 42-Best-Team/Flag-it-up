using System;

public abstract class BaseEntity
{
    public string Id { get; }

    protected BaseEntity(string id)
    {
        Id = id;
    }
}
