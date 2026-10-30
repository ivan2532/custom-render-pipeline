# Custom Render Pipeline

Unity 2022.3.5f1 project following the Catlike Coding "Custom SRP" tutorial series:
https://catlikecoding.com/unity/tutorials/custom-srp/

## Conventions
- Pipeline C# code and shaders live under `Assets/` (e.g. `Assets/Custom RP/`).
- Follow the tutorial's naming (`CustomRenderPipeline`, `CameraRenderer`, etc.).
- Shaders are HLSL; keep `.hlsl` includes separate from `.shader` files.
- Never edit `Library/`, `Temp/`, `Logs/`, or `UserSettings/` (generated, git-ignored).
- Commit `.meta` files alongside their assets.

## Notes
- Unity Version Control (collab-proxy) package was removed intentionally; don't re-add.
- Unity must be closed before touching `Library/`.
