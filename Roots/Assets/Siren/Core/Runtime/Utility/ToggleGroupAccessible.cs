using System.Collections.Generic;
using UnityEngine.UI;

namespace Siren
{
    /// <summary>
    /// Opens up Unity's standard toggle group to allow for easy access to the actual toggles contained within it
    /// </summary>
    public class ToggleGroupAccessible : ToggleGroup
    {
        public List<Toggle> GetAllToggles()
        {
            return m_Toggles;
        }
    }
}