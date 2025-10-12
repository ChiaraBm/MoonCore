using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace MoonCore.Blazor.FlyonUi.Common;

/// <summary>
/// Represents options for a type unspecific blazor component.
/// Used by e.g. the <see cref="Modals.ModalService"/> to define parameters
/// in a quick and easy way with type safe support using <see cref="ComponentOptions{TComponent}"/>
/// </summary>
public class ComponentOptions : IDictionary<string, object?>
{
    /// <inheritdoc />
    public ICollection<string> Keys => Parameters.Keys;

    /// <inheritdoc />
    public ICollection<object?> Values => Parameters.Values;

    /// <inheritdoc />
    public int Count => Parameters.Count;
    
    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <summary>
    /// Inner storage for parameters
    /// </summary>
    protected readonly Dictionary<string, object?> Parameters = new();

    /// <inheritdoc />
    public void Add(KeyValuePair<string, object?> item)
        => Parameters.Add(item.Key, item.Value);

    /// <inheritdoc />
    public void Add(string key, object? value)
        => Parameters.Add(key, value);

    /// <inheritdoc />
    public void Clear()
        => Parameters.Clear();

    /// <inheritdoc />
    public bool Contains(KeyValuePair<string, object?> item)
        => Parameters.Contains(item);

    /// <inheritdoc />
    public bool ContainsKey(string key)
        => Parameters.ContainsKey(key);

    /// <inheritdoc />
    public bool Remove(KeyValuePair<string, object?> item)
        => Parameters.Remove(item.Key);

    /// <inheritdoc />
    public bool Remove(string key)
        => Parameters.Remove(key);

    /// <inheritdoc />
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        => Parameters.TryGetValue(key, out value);

    /// <inheritdoc />
    public void CopyTo(KeyValuePair<string, object?>[] array, int index)
    {
        if (index > array.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (array.Length - index < Parameters.Count)
            throw new ArgumentException(nameof(array));

        foreach (var parameter in Parameters)
            array[index++] = new KeyValuePair<string, object?>(parameter.Key, parameter.Value);
    }

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => Parameters.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => Parameters.GetEnumerator();

    /// <inheritdoc />
    public object? this[string key]
    {
        get => Parameters[key];
        set => Parameters[key] = value;
    }
}

/// <summary>
/// Generic version of <see cref="ComponentOptions"/> which us used by e.g. <see cref="Modals.ModalService"/>
/// to specify type specific parameters while using blazor's way of defining dynamic parameters
/// </summary>
/// <typeparam name="TComponent">Type of the component options should be set for</typeparam>
public sealed class ComponentOptions<TComponent> : ComponentOptions where TComponent : ComponentBase
{
    /// <summary>
    /// Adds an option for a specific property selected by a property/member expression
    /// </summary>
    /// <param name="expression">Property/member expression to select the option</param>
    /// <param name="value">Value to set the option to</param>
    /// <typeparam name="TValue">Type of the value. Will be inferred by the provided expression</typeparam>
    /// <exception cref="ArgumentException">Thrown when an invalid expression is provided</exception>
    public void Add<TValue>(Expression<Func<TComponent, TValue>> expression, TValue value)
    {
        if (expression.Body is not MemberExpression memberExpression)
            throw new ArgumentException("Expression must be a member expression", nameof(expression));
        
        Parameters.Add(memberExpression.Member.Name, value);
    }
}