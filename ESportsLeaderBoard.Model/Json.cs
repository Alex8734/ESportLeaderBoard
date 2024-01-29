namespace ESportsLeaderBoard.Model;

public class JsonSingleOutput<T>
{
    public JsonSingleOutput(T value)
    {
        Value = value;
    }
    public T Value { get; set; }
}  