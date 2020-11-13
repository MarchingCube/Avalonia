using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using XamlX.Ast;
using XamlX.Emit;
using XamlX.IL;
using XamlX.Transform;
using XamlX.TypeSystem;

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

                        if (classes.Length == 0)
                        {
                            return node;
                        }
                        else if (classes.Length == 1)
                        {
                            propertyValueNode.Values.Add(new XamlConstantNode(node, context.Configuration.WellKnownTypes.String, classes[0]));
                        }
                        else
                        {
                            propertyValueNode.Values.Add(
                                new XamlConstantArrayNode(node,
                                    context.Configuration.WellKnownTypes.IListOfT.MakeGenericType(context.Configuration.WellKnownTypes.String),
                                    typeof(string),
                                    classes));
                        }
                    }

                    return propertyValueNode;
                }
            }

            return node;
        }
    }

    class XamlConstantArrayNode : XamlAstNode, IXamlAstValueNode, IXamlAstEmitableNode<IXamlILEmitter, XamlILNodeEmitResult>
    {
        public Type ElementType { get; }

        public IList<object> Values { get; }

        public XamlConstantArrayNode(IXamlLineInfo lineInfo, IXamlType type, Type elementType, IList<object> values) : base(lineInfo)
        {
            if (!elementType.IsPrimitive && elementType != typeof(string))
                throw new ArgumentException($"Don't know how to emit {elementType} arrays.");
            ElementType = elementType;
            Values = values;
            Type = new XamlAstClrTypeReference(lineInfo, type, false);
        }

        public IXamlAstTypeReference Type { get; }

        private void EmitStore(IXamlILEmitter codeGen, int index)
        {
            var value = Values[index];

            codeGen.Ldc_I4(index);

            if (value is string)
            {
                codeGen
                    .Ldstr((string)value)
                    .Stelem_ref();
            }
            else if (value is long || value is ulong)
            {
                codeGen.Emit(OpCodes.Ldc_I8, TypeSystemHelpers.ConvertLiteralToLong(value));

                codeGen.Emit(OpCodes.Stelem_I8);
            }
            else if (value is float f)
            {
                codeGen.Emit(OpCodes.Ldc_R4, f);

                codeGen.Emit(OpCodes.Stelem_R4);
            }
            else if (value is double d)
            {
                codeGen.Emit(OpCodes.Ldc_R8, d);

                codeGen.Emit(OpCodes.Stelem_R8);
            }
            else
            {
                codeGen.Emit(OpCodes.Ldc_I4, TypeSystemHelpers.ConvertLiteralToInt(value));

                codeGen.Emit(OpCodes.Stelem_I4);
            }
        }

        public XamlILNodeEmitResult Emit(XamlEmitContext<IXamlILEmitter, XamlILNodeEmitResult> context, IXamlILEmitter codeGen)
        {
            var count = Values.Count;

            codeGen
                .Ldc_I4(count)
                .Newarr(context.Configuration.WellKnownTypes.String);

            for (int i = 0; i < count; i++)
            {
                codeGen.Dup();

                EmitStore(codeGen, i);
            }

            return XamlILNodeEmitResult.Type(0, Type.GetClrType());
        }
    }
}
