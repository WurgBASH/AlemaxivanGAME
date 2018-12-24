using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class InputFunctions : MonoBehaviour {

    public InputField input;
    private string Text;
    private void Awake()
    {
        input= FindObjectOfType<InputField>();
    }
    void Start()
    {
        input.onEndEdit.AddListener(GetFuncs);

    }

    private void GetFuncs(string arg)
    {
        Text = arg;
        Debug.Log(arg);

        Player player = FindObjectOfType<Player>();
        MethodInfo m = player.GetType().GetMethod(Text);
        m.Invoke(player, null);
    }
}
