// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Platform;
using Avalonia.Platform.Gpu;

namespace Avalonia.Skia.Gpu
{
    /// <summary>
    /// Skia OpenGL render backend.
    /// </summary>
    public class OpenGLRenderBackend : IGpuRenderBackend
    {
        private readonly IOpenGLPlatform _openGLPlatform;
        private readonly IWindowImpl _globalWindow;

        /// <summary>
        /// Create new OpenGL render backend using provided platform.
        /// </summary>
        /// <param name="openGLPlatform">OpenGL platform to use.</param>
        /// <param name="windowingPlatform">Windowing platform to use.</param>
        public OpenGLRenderBackend(IOpenGLPlatform openGLPlatform, IWindowingPlatform windowingPlatform)
        {
            if (windowingPlatform == null) throw new ArgumentNullException(nameof(windowingPlatform));
            _openGLPlatform = openGLPlatform ?? throw new ArgumentNullException(nameof(openGLPlatform));
            
            // Need to create dummy hidden window for global resource context.
            _globalWindow = windowingPlatform.CreateWindow();

            ResourceRenderContext = CreateRenderContext(_globalWindow.Handle);

            if (ResourceRenderContext == null)
            {
                throw new InvalidOperationException("Failed to create global OpenGL resource context.");
            }
        }
        
        /// <inheritdoc />
        public IGpuRenderContext ResourceRenderContext { get; }
        
        /// <inheritdoc />
        public IGpuRenderContext CreateRenderContext(IPlatformHandle platformHandle)
        {
            return new OpenGLRenderContext(platformHandle, _openGLPlatform);
        }
    }
}