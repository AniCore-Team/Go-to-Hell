[System.Serializable]
public struct Optionals<T>
{
    private bool _isInit;
    private T _value;

    public bool isInit =>
        _isInit;
    public T Value =>
        _value;

    public Optionals(T value)
    {
        _value = value;
        _isInit = true;
    }
}
