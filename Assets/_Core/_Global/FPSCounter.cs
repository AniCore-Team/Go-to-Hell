using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]private bool toFPS = true;
    [SerializeField, Range(30, 60)] private int fps_count;
    
    private int _accumulator, _counter = 0;
    private float _timer = 0f;
    
    private readonly GUIStyle _style = new GUIStyle();
    
    private void Start()
    {
        Application.targetFrameRate = fps_count;

        if (toFPS == false)
            return;

        _style.normal.textColor = Color.cyan;
        _style.fontSize = 32;
        _style.fontStyle = FontStyle.Bold;
    }

    private void OnGUI()
    {
        if (toFPS == false) 
            return;

        GUI.Label(new Rect(10, 10, 100, 34), "FPS: " + _counter, _style);
    }

    private void Update()
    {
        if (toFPS == false)
            return;

        _accumulator++;
        _timer += Time.deltaTime;

        if (_timer >= 1)
        {
            _timer = 0;
            _counter = _accumulator;
            _accumulator = 0;
        }
    }

}
