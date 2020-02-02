using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Gui;

namespace Gui
{
    public class ExitGameOnBtnClick : ActionOnButtonClick
    {
        protected override void ButtonClicked()
        {
            Utils.CloseGame();
        }
    }
}