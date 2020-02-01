using UnityEngine.SceneManagement;
using Varguiniano.Core.Runtime.Gui;

public class LoadMainSceneOnBtn : ActionOnButtonClick
{
    protected override void ButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}
