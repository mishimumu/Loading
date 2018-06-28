using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField]
    private Slider m_slider;
    [SerializeField]
    private Text m_tips;
    [SerializeField]
    private Text m_process;
    [SerializeField]
    private AnimationCurve m_curve;
    [SerializeField]
    private GameObject m_viewObj;
    private float m_time;
    private bool m_isLoading;
    private float m_processValue;
    private float m_lastProcessValue;
    private float m_dValue;
    private float m_difference = 0.0001f;

    private void Awake()
    {
        SceneMgr.Instance.FreshProcessEvent += Refresh;
        // Close();
    }

    void Update()
    {
        /*
         * 进度条显示策略：前30%在1秒内定值dValue曲线变化
         * 后面的进度，收到新的进度重置曲线时间轴，设置进度条初值为上次的进度值，变化值为（目标值-初值）*曲线值
         */
        if (m_isLoading)
        {
            m_time += Time.deltaTime;
            m_slider.value = m_lastProcessValue + m_curve.Evaluate(m_time) * m_dValue;
            float sliderValue = m_slider.value * 100;
            m_process.text = (int)sliderValue + "%";
            if (Mathf.Abs(1 - m_slider.value) <= m_difference)
            {
                float delayTime = m_time < m_curve.keys[1].time ? m_curve.keys[1].time - m_time + 0.2f : 0.2f;
                Invoke("Close", delayTime);
            }
        }
    }

    private void Refresh(float value)
    {
        if (value <= 0 || value > 1) return;
        // m_viewObj.SetActive(true);
        m_isLoading = true;
        if (value <= SceneConst.LOADSCENEPROCESS)
        {
            m_lastProcessValue = 0;
            m_processValue = value;
            m_dValue = SceneConst.LOADSCENEPROCESS;
        }
        else
        {
            m_time = 0;
            m_lastProcessValue = m_slider.value;
            m_processValue = value;
            m_dValue = m_processValue - m_lastProcessValue;
        }
    }

    void Close()
    {
        m_time = 0;
        // m_viewObj.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneMgr.Instance.FreshProcessEvent -= Refresh;
    }

}

