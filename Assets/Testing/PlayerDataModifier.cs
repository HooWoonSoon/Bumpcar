
public enum StataModType
{
    Flat,
    Percent,
}

public class PlayerDataModifier 
{
    public readonly float Value;
    public readonly StataModType Type;
    public readonly int Order;

    public PlayerDataModifier(float value, StataModType type, int order)
    {
        Value = value;
        Type = type;
        Order = order;
    }

    public PlayerDataModifier(float value, StataModType type) : this(value, type, (int)type) { }
}