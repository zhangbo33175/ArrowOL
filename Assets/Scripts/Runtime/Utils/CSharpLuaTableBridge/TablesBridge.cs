using System.Collections.Generic;

namespace GameLib
{
    public class TablesBridge
    {
        //=>映射捏脸点位表、服装表的数据结构
        #region TableAvatarCustomize、TableCustomize
        public class TableCustomizeItem
        {
            //=>捏脸点位ID
            public string ID;
            //=>点位类型
            public string Type;
            //=>皮肤插槽点位类型
            public string SlotName;
        }
        #endregion

        #region 映射捏脸点位类型表，主要用于控制
        public class TableAvatarCustomizeItem
        {
            //=>捏脸点位类型ID
            public string ID;
            //=>是否可参与换色
            public bool EnableColorChange;
            //=>这一组插槽使用下文的颜色
            public List<string> ColorSlotName;
            //=>可使用的颜色列表
            public List<TableAvatarCustomizeItemColor> ColorRGB;
            //=>参与随机的颜色ID组
            public List<int> RandomColorID;
        }

        public class TableAvatarCustomizeItemColor
        {
            public int ID;
            public string Color;
        }
        #endregion
    }
}