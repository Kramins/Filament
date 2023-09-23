
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace filament.scheduler;


public class ScheduleTask
{
    public string Type { get; }
    public string Method { get; }
    public List<string> ArgumentTypes { get; }
    public List<string> Arguments { get; set; }

    public ScheduleTask(string type, string method, List<string> argumentTypes, List<string> arguments)
    {

        this.Type = type;
        this.Method = method;
        this.ArgumentTypes = argumentTypes;
        this.Arguments = arguments;

    }
    public static ScheduleTask FromExpression(LambdaExpression methodCall, Type explicitType)
    {
        if (methodCall == null) throw new ArgumentNullException(nameof(methodCall));

        var callExpression = methodCall.Body as MethodCallExpression;
        if (callExpression == null)
        {
            throw new ArgumentException("Expression body should be of type `MethodCallExpression`", nameof(methodCall));
        }

        var type = explicitType ?? callExpression.Method.DeclaringType;
        var method = callExpression.Method;

        var argumentTypes = method.GetParameters().Select(x => x.ParameterType.FullName).ToList();
        var arguments = callExpression.Arguments.Select(x => (GetValueFromExpression(x))).ToList();


        return new ScheduleTask(
            type.FullName,
            method.Name,
            argumentTypes,
            arguments
            );
    }

    public static string GetValueFromExpression(Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var constantExpression = expression as ConstantExpression;
        if (constantExpression != null)
        {
            return constantExpression.Value.ToString();
        }

        var memberExpression = expression as MemberExpression;
        if (memberExpression != null)
        {
            var convertedExp = Expression.Convert(memberExpression, typeof(object));
            var compiledExpression = Expression.Lambda<Func<object>>(convertedExp).Compile();
            object nameValue = compiledExpression.Invoke();
            return nameValue.ToString();
        }

        throw new ArgumentException("Expression should be of type `ConstantExpression` or `MemberExpression`", nameof(expression));
    }
}
