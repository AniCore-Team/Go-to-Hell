using System;
using UnityEngine;

public struct FloatData : ISendData
{
    public float value;
}

public struct DoubleData : ISendData
{
    public double value;
}

public struct IntData : ISendData
{
    public int value;
}

public struct BoolData : ISendData
{
    public bool value;
}

public struct ObjectData : ISendData
{
    public object value;
}

public struct EnumData : ISendData
{
    public Enum value;
}

public struct TransformData : ISendData
{
    public Transform value;
}