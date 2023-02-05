using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light m_directionalLight;

    [SerializeField] private LightingSettings m_lightingSettings;

    [SerializeField] [Range(0, 24)] private float m_timeOfDay;


    public void OnValidate()
    {
        if (m_directionalLight == null)
        {
            if (RenderSettings.sun != null)
            {
                m_directionalLight = RenderSettings.sun;
            }
            else
            {
                var lights = FindObjectsOfType<Light>();
                foreach (var light in lights)
                    if (light.type == LightType.Directional)
                    {
                        m_directionalLight = light;
                        return;
                    }
            }
        }
    }

    private void UpdateLighting(float t)
    {
        RenderSettings.ambientLight = m_lightingSettings.m_ambientColour.Evaluate(t);
        RenderSettings.fogColor = m_lightingSettings.m_fogColour.Evaluate(t);

        if (m_directionalLight != null)
        {
            m_directionalLight.color = m_lightingSettings.m_directionalColour.Evaluate(t);
            m_directionalLight.transform.rotation = Quaternion.Euler(new Vector3(t * 360 - 90, 170, 0));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_lightingSettings != null) UpdateLighting(TimeSystem.GameTime01);
    }
}