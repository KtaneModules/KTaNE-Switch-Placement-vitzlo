using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public static class Ut
{

    /// <summary>
    /// Returns the number of times a given <typeparamref name="T"/> <paramref name="item"/> appears in <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection"> The collection from which count is taken from.</param>
    /// <param name="item">The item that is counted.</param>
	public static int CountOf<T>(this IEnumerable<T> collection, T item)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");
        if (item == null)
            throw new ArgumentNullException("item");
        return collection.Count(x => x.Equals(item));
    }

    /// <summary>
    ///     Returns the entries of <paramref name="collection"/> which occur a total of <paramref name="count"/> times within <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection">The collection which is searched.</param>
    /// <param name="count">The target number of occurences.</param>
    public static IEnumerable<T> WhereCountIs<T>(this IEnumerable<T> collection, int count)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");
        return collection.Where(x => collection.CountOf(x) == count);
    }

    /// <summary>
    /// Determines whether or not <paramref name="collection"/> has any entries which are equal.
    /// </summary>
    /// <param name="collection"></param>
    public static bool HasDuplicates<T>(this IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");
        return collection.Distinct().Count() != collection.Count();
    }

    /// <summary>
    ///     Returns a random bool, either true or false.
    /// </summary>
    public static bool RandBool()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

    /// <summary>
    ///     Returns the item in <paramref name="collection"/> which has the highest value when compared using the given <paramref name="comparer"/>.
    ///     If multiple items have this value, the first will be chosen.
    /// </summary>
    /// <param name="collection">The source from which values are taken.</param>
    /// <param name="comparer">The function applied to the values.</param>
    public static T MaxBy<T>(this IEnumerable<T> collection, Func<T, int> comparer)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");
        if (collection.Count() == 0)
            throw new InvalidOperationException("Max operation cannot be performed on an empty set");
        var ordered = collection.OrderBy(comparer);
        return collection.First(x => x.Equals(ordered.Last()));
    }

    /// <summary>
    ///     Returns the item in <paramref name="collection"/> which has the lowest value when compared using the given <paramref name="comparer"/>.
    ///     If multiple items have this value, the first will be chosen.
    /// </summary>
    /// <param name="collection">The source from which items are taken.</param>
    /// <param name="comparer">The function applied to the items to obtain their values.</param>
    public static T MinBy<T>(this IEnumerable<T> collection, Func<T, int> comparer)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");

        if (collection.Count() == 0)
            throw new InvalidOperationException("Min operation cannot be performed on an empty set");
        var ordered = collection.OrderBy(comparer);
        return collection.First(x => x.Equals(ordered.First()));
    }

    /// <summary>
    ///     Converts an integer number <paramref name="input"/> into a string representing its ordinal form.
    /// </summary>
    /// <param name="input">The number to be converted.</param>
    /// <returns>The number's ordinal form.</returns>
    public static string Ordinal(int input)
    {
        if (input < 0)
            return string.Format("({0})th", input);
        if ((input % 100).EqualsAny(11, 12, 13))
            return input + "th";
        else switch (input % 10)
            {
                case 1: return input + "st";
                case 2: return input + "nd";
                case 3: return input + "rd";
                default: return input + "th";
            }
    }

    /// <summary>
    /// Chooses a random item from <paramref name="collection"/> which returns true when fed into <paramref name="condition"/>.
    /// </summary>
    /// <param name="collection">The source from which items are taken from.</param>
    /// <param name="condition">The function which chosen items must obey.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static T PickRandom<T>(this IEnumerable<T> collection, Func<T, bool> condition)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");
        if (collection.Count() == 0)
            throw new InvalidOperationException("Cannot pick an element from an empty set.");
        IEnumerable<T> filteredCollection = collection.Where(condition);
        if (filteredCollection.Count() == 0)
            throw new InvalidOperationException("Filtered set contains no items.");
        return filteredCollection.ElementAt(Rnd.Range(0, filteredCollection.Count()));
    }
    /// <summary>
    /// Chooses a random item from <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection">The source from which items are taken from.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static T PickRandom<T>(this IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");
        if (collection.Count() == 0)
            throw new InvalidOperationException("Cannot pick an element from an empty set.");
        return collection.ElementAt(Rnd.Range(0, collection.Count()));
    }

    /// <summary>
    /// Takes a dictionary and creates a new dictionary in which the keys of the original are the values of the output, and vice versa.
    /// </summary>
    /// <typeparam name="TKey">The type of the original dictionary's keys.</typeparam>
    /// <typeparam name="TVal">The type of the original dictionary's values.</typeparam>
    /// <param name="dict">The dictionary to be inverted.</param>
    /// <returns>A new dictionary with the keys and values swapped.</returns>
    /// <exception cref="InvalidOperationException"></exception>"
    public static Dictionary<TVal, TKey> InvertDictionary<TKey, TVal>(this Dictionary<TKey, TVal> dict)
    {
        if (dict == null)
            throw new ArgumentException("dict");
        var output = new Dictionary<TVal, TKey>();
        if (dict.Select(x => x.Value).HasDuplicates())
            throw new InvalidOperationException("Inputted dictionary has multiple entries with the same value, making it impossible to invert.");
        foreach (var pair in dict)
            output.Add(pair.Value, pair.Key);
        return output;
    }

    /// <summary>
    /// Sets a given component of <paramref name="vect"/> to <paramref name="value"/>. <paramref name="component"/> must be equal to X, Y, Z, x, y or z.
    /// </summary>
    /// <param name="vect">The vector to be modified.</param>
    /// <param name="component">The component of the vector to be modified. Must be X, Y, Z, x, y or z.</param>
    /// <param name="value">The value to set the component to.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void SetComponent(this Vector3 vect, char component, float value)
    {
        if (vect == null)
            throw new ArgumentNullException("vect");
        switch (component)
        {
            case 'X':
            case 'x':
                vect.Set(value, vect.y, vect.z);
                break;
            case 'Y':
            case 'y':
                vect.Set(vect.x, value, vect.z);
                break;
            case 'Z':
            case 'z':
                vect.Set(vect.x, vect.y, value);
                break;
            default: throw new ArgumentOutOfRangeException("component", string.Format("Given component name ({0}) does match any of the components of the vector (x/y/z).", component));
        }
    }
    /// <summary>
    ///     Linearly interpolates from <paramref name="start"/> to <paramref name="end"/> while <paramref name="t"/> is less than 0.5, and then interpolates backwards from <paramref name="end"/> to <paramref name="start"/> while <paramref name="t"/> is greater than 0.5.
    /// </summary>
    /// <param name="start">The initial value to be interpolated. This value will be equal to the output when <paramref name="t"/> == 0 or <paramref name="t"/> == 1.</param>
    /// <param name="end">The end value to be interpolated to. This value will be equal to the output when <paramref name="t"/></param>
    /// <param name="t">The interpolant; within 0.0 to 1.0.</param>
    /// <returns>The result of the interpolation.</returns>
    public static float InOutLerp(float start, float end, float t)
    {
        if (t < 0)
            t = 0;
        if (t > 1)
            t = 1;
        if (t <= 0.5)
            return Mathf.Lerp(start, end, t * 2);
        else return Mathf.Lerp(end, start, t * 2 - 1);
    }
    /// <summary>
    ///     Returns the value of <paramref name="str"/> with its first character capitalized
    /// </summary>
    /// <param name="str">The string to have its first letter capitalized.</param>
    public static string CapitalizeFirst(this string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }

    /// <summary>
    ///     Takes <paramref name="str"/> and inserts spaces before all capital letters other than the first.
    /// </summary>
    /// <param name="str">The string to be split</param>
    /// <returns></returns>
    public static string SplitCaps(this string str)
    {
        string output = "";
        foreach (char letter in str)
        {
            if (char.IsUpper(letter))
                output += ' ';
            output += letter;
        }
        return output.TrimStart(' ');
    }

    /// <summary>
    ///     Decomposes a rectangular grid into multiple rows and sends each as a log message such that the logging is compatible with the Logfile Analyzer.
    /// </summary>
    /// <param name="loggingTag">The module's display name and moduleId, formatted as "[{displayName} #{moduleId}]"</param>
    /// <param name="grid">The grid to be logged. The grid must be rectangular.</param>
    /// <param name="height">The number of rows in the grid. The output will consist of this many messages.</param>
    /// <param name="width">The number of items per row of the grid.</param>
    /// <param name="separator">A string to separate each entry of the grid</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when the stated width or height of the grid is 0 or fewer.</exception>
    /// <exception cref="ArgumentNullException">Occurs when the entered grid is null.</exception>
    public static void LogGrid(string loggingTag, object[] grid, int height, int width, string separator = " ")
    {
        if (grid == null)
            throw new ArgumentNullException("Received grid has a null value");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(string.Format("Unexpected value of height, expected a positive value, received {0}.", height));
        if (width <= 0)
            throw new ArgumentOutOfRangeException(string.Format("Unexpected value of width, expected a positive value, received {0}.", width));
        for (int row = 0; row < height; row++)
            Debug.Log(loggingTag + " " + string.Join(separator, Enumerable.Range(row * height, width).Select(x => grid[x].ToString()).ToArray()));
    }
    /// <summary>
    ///     Decomposes a rectangular grid of integers into multiple rows and sends each as a log message such that the logging is compatible with the Logfile Analyzer.<br></br>Each integer is indexed into <paramref name="itemSet"/> to obtain a string to represent each integer.
    /// </summary>
    /// <param name="loggingTag">The module's display name and moduleId, formatted as "[{displayName} #{moduleId}]"</param>
    /// <param name="grid">The integer grid to be logged. The grid must be rectangular.</param>
    /// <param name="height">The number of rows in the grid. The output will consist of this many messages.</param>
    /// <param name="width">The number of items per row of the grid.</param>
    /// <param name="itemSet">The set of objects to index each entry of <paramref name="grid"/> into.</param>
    /// <param name="separator">A string to separate each entry of the grid</param>
    /// <param name="offset">A value added to each value of <paramref name="grid"/> before it is indexed into <paramref name="itemSet"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when the stated width or height of the grid is 0 or fewer.</exception>
    /// <exception cref="ArgumentNullException">Occurs when the entered grid is null.</exception>
    public static void LogIntegerGrid(string loggingTag, int[] grid, int height, int width, object[] itemSet, string separator = " ", int offset = 0)
    {
        if (grid == null)
            throw new ArgumentNullException("Received grid has a null value");
        if (height <= 0)
            throw new ArgumentOutOfRangeException(string.Format("Unexpected value of height, expected a positive value, received {0}.", height));
        if (width <= 0)
            throw new ArgumentOutOfRangeException(string.Format("Unexpected value of width, expected a positive value, received {0}.", width));
        grid = grid.Select(x => x + offset).ToArray();
        if (grid.Any(x => x < 0))
            throw new IndexOutOfRangeException(string.Format("Unexpected value {0} found within the grid after applying offset. Values cannot be less than 0.",
                grid.First(x => x < 0)));
        if (grid.Any(x => x >= itemSet.Count()))
            throw new IndexOutOfRangeException(string.Format("Unexpected value {0} found within the grid after applying offset. Values cannot be greater than 1 fewer than the length of charSet ({1}).",
                grid.First(x => x >= itemSet.Count()), itemSet.Count()));

        for (int row = 0; row < height; row++)
            Debug.Log(loggingTag + " " + string.Join(separator, Enumerable.Range(row * height, width).Select(x => itemSet[grid[x] + offset].ToString()).ToArray()));
    }
    /// <summary>
    ///     Takes an IEnumerable <paramref name="collection"/> and cyclically shifts its entries to the right.<br></br>If the value of <paramref name="rightShift"/> is less than 0, <paramref name="collection"/> will be shifted to the left by its absolute value. 
    /// </summary>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <typeparam name="TElem">The type of the collection's items.</typeparam>
    /// <param name="collection">The collection to be shifted.</param>
    /// <param name="rightShift">The number of places to shift the collection right by. If this value is negative, the collection will be shifted <em>left</em> by its absolute value</param>
    /// <returns></returns>
    public static T ShiftRight<T, TElem>(this T collection, int rightShift) where T : IEnumerable<TElem>
    {
        if (collection == null)
            throw new ArgumentNullException("collection unexpected null value");
        if (rightShift < 0)
            return (T)collection.Skip(-1 * rightShift).Concat(collection.Take(-1 * rightShift));
        else return (T)collection.Skip(collection.Count() - rightShift).Concat(collection.Take(rightShift));
    }

    /// <summary>
    ///     Splits an IEnumerable into several smaller IEnumerables with the same size.<br></br>If <paramref name="ignoreThrow"/> is true, the method will allow the last IEnumerable to have a size smaller than the <paramref name="groupSize"/>.
    /// </summary>
    /// <param name="collection">The collection to be split.</param>
    /// <param name="groupSize">The size of each resulting IEnumerable</param>
    /// <param name="ignoreThrow">If this value is true, a check for all groups being equal will be bypassed, allowing the final group to have a value less than the <paramref name="groupSize"/></param>
    public static IEnumerable<IEnumerable<T>> SplitCount<T>(this IEnumerable<T> collection, int groupSize, bool ignoreThrow = false)
    {
        if (collection == null)
            throw new ArgumentNullException("collection unexpected null value");
        if (groupSize <= 0)
            throw new ArgumentOutOfRangeException("Group size must be positive, received value " + groupSize);
        if (collection.Count() % groupSize != 0 && !ignoreThrow)
            throw new ArgumentException("Group size is not a multiple of the collection's count.");
        int fullPartitionCount = (collection.Count() / groupSize);
        for (int i = 0; i < fullPartitionCount; i++)
            yield return collection.Skip(groupSize * i).Take(groupSize);
        yield return collection.Skip(fullPartitionCount * groupSize);
    }
}
