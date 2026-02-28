using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public class AorListItemPool
    {
        GameObject mPrefabObj;
        string mPrefabName;
        int mInitCreateCount = 1;
        float mPadding = 0;
        float mStartPosOffset = 0;
        List<AorListViewItem> mTmpPooledItemList = new List<AorListViewItem>();
        List<AorListViewItem> mPooledItemList = new List<AorListViewItem>();
        static int mCurItemIdCount = 0;
        RectTransform mItemParent = null;

        public AorListItemPool()
        {
        }

        public void Init(GameObject prefabObj, float padding, float startPosOffset, int createCount,
            RectTransform parent)
        {
            mPrefabObj = prefabObj;
            UIFlagBehaviour uiFlagBehaviour = mPrefabObj.GetOrAddComponent<UIFlagBehaviour>();
            if (uiFlagBehaviour)
            {
                uiFlagBehaviour.FollowParentDestroy = true;
            }

            mPrefabName = mPrefabObj.name;
            mInitCreateCount = createCount;
            mPadding = padding;
            mStartPosOffset = startPosOffset;
            mItemParent = parent;
            mPrefabObj.SetActive(false);
            for (int i = 0; i < mInitCreateCount; ++i)
            {
                AorListViewItem tViewItem = CreateItem();
                RecycleItemReal(tViewItem);
            }
        }

        public AorListViewItem GetItem()
        {
            mCurItemIdCount++;
            AorListViewItem tItem = null;
            if (mTmpPooledItemList.Count > 0)
            {
                int count = mTmpPooledItemList.Count;
                tItem = mTmpPooledItemList[count - 1];
                mTmpPooledItemList.RemoveAt(count - 1);
                tItem.gameObject.SetActive(true);
            }
            else
            {
                int count = mPooledItemList.Count;
                if (count == 0)
                {
                    tItem = CreateItem();
                }
                else
                {
                    tItem = mPooledItemList[count - 1];
                    mPooledItemList.RemoveAt(count - 1);
                    tItem.gameObject.SetActive(true);
                }
            }

            tItem.Padding = mPadding;
            tItem.ItemId = mCurItemIdCount;
            return tItem;
        }

        public void DestroyAllItem()
        {
            ClearTmpRecycledItem();
            int count = mPooledItemList.Count;
            for (int i = 0; i < count; ++i)
            {
                GameObject.DestroyImmediate(mPooledItemList[i].gameObject);
            }

            mPooledItemList.Clear();
        }

        public AorListViewItem CreateItem()
        {
            GameObject go = AssetManager.Instance.InstantiateGO(mItemParent, mPrefabObj);
            go.SetActive(true);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;

            UIFlagBehaviour uiFlagBehaviour = go.GetOrAddComponent<UIFlagBehaviour>();
            if (uiFlagBehaviour)
            {
                uiFlagBehaviour.FollowParentDestroy = true;
            }

            RectTransform rf = go.GetComponent<RectTransform>();
            rf.localScale = Vector3.one;
            rf.anchoredPosition3D = Vector3.zero;
            rf.localEulerAngles = Vector3.zero;
            AorListViewItem tViewItem = go.GetComponent<AorListViewItem>();
            tViewItem.ItemPrefabName = mPrefabName;
            tViewItem.StartPosOffset = mStartPosOffset;
            return tViewItem;
        }

        void RecycleItemReal(AorListViewItem item)
        {
            item.gameObject.SetActive(false);
            mPooledItemList.Add(item);
        }

        public void RecycleItem(AorListViewItem item)
        {
            mTmpPooledItemList.Add(item);
        }

        public void ClearTmpRecycledItem()
        {
            int count = mTmpPooledItemList.Count;
            if (count == 0)
            {
                return;
            }

            for (int i = 0; i < count; ++i)
            {
                RecycleItemReal(mTmpPooledItemList[i]);
            }

            mTmpPooledItemList.Clear();
        }
    }
}