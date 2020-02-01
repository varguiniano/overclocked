using UnityEngine;
using Varguiniano.Core.Runtime.Debug;

public class ConsoleOpener : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyUp(KeyCode.F12)) return;
        InGameConsole.Instance.Show = !InGameConsole.Instance.Show;
    }
}
