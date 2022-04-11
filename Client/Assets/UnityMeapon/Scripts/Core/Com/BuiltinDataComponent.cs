using UnityEngine;
using GameFramework;
using MeaponUnity.Core.Entry;
using UnityGameFramework.Runtime;
using MeaponUnity.Core.Definition;

namespace MeaponUnity.Core.Com
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField]
        private TextAsset m_BuildInfoTextAsset = null;

        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;

        private BuildInfo m_BuildInfo = null;

        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }

        public void InitBuildInfo()
        {
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Error("Build info can not be found or empty.");
                return;
            }

            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if (m_BuildInfo == null)
            {
                Log.Error("Parse build info failure.");
                return;
            }
        }

        public void InitDefaultDictionary()
        {
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Error("Default dictionary can not be found or empty.");
                return;
            }

            if (!MeaponEntry.Localization.ParseData(m_DefaultDictionaryTextAsset.text))
            {
                Log.Error("Parse default dictionary failure.");
                return;
            }
        }

    }
}
