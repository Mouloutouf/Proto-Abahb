#if LETAI_TRUESHADOW
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeTai.TrueShadow.PluginInterfaces;

namespace MPUIKIT
{
    public partial class MPImage : ITrueShadowRendererNormalMaterialProvider
    {
        public Material GetTrueShadowRendererNormalMaterial()
        {
            return m_Material;
        }
    }
}
#endif