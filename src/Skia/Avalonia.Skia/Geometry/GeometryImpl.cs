// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Media;
using Avalonia.Platform;
using SkiaSharp;

namespace Avalonia.Skia
{
    abstract class GeometryImpl : IGeometryImpl
    {
        public abstract Rect Bounds { get; }
        public abstract SKPath EffectivePath { get; }
        public abstract bool FillContains(Point point);
        public abstract Rect GetRenderBounds(Pen pen);
        public abstract IGeometryImpl Intersect(IGeometryImpl geometry);
        public abstract bool StrokeContains(Pen pen, Point point);
        public abstract ITransformedGeometryImpl WithTransform(Matrix transform);
    }
}
