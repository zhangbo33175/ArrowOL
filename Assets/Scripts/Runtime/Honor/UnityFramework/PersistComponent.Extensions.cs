using System.Collections;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class PersistComponent
    {
        private Coroutine _SavePlayerPrefsDataCoroutine;

        /// <summary>
        /// 保存指定分类的数据
        /// </summary>
        /// <param name="wayType"></param>
        /// <param name="classifyName"></param>
        public void SaveByClassifyName(PersistWayType wayType, string classifyName)
        {
            Save(wayType, classifyName);
        }

        /// <summary>
        /// 移除指定分类的所有数据
        /// </summary>
        /// <param name="wayType"></param>
        /// <param name="classifyName"></param>
        public void RemoveAllItemsByClassifyName(PersistWayType wayType, string classifyName)
        {
            RemoveAllItems(wayType, classifyName);
        }

        /// <summary>
        /// 在帧末保存PlayerPrefs数据
        /// </summary>
        public void SavePlayerPrefsDataAfterFrameEnd()
        {
            Log.Info($"SavePlayerPrefsDataAfterFrameEnd ---> frameCount: {Time.frameCount}");

            if (_SavePlayerPrefsDataCoroutine == null)
            {
                _SavePlayerPrefsDataCoroutine = StartCoroutine(CoSaveGameDataAfterFrameEnd());
            }
        }

        /// <summary>
        /// 帧末保存数据
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoSaveGameDataAfterFrameEnd()
        {
            yield return new WaitForEndOfFrame();
            Save(PersistWayType.PlayerPrefs);
            Log.Info($"SavePlayerPrefsDataAfterFrameEnd Done ---> frameCount: {Time.frameCount}");
            _SavePlayerPrefsDataCoroutine = null;
        }
        
    }
}