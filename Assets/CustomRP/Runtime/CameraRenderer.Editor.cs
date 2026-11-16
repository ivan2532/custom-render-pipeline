using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRP.Runtime
{
    public partial class CameraRenderer
    {
        private partial void DrawGizmos();

        private partial void DrawUnsupportedShaders();

#if UNITY_EDITOR
        private static readonly ShaderTagId[] LegacyShaderTagIds =
        {
            new("Always"),
            new("ForwardBase"),
            new("PrepassBase"),
            new("Vertex"),
            new("VertexLMRGBM"),
            new("VertexLM"),
        };

        private static Material _errorMaterial;

        private partial void DrawGizmos()
        {
            if (!Handles.ShouldRenderGizmos())
            {
                return;
            }

            _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
            _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
        }

        private partial void DrawUnsupportedShaders()
        {
            if (_errorMaterial == null)
            {
                _errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
            }

            DrawingSettings drawingSettings = new(
                LegacyShaderTagIds[0],
                new SortingSettings(_camera)
            )
            {
                overrideMaterial = _errorMaterial,
            };

            for (int i = 1; i < LegacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, LegacyShaderTagIds[i]);
            }

            FilteringSettings filteringSettings = FilteringSettings.defaultValue;

            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        }
#endif
    }
}
