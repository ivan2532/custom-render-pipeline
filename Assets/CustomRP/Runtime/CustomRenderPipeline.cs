using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private readonly CameraRenderer _cameraRenderer = new();

        // Newer versions use a List<Camera> instead to avoid allocations,
        // we still must implement the old method for compatibility with older versions of Unity.
        protected override void Render(ScriptableRenderContext context, Camera[] cameras) { }

        protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
        {
            foreach (Camera camera in cameras)
            {
                _cameraRenderer.Render(context, camera);
            }
        }
    }
}
