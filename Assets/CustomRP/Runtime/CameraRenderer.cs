using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public partial class CameraRenderer
    {
        private static readonly ShaderTagId UnlitShaderTagId = new("SRPDefaultUnlit");

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

            PrepareBuffer();
            PrepareForSceneView();
            Cull();
            Setup();
            DrawVisibleGeometry();
            DrawUnsupportedShaders();
            DrawGizmos();
            Submit();
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

        private void Setup()
        {
            _context.SetupCameraProperties(_camera);

            CameraClearFlags clearFlags = _camera.clearFlags;
            _buffer.ClearRenderTarget(
                clearFlags <= CameraClearFlags.Depth,
                clearFlags <= CameraClearFlags.Color,
                clearFlags == CameraClearFlags.Color ? _camera.backgroundColor.linear : Color.clear
            );

            _buffer.BeginSample(SampleName);

            ExecuteBuffer();
        }

        private void DrawVisibleGeometry()
        {
            DrawOpaqueGeometry();
            _context.DrawSkybox(_camera);
            DrawTransparentGeometry();
        }

        private void Submit()
        {
            _buffer.EndSample(SampleName);
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
            DrawingSettings drawingSettings = new(UnlitShaderTagId, sortingSettings);
            FilteringSettings filteringSettings = new(RenderQueueRange.opaque);

            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        }

        private void DrawTransparentGeometry()
        {
            SortingSettings sortingSettings = new(_camera)
            {
                criteria = SortingCriteria.CommonTransparent,
            };
            DrawingSettings drawingSettings = new(UnlitShaderTagId, sortingSettings);
            FilteringSettings filteringSettings = new(RenderQueueRange.transparent);

            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        }
    }
}
