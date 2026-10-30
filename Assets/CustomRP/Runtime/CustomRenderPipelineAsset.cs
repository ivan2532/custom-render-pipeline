using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    [CreateAssetMenu(
        fileName = "CustomRenderPipelineAsset",
        menuName = "Rendering/Custom Render Pipeline Asset"
    )]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new CustomRenderPipeline();
        }
    }
}
