using System.Collections;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 持久化存储组件（分部类）
    /// 提供 PlayerPrefs / 分类数据 保存、删除、延迟存储功能
    /// </summary>
    public sealed partial class PersistComponent
    {
        /// <summary>
        /// 保存PlayerPrefs数据的协程引用
        /// </summary>
        private Coroutine _SavePlayerPrefsDataCoroutine;

        /// <summary>
        /// 根据分类名称保存指定类型的数据
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名称</param>
        public void SaveByClassifyName(PersistWayType wayType, string classifyName)
        {
            Save(wayType, classifyName);
        }

        /// <summary>
        /// 根据分类名称移除指定类型的所有数据
        /// </summary>
        /// <param name="wayType">持久化方式</param>
        /// <param name="classifyName">分类名称</param>
        public void RemoveAllItemsByClassifyName(PersistWayType wayType, string classifyName)
        {
            RemoveAllItems(wayType, classifyName);
        }

        /// <summary>
        /// 在当前帧结束后 异步保存PlayerPrefs数据（避免频繁调用）
        /// </summary>
        public void SavePlayerPrefsDataAfterFrameEnd()
        {
            Log.Info($"SavePlayerPrefsDataAfterFrameEnd ---> frameCount: {Time.frameCount}");

            // 防止重复开启协程
            if (_SavePlayerPrefsDataCoroutine == null)
            {
                _SavePlayerPrefsDataCoroutine = StartCoroutine(CoSaveGameDataAfterFrameEnd());
            }
        }

        /// <summary>
        /// 帧结束后 真正执行保存的协程
        /// </summary>
        private IEnumerator CoSaveGameDataAfterFrameEnd()
        {
            // 等待当前帧渲染完毕
            yield return new WaitForEndOfFrame();

            // 执行保存
            Save(PersistWayType.PlayerPrefs);

            Log.Info($"SavePlayerPrefsDataAfterFrameEnd Done ---> frameCount: {Time.frameCount}");

            // 重置协程引用，允许下次调用
            _SavePlayerPrefsDataCoroutine = null;
        }
    }
}