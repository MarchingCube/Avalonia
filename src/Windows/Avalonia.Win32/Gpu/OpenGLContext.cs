// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Platform.Gpu;
using OpenTK.Graphics;
using OpenTK.Platform;

namespace Avalonia.Win32.Gpu
{
    /// <summary>
    /// Win32 based OpenGL context.
    /// </summary>
    public class OpenGLContext : IOpenGLContext
    {
        private readonly IWindowInfo _windowInfo;
        private readonly IGraphicsContext _graphicsContext;

        /// <summary>
        /// Create new OpenGL context for given window info.
        /// </summary>
        /// <param name="windowInfo">Window info.</param>
        public OpenGLContext(IWindowInfo windowInfo)
        {
            _windowInfo = windowInfo ?? throw new ArgumentNullException(nameof(windowInfo));

            _graphicsContext = new GraphicsContext(GraphicsMode.Default, _windowInfo, 4, 0, GraphicsContextFlags.Default);
            _graphicsContext.LoadAll();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _graphicsContext.Dispose();
            _windowInfo.Dispose();
        }

        /// <inheritdoc />
        public void ResizeNotify()
        {
            _graphicsContext.Update(_windowInfo);
        }

        /// <inheritdoc />
        public void MakeCurrent()
        {
            _graphicsContext.MakeCurrent(_windowInfo);
        }

        /// <inheritdoc />
        public void SwapBuffers()
        {
            _graphicsContext.SwapBuffers();
        }
    }
}