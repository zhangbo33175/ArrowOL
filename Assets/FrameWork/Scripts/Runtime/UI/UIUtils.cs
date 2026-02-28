using UnityEngine.UI;

namespace Honor.Runtime
{
    public static class UIUtils
    {

        public static UIInfo GetUIInfo(UIType uiType,string _ABPath,string _AssetName)
        {
            UIInfo uiInfo = new UIInfo()
            {
                UIType = uiType,
                ABPath = _ABPath,
                AssetName = _AssetName,
                IsModal = false,
                ZOrder = GameConstants.ProcedureTransitionUIZOrder,
                Priority = 0,
                CloseOnEscapeKeyUp = false,
                BlockingMask = "Everything",
                BlockingObjects = GraphicRaycaster.BlockingObjects.None,
                MultiTypeTextCompsCoexist = false,
                LuaParams = null,
                OverCallback = null,
            };

            return uiInfo;
        }

    }
}