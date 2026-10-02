/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponent.cs
 * author:    taoye
 * created:   2020/8/26
 * descrip:   调试组件
 ***************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        private sealed partial class RuntimeMemorySummaryWindow : ScrollableDebuggerWindowBase
        {
            private readonly List<Record> m_Records = new List<Record>();
            private DateTime m_SampleTime = DateTime.MinValue;
            private int m_SampleCount = 0;
            private long m_SampleSize = 0L;

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Runtime Memory Summary</b>");
                GUILayout.BeginVertical("box");
                {
                    // by liuyu 去掉自定义button高度
                    if (GUILayout.Button("Take Sample"))
                    {
                        TakeSample();
                    }

                    if (m_SampleTime <= DateTime.MinValue)
                    {
                        GUILayout.Label("<b>Please take sample first.</b>");
                    }
                    else
                    {
                        GUILayout.Label($"<b>{m_SampleCount} Objects ({GetByteLengthString(m_SampleSize)}) obtained at {m_SampleTime.ToString("yyyy - MM - dd HH: mm:ss")}.</b>");

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("<b>Type</b>");
                            GUILayout.Label("<b>Count</b>", GUILayout.Width(120f));
                            GUILayout.Label("<b>Size</b>", GUILayout.Width(120f));
                        }
                        GUILayout.EndHorizontal();

                        for (int i = 0; i < m_Records.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(m_Records[i].Name);
                                GUILayout.Label(m_Records[i].Count.ToString(), GUILayout.Width(120f));
                                GUILayout.Label(GetByteLengthString(m_Records[i].Size), GUILayout.Width(120f));
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                GUILayout.EndVertical();
            }

            private void TakeSample()
            {
                m_Records.Clear();
                m_SampleTime = DateTime.Now;
                m_SampleCount = 0;
                m_SampleSize = 0L;

                UnityEngine.Object[] samples = Resources.FindObjectsOfTypeAll<UnityEngine.Object>();
                for (int i = 0; i < samples.Length; i++)
                {
                    long sampleSize = 0L;
#if UNITY_5_6_OR_NEWER
                    sampleSize = Profiler.GetRuntimeMemorySizeLong(samples[i]);
#else
                    sampleSize = Profiler.GetRuntimeMemorySize(samples[i]);
#endif
                    string name = samples[i].GetType().Name;
                    m_SampleCount++;
                    m_SampleSize += sampleSize;

                    Record record = null;
                    foreach (Record r in m_Records)
                    {
                        if (r.Name == name)
                        {
                            record = r;
                            break;
                        }
                    }

                    if (record == null)
                    {
                        record = new Record(name);
                        m_Records.Add(record);
                    }

                    record.Count++;
                    record.Size += sampleSize;
                }

                m_Records.Sort(RecordComparer);
            }

            private int RecordComparer(Record a, Record b)
            {
                int result = b.Size.CompareTo(a.Size);
                if (result != 0)
                {
                    return result;
                }

                result = a.Count.CompareTo(b.Count);
                if (result != 0)
                {
                    return result;
                }

                return a.Name.CompareTo(b.Name);
            }
        }
    }
}


