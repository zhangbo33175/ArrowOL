/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundAgentHelper.cs
 * author:  云毅
 * created:
 * descrip:   声音代理辅助器 —— 封装Unity AudioSource，实现播放/暂停/淡入淡出/3D定位
 ***************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音代理辅助器（MonoBehaviour）
    /// 功能：直接封装 Unity AudioSource，实现播放、暂停、停止、淡入淡出、3D 定位
    /// 每个声音播放器对应一个此脚本
    /// </summary>
    public class SoundAgentHelper : MonoBehaviour
    {
        /// <summary>
        /// 缓存 Transform，提升性能
        /// </summary>
        private Transform m_CachedTransform;

        /// <summary>
        /// Unity 官方音频播放组件
        /// </summary>
        private AudioSource m_AudioSource;

        /// <summary>
        /// 暂停前保存的音量，用于恢复
        /// </summary>
        private float m_VolumeWhenPause;

        /// <summary>
        /// 当前是否正在播放
        /// </summary>
        public bool IsPlaying => m_AudioSource.isPlaying;

        /// <summary>
        /// 音频长度（秒）
        /// </summary>
        public float Length => m_AudioSource.clip != null ? m_AudioSource.clip.length : 0f;

        /// <summary>
        /// 当前播放时间位置
        /// </summary>
        public float Time
        {
            get => m_AudioSource.time;
            set => m_AudioSource.time = value;
        }

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool Mute
        {
            get => m_AudioSource.mute;
            set => m_AudioSource.mute = value;
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop
        {
            get => m_AudioSource.loop;
            set => m_AudioSource.loop = value;
        }

        /// <summary>
        /// 声音优先级（外部：数字越大优先级越高）
        /// </summary>
        public int Priority
        {
            get => 128 - m_AudioSource.priority;
            set => m_AudioSource.priority = 128 - value;
        }

        /// <summary>
        /// 最终音量
        /// </summary>
        public float Volume
        {
            get => m_AudioSource.volume;
            set => m_AudioSource.volume = value;
        }

        /// <summary>
        /// 音调 / 播放速度
        /// </summary>
        public float Pitch
        {
            get => m_AudioSource.pitch;
            set
            {
                m_AudioSource.pitch = value;
                m_AudioSource.time *= value; // 变速同步时间轴，防止跳变
            }
        }

        /// <summary>
        /// 立体声相位（-1 左，1 右）
        /// </summary>
        public float PanStereo
        {
            get => m_AudioSource.panStereo;
            set => m_AudioSource.panStereo = value;
        }

        /// <summary>
        /// 空间混合：0=2D，1=3D
        /// </summary>
        public float SpatialBlend
        {
            get => m_AudioSource.spatialBlend;
            set => m_AudioSource.spatialBlend = value;
        }

        /// <summary>
        /// 3D 声音最大距离
        /// </summary>
        public float MaxDistance
        {
            get => m_AudioSource.maxDistance;
            set => m_AudioSource.maxDistance = value;
        }

        /// <summary>
        /// 多普勒效果强度
        /// </summary>
        public float DopplerLevel
        {
            get => m_AudioSource.dopplerLevel;
            set => m_AudioSource.dopplerLevel = value;
        }

        /// <summary>
        /// 混音器轨道分组
        /// </summary>
        public AudioMixerGroup AudioMixerGroup
        {
            get => m_AudioSource.outputAudioMixerGroup;
            set => m_AudioSource.outputAudioMixerGroup = value;
        }

        /// <summary>
        /// 播放声音（支持淡入）
        /// </summary>
        public void Play(float fadeInSeconds)
        {
            StopAllCoroutines();
            m_AudioSource.Play();

            if (fadeInSeconds > 0f)
            {
                float finalVolume = m_AudioSource.volume;
                m_AudioSource.volume = 0f;
                StartCoroutine(FadeToVolume(finalVolume, fadeInSeconds));
            }
        }

        /// <summary>
        /// 停止声音（支持淡出）
        /// </summary>
        public void Stop(float fadeOutSeconds)
        {
            StopAllCoroutines();

            if (fadeOutSeconds > 0f && gameObject.activeInHierarchy)
                StartCoroutine(StopCo(fadeOutSeconds));
            else
                m_AudioSource.Stop();
        }

        /// <summary>
        /// 暂停声音（支持淡出）
        /// </summary>
        public void Pause(float fadeOutSeconds)
        {
            StopAllCoroutines();

            if (fadeOutSeconds > 0f && gameObject.activeInHierarchy)
                StartCoroutine(PauseCo(fadeOutSeconds));
            else
                m_AudioSource.Pause();
        }

        /// <summary>
        /// 恢复播放（支持淡入）
        /// </summary>
        public void Resume(float fadeInSeconds)
        {
            StopAllCoroutines();
            m_AudioSource.UnPause();

            if (fadeInSeconds > 0f)
                StartCoroutine(FadeToVolume(m_VolumeWhenPause, fadeInSeconds));
        }

        /// <summary>
        /// 重置状态（对象池复用）
        /// </summary>
        public void Reset()
        {
            if (m_CachedTransform)
                m_CachedTransform.localPosition = Vector3.zero;
            
            if (m_AudioSource)
                m_AudioSource.clip = null;
            
            m_VolumeWhenPause = 0f;
        }

        /// <summary>
        /// 设置音频片段
        /// </summary>
        public bool SetSoundAsset(object soundAsset)
        {
            AudioClip audioClip = soundAsset as AudioClip;
            if (audioClip == null)
                return false;

            m_AudioSource.clip = audioClip;
            return true;
        }

        /// <summary>
        /// 设置 3D 世界位置
        /// </summary>
        public void SetWorldPosition(Vector3 worldPosition)
        {
            m_CachedTransform.position = worldPosition;
        }

        private void Awake()
        {
            m_CachedTransform = transform;
            m_AudioSource = gameObject.GetOrAddComponent<AudioSource>();
            m_AudioSource.playOnAwake = false;
            m_AudioSource.rolloffMode = AudioRolloffMode.Custom;
        }

        /// <summary>
        /// 淡出后停止
        /// </summary>
        private IEnumerator StopCo(float fadeOutSeconds)
        {
            yield return FadeToVolume(0f, fadeOutSeconds);
            m_AudioSource.Stop();
        }

        /// <summary>
        /// 淡出后暂停
        /// </summary>
        private IEnumerator PauseCo(float fadeOutSeconds)
        {
            yield return FadeToVolume(0f, fadeOutSeconds);
            m_AudioSource.Pause();
        }

        /// <summary>
        /// 音量渐变通用协程
        /// </summary>
        private IEnumerator FadeToVolume(float targetVolume, float duration)
        {
            float elapsed = 0f;
            float startVolume = m_AudioSource.volume;

            while (elapsed < duration)
            {
                elapsed += UnityEngine.Time.deltaTime;
                m_AudioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                yield return null;
            }

            m_AudioSource.volume = targetVolume;
        }
    }
}