// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Skia.Gpu;
using Avalonia.Skia.Helpers;
using SkiaSharp;

namespace Avalonia.Skia.Media
{
    /// <summary>
    /// Skia render target that renders to a window using Gpu acceleration.
    /// </summary>
    public class WindowRenderTarget : IRenderTarget
    {
        private readonly TopLevel _topLevel;
        private readonly IGpuRenderContext _renderContext;
        private readonly IDisposable _postRenderHandler;
        private GRBackendRenderTargetDesc _rtDesc;
        private SKSurface _surface;
        
        /// <summary>
        /// Create new window render target that will render to given handle using passed render backend.
        /// </summary>
        /// <param name="platformHandle">Platform handle to render to.</param>
        /// <param name="renderBackend">Render backend to use.</param>
        public WindowRenderTarget(IPlatformHandle platformHandle, IGpuRenderBackend renderBackend)
        {
            if (platformHandle == null) throw new ArgumentNullException(nameof(platformHandle));
            if (renderBackend == null) throw new ArgumentNullException(nameof(renderBackend));

            // Kinda dirty, but I am unaware of better way of cross-plat window size tracking.
            _topLevel = Window.OpenWindows.FirstOrDefault(w => w.PlatformImpl?.Handle == platformHandle);

            if (_topLevel == null)
            {
                throw new InvalidOperationException("Failed to find TopLevel window for given platform handle");
            }
            
            _renderContext = renderBackend.CreateRenderContext(platformHandle);

            if (_renderContext == null)
            {
                throw new InvalidOperationException("Failed to create Skia render context");
            }
            
            _rtDesc = CreateInitialRenderTargetDesc();
            _postRenderHandler = new LambdaDisposable(Flush);
        }

        private GRBackendRenderTargetDesc CreateInitialRenderTargetDesc()
        {
            var framebufferDesc = _renderContext.GetPrimaryFramebufferDescriptor();
            
            var pixelConfig = SKImageInfo.PlatformColorType == SKColorType.Bgra8888
                ? GRPixelConfig.Bgra8888
                : GRPixelConfig.Rgba8888;

            var rtDesc = new GRBackendRenderTargetDesc
            {
                Width = 0,
                Height = 0,
                Config = pixelConfig,
                Origin = GRSurfaceOrigin.BottomLeft,
                SampleCount = framebufferDesc.SampleCount,
                StencilBits = framebufferDesc.StencilBits,
                RenderTargetHandle = framebufferDesc.FramebufferHandle
            };

            return rtDesc;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _surface?.Dispose();
            _renderContext.Dispose();
        }
        
        /// <summary>
        /// Flush rendering commands and present.
        /// </summary>
        private void Flush()
        {
            _renderContext.Context.Flush();
            _renderContext.Present();
        }

        /// <summary>
        /// Create surface if needed.
        /// </summary>
        private void CreateSurface()
        {
            var newSize = _topLevel.ClientSize;
            var newWidth = (int)newSize.Width;
            var newHeight = (int)newSize.Height;

            if (_surface == null || newWidth != _rtDesc.Width || newHeight != _rtDesc.Height)
            {
                _surface?.Dispose();

                _rtDesc.Width = newWidth;
                _rtDesc.Height = newHeight;

                _renderContext.NotifyResize();

                _surface = SKSurface.Create(_renderContext.Context, _rtDesc);

                if (_surface == null)
                {
                    throw new InvalidOperationException("Failed to create Skia surface for window render target");
                }
            }
        }

        /// <inheritdoc />
        public IDrawingContextImpl CreateDrawingContext(IVisualBrushRenderer visualBrushRenderer)
        {
            CreateSurface();

            _renderContext.PrepareForRendering();

            var canvas = _surface.Canvas;

            canvas.RestoreToCount(-1);
            canvas.ResetMatrix();

            var createInfo = new DrawingContextImpl.CreateInfo
            {
                Canvas = canvas,
                Dpi = SkiaPlatform.DefaultDpi,
                VisualBrushRenderer = visualBrushRenderer,
                RenderContext = _renderContext
            };

            return new DrawingContextImpl(createInfo, _postRenderHandler);
        }
    }
}