using System;
using System.Reactive.Disposables;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.VisualTree;
using SkiaSharp;

namespace Avalonia.Skia
{
    internal class DebugPictureRenderTarget : IRenderTarget
    {
        private readonly PixelSize _pixelSize;
        private readonly Matrix _transform;

        public DebugPictureRenderTarget(PixelSize pixelSize, Matrix transform)
        {
            _pixelSize = pixelSize;
            _transform = transform;
        }

        public SKPicture Picture { get; private set; }

        public void Dispose()
        {
            Picture?.Dispose();
        }

        public void Render(IVisual visual)
        {
            ImmediateRenderer.Render(visual, this);
        }

        public IDrawingContextImpl CreateDrawingContext(IVisualBrushRenderer visualBrushRenderer)
        {
            var recorder = new SKPictureRecorder();

            var canvas = recorder.BeginRecording(new SKRect(0, 0, _pixelSize.Width, _pixelSize.Height));

            IDisposable finalizeRendering = Disposable.Create((instance: this, recorder), state => state.instance.Picture = state.recorder.EndRecording());

            var context = new DrawingContextImpl(new DrawingContextImpl.CreateInfo
            {
                Canvas = canvas,
                Dpi = SkiaPlatform.DefaultDpi,
                VisualBrushRenderer = visualBrushRenderer,
            }, finalizeRendering, recorder);

            context.Transform = _transform;

            return context;
        }
    }
}
