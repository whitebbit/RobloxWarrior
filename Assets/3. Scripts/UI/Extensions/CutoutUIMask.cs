using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace _3._Scripts.UI.Extensions
{
    public class CutoutUIMask : Image
    {
        private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

        public override Material materialForRendering
        {
            get
            {
                var rendering = new Material(base.materialForRendering);
                rendering.SetInt(StencilComp, (int)CompareFunction.NotEqual);
                return rendering;
            }
        }
    }
}