using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Animation;
using Avalonia.Animation.Animators;
using Avalonia.Media.Transformation;
using Avalonia.VisualTree;

namespace Avalonia.Media
{
    /// <summary>
    /// Creates an <see cref="ITransform"/> from a string representation.
    /// </summary>
    public class TransformConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return TransformOperations.Parse((string)value);
        }
    }

    [TypeConverter(typeof(TransformConverter))]
    public interface ITransform
    {
        Matrix Value { get; }
    }

    public interface IMutableTransform : ITransform
    {
        /// <summary>
        /// Raised when the transform changes.
        /// </summary>
        event EventHandler Changed;
    }

    /// <summary>
    /// Represents a transform on an <see cref="IVisual"/>.
    /// </summary>
    public abstract class Transform : Animatable, IMutableTransform
    {
        static Transform()
        {
            Animation.Animation.RegisterAnimator<TransformAnimator>(prop =>
                typeof(ITransform).IsAssignableFrom(prop.PropertyType));
        }

        /// <summary>
        /// Raised when the transform changes.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Gets the transform's <see cref="Matrix"/>.
        /// </summary>
        public abstract Matrix Value { get; }

        /// <summary>
        /// Parses a <see cref="Transform"/> string.
        /// </summary>
        /// <param name="s">Six comma-delimited double values that describe the new <see cref="Transform"/>. For details check <see cref="Matrix.Parse(string)"/> </param>
        /// <returns>The <see cref="Transform"/>.</returns>
        public static Transform Parse(string s)
        {
            return new MatrixTransform(Matrix.Parse(s));
        }

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        protected void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Returns a String representing this transform matrix instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
