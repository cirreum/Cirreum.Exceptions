namespace Cirreum.Exceptions;

/// <summary>
/// Contains related details associated with an unhandled exception.
/// </summary>
public sealed record FailureModel {

	/// <summary>
	/// Constructor
	/// </summary>
	public FailureModel() {

	}

	/// <summary>
	/// The name of the property.
	/// </summary>
	public string PropertyName { get; init; } = "";
	/// <summary>
	/// The error message
	/// </summary>
	public string ErrorMessage { get; init; } = "";
	/// <summary>
	/// The property value that caused the failure.
	/// </summary>
	public string? AttemptedValue { get; init; }
	/// <summary>
	/// Custom state associated with the failure.
	/// </summary>
	public string? CustomState { get; init; }
	/// <summary>
	/// Custom severity level associated with the failure.
	/// </summary>
	public FailureSeverity Severity { get; init; }
	/// <summary>
	/// Gets or sets the error code.
	/// </summary>
	public string ErrorCode { get; init; } = "";

}