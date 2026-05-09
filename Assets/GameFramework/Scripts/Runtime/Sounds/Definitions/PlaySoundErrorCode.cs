namespace Honor.Runtime
{
    /// <summary>
    /// 声音播放错误码枚举
    /// 用于标识播放音频时出现的各种失败原因，方便日志排查与逻辑判断
    /// </summary>
    public enum PlaySoundErrorCode : byte
    {
        /// <summary>
        /// 未知错误（未归类的异常）
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 错误：指定的声音组不存在
        /// </summary>
        SoundGroupNotExist,

        /// <summary>
        /// 错误：声音组内 AudioSource 代理数量不足，无法播放新声音
        /// </summary>
        SoundGroupHasNotEnoughAgent,

        /// <summary>
        /// 错误：音频资源加载失败（AB包/路径/文件问题）
        /// </summary>
        LoadAssetFailure,

        /// <summary>
        /// 提示：因当前声音优先级过低，被更高优先级声音挤占，未播放
        /// </summary>
        IgnoredDueToLowPriority,

        /// <summary>
        /// 错误：给 AudioSource 设置音频片段失败（资源格式/类型错误）
        /// </summary>
        SetSoundAssetFailure
    }
}