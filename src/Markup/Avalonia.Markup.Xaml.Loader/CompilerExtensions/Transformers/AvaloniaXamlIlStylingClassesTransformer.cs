using XamlX.Ast;
using XamlX.Transform;

namespace Avalonia.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    using XamlParseException = XamlX.XamlParseException;
    using XamlLoadException = XamlX.XamlLoadException;

    class AvaloniaXamlIlStylingClassesTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (!(node is XamlAstXamlPropertyValueNode propertyValueNode))
                return node;

            if (!(propertyValueNode.Property is XamlAstClrProperty clrProperty))
                return node;

            var styledElement = context.GetAvaloniaTypes().StyledElement;

            if (clrProperty.DeclaringType == styledElement)
            {
                if (clrProperty.Name == "Classes")
                {
                    if (propertyValueNode.Values.Count == 1 && propertyValueNode.Values[0] is XamlAstTextNode valueText)
                    {
                        propertyValueNode.Values.Clear();

                        var classes = valueText.Text.Split(' ');

                        foreach (var @class in classes)
                        {
                            propertyValueNode.Values.Add(new XamlConstantNode(node,
                                context.Configuration.WellKnownTypes.String, @class));
                        }
                    }

                    return propertyValueNode;
                }
            }

            return node;
        }
    }
}
