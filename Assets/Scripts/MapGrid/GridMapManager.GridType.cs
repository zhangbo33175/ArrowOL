namespace GameLib
{
    public partial class GridMapManager
    {
        /// <summary>
        /// 地块状态
        /// </summary>
        public enum GridType
        {
            // 0: 空
            Empty=0,
            //障碍物
            Obstacle=1,
            //NPC
            NPC=2,
            //道具
            Props=3,
        }
        public enum LayerLevel
        {
            
            /// <summary>
            /// 地面
            /// </summary>
            ground=0,
            /// <summary>
            /// 建筑
            /// </summary>
            building=1,
            /// <summary>
            /// 装饰
            /// </summary>
            decoration=2
        }
    }
}