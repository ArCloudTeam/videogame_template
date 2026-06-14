using UnityEngine;

public class GameUI : MonoBehaviour
{
    void OnGUI()
    {
        if (!GameManager.isGameOver) return;

        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

        string message = GameManager.isVictory ? "¡VICTORIA!" : "DERROTA";
        string subtext = "Presiona R para reiniciar";

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 48;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = GameManager.isVictory ? Color.green : Color.red;

        GUI.Label(new Rect(0, Screen.height / 2f - 60, Screen.width, 60), message, style);

        GUIStyle subStyle = new GUIStyle(GUI.skin.label);
        subStyle.fontSize = 24;
        subStyle.alignment = TextAnchor.MiddleCenter;
        subStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(0, Screen.height / 2f + 10, Screen.width, 40), subtext, subStyle);

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R)
        {
            GameManager.Restart();
            Event.current.Use();
        }
    }
}
