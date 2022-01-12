using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Extensions
{
	public static class CSharpExtensions
	{
		public static IEnumerable<T> Clone<T>(this IEnumerable<T> source)
		{
			T[] destination = new T[source.Count()];

			Array.Copy(source.ToArray(), destination, source.Count());

			return destination;
		}
	}
}
