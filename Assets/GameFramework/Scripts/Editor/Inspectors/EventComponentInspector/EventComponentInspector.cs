using Honor.Runtime;
using UnityEditor;

namespace Honor.Editor
{
    [CustomEditor(typeof(EventComponent))]
    public class EventComponentInspector: HonorComponentInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("仅在运行时有效。", MessageType.Info);
                return;
            }

            EventComponent t = (EventComponent)target;

            if (IsPrefabInHierarchy(t.gameObject))
            {
                if (t.EventManager != null)
                {
                    EditorGUILayout.LabelField("已注册Event类型数量", t.SubscribedEventTypeCount.ToString());
                    EditorGUILayout.LabelField("待派发Event数量", t.EventsForFireCount.ToString());
                }
            }

            Repaint();
        }

        private void OnEnable()
        {
        }
    }
}