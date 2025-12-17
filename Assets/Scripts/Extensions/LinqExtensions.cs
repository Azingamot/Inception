using System.Collections.Generic;
using System;
using UnityEngine;

public static class LinqExtensions
{
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

		var seenKeys = new HashSet<TKey>();
		foreach (var element in source)
		{
			if (seenKeys.Add(keySelector(element)))
				yield return element;
		}
	}
}