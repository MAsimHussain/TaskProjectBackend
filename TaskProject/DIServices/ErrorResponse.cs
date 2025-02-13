namespace TaskProject.UI.DIServices
{
	public class ErrorResponse
	{
		public int StatusCode { get; set; }
		public string StatusText { get; set; }
		public string Message { get; set; }
		public string Detail { get; set; }  // Optional, for more detailed error info
		public DateTime Timestamp { get; set; }
		public ErrorResponse()
		{
		}
		// Constructor to quickly populate the error
		public ErrorResponse(int statusCode, string statusText, string message, string detail = null)
		{
			StatusCode = statusCode;
			StatusText = statusText;
			Message = message;
			Detail = detail;
			Timestamp = DateTime.UtcNow;
		}

		
	}

}
