// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Platform.Gpu
{
    /// <summary>
    /// OpenGL rendering context.
    /// </summary>
    public interface IOpenGLContext : IDisposable
    {
        /// <summary>
        /// Notify context that it's backing window was resized.
        /// </summary>
        void ResizeNotify();

        /// <summary>
        /// Make context current.
        /// </summary>
        void MakeCurrent();

        /// <summary>
        /// Swap buffers of backing window.
        /// </summary>
        void SwapBuffers();
    }
}
