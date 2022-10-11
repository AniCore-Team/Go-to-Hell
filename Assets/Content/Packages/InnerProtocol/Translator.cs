using System;
using System.Collections.Generic;

public delegate ISendData CustomAnswerEvent(Enum codeEvent, ISendData data);
public delegate void CustomEvent(Enum codeEvent, ISendData data);
public delegate void CustomSignal(Enum codeEvent);

public interface ISendData { }

public class Translator
{
    private static Dictionary<Type, List<CustomAnswerEvent>> _answerEventProtocols = new Dictionary<Type, List<CustomAnswerEvent>>();
    private static Dictionary<Type, CustomEvent> _eventProtocols = new Dictionary<Type, CustomEvent>();
    private static Dictionary<Type, CustomSignal> _signalProtocols = new Dictionary<Type, CustomSignal>();

    #region "Remove"
    public static void Remove<T>(Delegate d)
    {
        switch (d)
        {
            case CustomSignal listener:
                RemoveListener<T>(listener);
                break;
            case CustomEvent listener:
                RemoveListener<T>(listener);
                break;
            case CustomAnswerEvent listener:
                RemoveListener<T>(listener);
                break;
        }
    }

    public static void RemoveAll()
    {
        _answerEventProtocols.Clear();
        _eventProtocols.Clear();
        _signalProtocols.Clear();
    }

    public static void RemoveAll<T>()
    {
        if (_answerEventProtocols.ContainsKey(typeof(T)))
            _answerEventProtocols[typeof(T)] = null;
        if (_eventProtocols.ContainsKey(typeof(T)))
            _eventProtocols[typeof(T)] = null;
        if (_signalProtocols.ContainsKey(typeof(T)))
            _signalProtocols[typeof(T)] = null;
    }

    private static void RemoveListener<T>(CustomEvent listener)
    {
        if (listener != null && _eventProtocols.ContainsKey(typeof(T)))
            _eventProtocols[typeof(T)] -= listener;
    }

    private static void RemoveListener<T>(CustomSignal listener)
    {
        if (listener != null && _signalProtocols.ContainsKey(typeof(T)))
            _signalProtocols[typeof(T)] -= listener;
    }

    private static void RemoveListener<T>(CustomAnswerEvent listener)
    {
        if (listener != null && _answerEventProtocols.ContainsKey(typeof(T)))
            _answerEventProtocols[typeof(T)].Remove(listener);
    }
    #endregion "Remove"

    #region "Add"
    public static void Add<T>(Delegate d)
    {
        switch (d)
        {
            case CustomSignal listener:
                AddListener<T>(listener);
                break;
            case CustomEvent listener:
                AddListener<T>(listener);
                break;
            case CustomAnswerEvent listener:
                AddListener<T>(listener);
                break;
        }
    }

    private static void AddListener<T>(CustomEvent listener)
    {
        if (listener != null)
        {
            if (!_eventProtocols.ContainsKey(typeof(T)))
                _eventProtocols[typeof(T)] = listener;
            else
                _eventProtocols[typeof(T)] += listener;
        }
    }

    private static void AddListener<T>(CustomSignal listener)
    {
        if (listener != null)
        {
            if (!_signalProtocols.ContainsKey(typeof(T)))
                _signalProtocols[typeof(T)] = listener;
            else
                _signalProtocols[typeof(T)] += listener;
        }
    }

    private static void AddListener<T>(CustomAnswerEvent listener)
    {
        if (listener != null)
        {
            if (!_answerEventProtocols.ContainsKey(typeof(T)))
                _answerEventProtocols[typeof(T)] = new List<CustomAnswerEvent>();

            _answerEventProtocols[typeof(T)].Add(listener);
        }
    }
    #endregion "Add"

    #region "Send"

    public static void Send<T, V>(T codeEvent, V data = default) where T : Enum
                                                                 where V : ISendData
    {
        if (data == null)
            SendSignal<T>(codeEvent);
        else
            SendEvent<T>(codeEvent, data);

        //UnityEngine.Debug.LogError(codeEvent);
    }
    
    public static void Send<T>(T codeEvent) where T : Enum
    {
        Send<T, ISendData>(codeEvent);

        //UnityEngine.Debug.LogError(codeEvent);
    }

    public static N[] SendAnswers<T, V, N>(T codeEvent, V data) where T : Enum
                                                                where V : ISendData
                                                                where N : ISendData
    {
        return SendAnswerEvent<T, V, N>(codeEvent, data);
    }

    public static N SendOneAnswer<T, V, N>(T codeEvent, V data = default) where T : Enum
                                                                          where V : ISendData
                                                                          where N : ISendData
    {
        N[] obj = SendAnswerEvent<T, V, N>(codeEvent, data);
        if (obj.Length <= 0)
            return default;
        
        foreach (var o in obj)
            if (o is N r)
                return r;
        
        return default;

    }

    private static N[] SendAnswerEvent<T, V, N>(T codeEvent, V data) where T : Enum
                                                                     where V : ISendData
                                                                     where N : ISendData
    {
        List<N> obj = new List<N>();
        if (_answerEventProtocols.ContainsKey(typeof(T)))
        {
            foreach (var ev in _answerEventProtocols[typeof(T)])
            {
                var r = ev?.Invoke(codeEvent, data);
                if (r is N e)
                    obj.Add(e);
            }
        }
        return obj.ToArray();
    }

    private static void SendEvent<T>(Enum codeEvent, ISendData data)
    {
        if (_eventProtocols.ContainsKey(typeof(T)))
        {
            _eventProtocols[typeof(T)]?.Invoke(codeEvent, data);
        }
    }

    private static void SendSignal<T>(Enum codeSignal)
    {
        if (_signalProtocols.ContainsKey(typeof(T)))
        {
            _signalProtocols[typeof(T)]?.Invoke(codeSignal);
        }
    }

    #endregion "Send"
}
