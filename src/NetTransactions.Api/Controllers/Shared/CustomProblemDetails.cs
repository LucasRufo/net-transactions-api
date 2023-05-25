using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;

namespace NetTransactions.Api.Controllers.Shared;

public class CustomProblemDetails : ProblemDetails
{
	[JsonPropertyName("errors")]
	public IList<CustomProblemDetailsError> Errors { get; set; } = new List<CustomProblemDetailsError>();

	public CustomProblemDetails()
	{
	}

	public CustomProblemDetails(
		HttpStatusCode statusCode, 
		string path,
        IList<CustomProblemDetailsError> errors)
	{
		Title = statusCode switch
		{
			HttpStatusCode.BadRequest => "One or more validations errors occurred.",
			HttpStatusCode.InternalServerError => "Internal server error.",
			_ => "An error occurred."
		};
		Status = (int)statusCode;
        Detail = "An error occurred. Check 'errors' for details.";
		Errors = errors;
        Instance = path;
        Type = null;
	}
}

public class CustomProblemDetailsError
{
	public string Property { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;

	public CustomProblemDetailsError()
	{
	}

	public CustomProblemDetailsError(string propertyName, string errorMessage)
	{
        Property = propertyName;
        Message = errorMessage;
	}
}
