// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Platform;
using SkiaSharp;

namespace Avalonia.Skia.Gpu
{
    /// <summary>
    /// Render context for Gpu accelerated Skia rendering.
    /// </summary>
    public interface IGpuRenderContext : IDisposable
    {
        /// <summary>
        /// Skia graphics context.
        /// </summary>
        GRContext Context { get; }

        /// <summary>
        /// Platform handle bound to given context.
        /// </summary>
        IPlatformHandle PlatformHandle { get; }

        /// <summary>
        /// Get primary framebuffer (usually window) descriptor.
        /// </summary>
        /// <returns></returns>
        FramebufferDescriptor GetPrimaryFramebufferDescriptor();

        /// <summary>
        /// Notify context that backing framebuffer was resized.
        /// </summary>
        void NotifyResize();

        /// <summary>
        /// Prepare context for rendering commands.
        /// </summary>
        void PrepareForRendering();

        /// <summary>
        /// Flush Gpu rendering commands.
        /// </summary>
        void Flush();

        /// <summary>
        /// Present rendering results to framebuffer.
        /// </summary>
        void Present();
    }
}