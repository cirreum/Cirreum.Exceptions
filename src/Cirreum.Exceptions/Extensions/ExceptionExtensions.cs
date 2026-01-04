namespace System;

/// <summary>
/// Extension methods for <see cref="Exception"/>
/// </summary>
public static class ExceptionExtensions {

	/// <summary>
	/// Determines if this exception is considered "fatal"
	/// </summary>
	/// <param name="ex">The exception to check.</param>
	/// <returns><see langword="true"/> if the exception is <see cref="OutOfMemoryException"/>
	/// or <see cref="StackOverflowException"/> or <see cref="ThreadAbortException"/></returns>
	public static bool IsFatal<TException>(this TException ex) where TException : Exception =>
		ex is OutOfMemoryException or
		StackOverflowException or
		ThreadAbortException;

}