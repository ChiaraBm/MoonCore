using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace MoonCore.Blazor.FlyonUi.Common;

public class ComponentOptions : IDictionary<string, object?>
{
    public ICollection<string> Keys => Parameters.Keys;
    public ICollection<object?> Values => Parameters.Values;
    public int Count => Parameters.Count;
    public bool IsReadOnly => false;

    protected readonly Dictionary<string, object?> Parameters = new();

    public void Add(KeyValuePair<string, object?> item)
        => Parameters.Add(item.Key, item.Value);

    public void Add(string key, object? value)
        => Parameters.Add(key, value);

    public void Clear()
        => Parameters.Clear();

    public bool Contains(KeyValuePair<string, object?> item)
        => Parameters.Contains(item);

    public bool ContainsKey(string key)
        => Parameters.ContainsKey(key);

    public bool Remove(KeyValuePair<string, object?> item)
        => Parameters.Remove(item.Key);

    public bool Remove(string key)
        => Parameters.Remove(key);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        => Parameters.TryGetValue(key, out value);

    public void CopyTo(KeyValuePair<string, object?>[] array, int index)
    {
        if (index > array.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (array.Length - index < Parameters.Count)
            throw new ArgumentException(nameof(array));

        foreach (var parameter in Parameters)
            array[index++] = new KeyValuePair<string, object?>(parameter.Key, parameter.Value);
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => Parameters.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Parameters.GetEnumerator();

    public object? this[string key]
    {
        get => Parameters[key];
        set => Parameters[key] = value;
    }
}

public sealed class ComponentOptions<TComponent> : ComponentOptions where TComponent : ComponentBase
{
    public void Add<TValue>(Expression<Func<TComponent, TValue>> expression, TValue value)
    {
        if (expression.Body is not MemberExpression memberExpression)
            throw new ArgumentException("Expression must be a member expression", nameof(expression));
        
        Parameters.Add(memberExpression.Member.Name, value);
    }
}