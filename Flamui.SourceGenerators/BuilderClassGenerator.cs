namespace Flamui.SourceGenerators;

public static class BuilderClassGenerator
{
    public const string Attribute = @"
namespace Flamui;

[System.AttributeUsage(System.AttributeTargets.Property)]
public class ParameterAttribute : System.Attribute
{

    public ParameterAttribute(bool isRef = false)
    {

    }

}
";

    public static string Generate(FlamuiComponentSg component)
    {
        var sb = new SourceBuilder();

        sb.AppendFormat("namespace {0};", component.Component.ContainingNamespace.ToDisplayString()).AppendLine();
        sb.AppendLine();

        sb.AppendFormat("public partial struct {0}Builder", component.Component.Name).AppendLine();
        sb.AppendLine("{");
        sb.AddIndent();

        BuildFields(sb, component);

        BuildConstructor(sb, component);

        foreach (var parameter in component.Parameters.AsSpan())
        {
            if(parameter.Mandatory)
                continue;

            sb.AppendLine();

            BuildParameterMethods(sb, parameter, component.Component.Name);
        }

        BuildBuildMethod(sb, component);

        sb.RemoveIndent();
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void BuildConstructor(SourceBuilder sb, FlamuiComponentSg component)
    {
        sb.AppendFormat("public {0}Builder({1} component)", component.Component.Name, component.Component.ToDisplayString()).AppendLine();
        sb.AppendLine("{");
        sb.AddIndent();

        sb.AppendLine("Component = component;");

        sb.RemoveIndent();
        sb.AppendLine("}");
    }

    private static void BuildParameterMethods(SourceBuilder sb, ComponentParameter parameter, string typeName)
    {

        sb.AppendFormat("public unsafe {0}Builder {1}({2}{3} {1})", typeName, parameter.Name, parameter.IsRef ? "ref " : string.Empty, parameter.FullTypename);
        sb.AppendLine("{");
        sb.AddIndent();

        BuildFieldAssignement(sb, parameter);

        sb.AppendLine("return this;");

        sb.RemoveIndent();
        sb.AppendLine("}");
    }

    private static void BuildFields(SourceBuilder sb, FlamuiComponentSg flamuiComponentSg)
    {
        sb.AppendFormat("public {0} Component {{ get; }}", flamuiComponentSg.Component.ToDisplayString()).AppendLine();

        foreach (var parameter in flamuiComponentSg.Parameters.AsSpan())
        {
            if (parameter.IsRef)
            {
                sb.AppendFormat("private {0}* {1};", parameter.FullTypename, parameter.Name).AppendLine();
            }
        }

        sb.AppendLine();
    }

    private static void BuildFieldAssignement(SourceBuilder sb, ComponentParameter componentParameter)
    {
        sb.AppendFormat("Component.{0} = {0};", componentParameter.Name, componentParameter.Name).AppendLine();

        if (!componentParameter.IsRef)
            return;

        sb.AppendFormat("fixed ({0}* ptr = &{1})", componentParameter.FullTypename, componentParameter.Name).AppendLine();
        sb.AppendLine("{");
        sb.AddIndent();

        sb.AppendFormat("_{0} = ptr;", componentParameter.Name);

        sb.RemoveIndent();
        sb.AppendLine("}");
        sb.AppendLine();
    }

    private static void BuildBuildMethod(SourceBuilder sb, FlamuiComponentSg sg)
    {
        sb.AppendLine();
        sb.AppendLine("public void Build()");
        sb.AppendLine("{");
        sb.AddIndent();

        sb.AppendLine("Component.Build();");

        foreach (var parameter in sg.Parameters.AsSpan())
        {
            if (parameter.IsRef)
            {
                sb.AppendFormat("Component.{0} = *_{0};", parameter.Name);
            }
        }

        sb.RemoveIndent();
        sb.AppendLine("}");
    }
}
