using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public class CameraRenderer
    {
        private ScriptableRenderContext _context;
        private Camera _camera;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            _context = context;
            _camera = camera;

            Setup();
            DrawVisibleGeometry();
            Submit();
        }

        private void Setup()
        {
            _context.SetupCameraProperties(_camera);
        }

        private void DrawVisibleGeometry()
        {
            _context.DrawSkybox(_camera);
        }

        private void Submit()
        {
            _context.Submit();
        }
    }
}
