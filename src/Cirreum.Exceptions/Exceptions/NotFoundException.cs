namespace Cirreum.Exceptions;

using System;
using System.Collections.Generic;
using Cirreum;

/// <summary>
/// The application NotFound exception.
/// </summary>
public class NotFoundException(
	params ReadOnlySpan<object> keys
) : Exception(GetMessage(keys)), IErrorState {
	/// <summary>
	/// Gets an array containing all keys in the collection.
	/// </summary>
	public object[] Keys { get; } = keys.ToArray();

	/// <summary>
	/// Exposes the lookup keys as serializable error state so they survive a
	/// <c>Result</c> round-trip onto <c>SurrogateResultException.State</c> (empty keys write nothing).
	/// </summary>
	IReadOnlyDictionary<string, string> IErrorState.State {
		get {
			var state = new Dictionary<string, string>(1);
			if (this.Keys.Length > 0) {
				state["keys"] = string.Join(", ", this.Keys);
			}

			return state;
		}
	}

	private static string GetMessage(ReadOnlySpan<object> keys) => keys.Length switch {
		0 => "Item was not found.",
		1 => $"Item {keys[0]} was not found.",
		2 => $"Items ({keys[0]} and {keys[1]}) were not found.",
		_ => $"Items ({FormatKeys(keys)}) were not found."
	};

	private static string FormatKeys(ReadOnlySpan<object> keys) {
		// "a, b, and c" format
		var items = keys.ToArray();
		return string.Join(", ", items[..^1]) + ", and " + items[^1];
	}

}