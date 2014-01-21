namespace RealTimeGraph
{
    /// <summary>可设定的曲线显示模式枚举类型
    /// </summary>
    public enum GraphMode
    {
        /// <summary>全局实时显示模式
        /// </summary>
        GlobalMode,
        /// <summary>固定坐标尺度的滚动实时显示模式
        /// </summary>
        FixMoveMode,
        /// <summary>框选放大模式
        /// </summary>
        RectZoomInMode,
        /// <summary>拖动模式
        /// </summary>
        DragMode,
    }
}