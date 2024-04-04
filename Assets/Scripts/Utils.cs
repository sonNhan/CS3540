using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for helper functions
public static class Utils
{
    // Authored by husayt + Yahya Hussien
    // https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp
    // This is a helper for getting the next value of an enum
    public static T GetNext<T>(this T src) where T : Enum
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(typeof(T) + " is not an Enum");

        T[] arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(arr, src) + 1;
        return (arr.Length == j) ? arr[0] : arr[j];
    }

    public static T GetPrev<T>(this T src) where T : Enum
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(typeof(T) + " is not an Enum");

        T[] arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(arr, src) + 1;
        return (arr.Length == j) ? arr[0] : arr[j];
    }
}
