using System.IO;
using Avalonia.VisualTree;

namespace Avalonia.Platform
{
    public readonly struct RenderDiagnostics
    {
        public Stream DiagnosticsStream { get; }
        public string FileExtension { get; }

        public RenderDiagnostics(Stream stream, string fileExtension)
        {
            DiagnosticsStream = stream;
            FileExtension = fileExtension;
        }
    }

    public interface IPlatformRenderDebugInterface
    {
        RenderDiagnostics? CreateRenderDiagnostics(IVisual visual);
    }
}
