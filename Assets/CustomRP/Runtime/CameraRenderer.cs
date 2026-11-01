using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public class CameraRenderer
    {
        private static ShaderTagId _unlitShaderTagId = new("SRPDefaultUnlit");

        private const string BufferName = "RenderCamera";

        private ScriptableRenderContext _context;
        private Camera _camera;
        private CommandBuffer _buffer;
        private CullingResults _cullingResults;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            _context = context;
            _camera = camera;
            _buffer = new CommandBuffer { name = BufferName };

            Setup();
            Cull();
            DrawVisibleGeometry();
            Submit();
        }

        private void Setup()
        {
            _context.SetupCameraProperties(_camera);

            _buffer.ClearRenderTarget(true, true, Color.clear);
            _buffer.BeginSample(BufferName);
            ExecuteBuffer();
        }

        private void Cull()
        {
            if (!_camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParameters))
            {
                throw new InvalidOperationException(
                    "Camera render failed because of a failure to get culling parameters!"
                );
            }

            _cullingResults = _context.Cull(ref cullingParameters);
        }

        private void DrawVisibleGeometry()
        {
            DrawOpaqueGeometry();
            _context.DrawSkybox(_camera);
            DrawTransparentGeometry();
        }

        private void Submit()
        {
            _buffer.EndSample(BufferName);
            ExecuteBuffer();
            _context.Submit();
        }

        private void ExecuteBuffer()
        {
            _context.ExecuteCommandBuffer(_buffer);
            _buffer.Clear();
        }

        private void DrawOpaqueGeometry()
        {
            SortingSettings sortingSettings = new(_camera)
            {
                criteria = SortingCriteria.CommonOpaque,
            };
            DrawingSettings drawingSettings = new(_unlitShaderTagId, sortingSettings);
            FilteringSettings filteringSettings = new(RenderQueueRange.opaque);

            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        }

        private void DrawTransparentGeometry()
        {
            SortingSettings sortingSettings = new(_camera)
            {
                criteria = SortingCriteria.CommonTransparent,
            };
            DrawingSettings drawingSettings = new(_unlitShaderTagId, sortingSettings);
            FilteringSettings filteringSettings = new(RenderQueueRange.transparent);

            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        }
    }
}
