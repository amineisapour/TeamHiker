﻿namespace LoggingMicroservice.Api.Controllers
{
	[Microsoft.AspNetCore.Mvc.ApiController]
	[Microsoft.AspNetCore.Mvc.Route(template: Infrastructure.Constant.Router.Controller)]
	public class LogsController : Infrastructure.ControllerBase
	{
		public LogsController(MediatR.IMediator mediator) : base(mediator: mediator)
		{
		}

        #region Post (Create Log)
		[Microsoft.AspNetCore.Mvc.HttpPost]
		[Microsoft.AspNetCore.Mvc.ProducesResponseType(type: typeof(FluentResults.Result<System.Guid>), statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
		[Microsoft.AspNetCore.Mvc.ProducesResponseType(type: typeof(FluentResults.Result), statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
		public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Post([Microsoft.AspNetCore.Mvc.FromBody] Application.Logs.Commands.CreateLogCommand request)
		{
			var result =
				await Mediator.Send(request);

			return FluentResult(result: result);
		}
		#endregion /Post (Create Log)

		#region Get (Get Some Logs)
		[Microsoft.AspNetCore.Mvc.HttpGet(template: "{count?}")]

		[Microsoft.AspNetCore.Mvc.ProducesResponseType
			(type: typeof(FluentResults.Result<System.Collections.Generic.IList<Persistence.ViewModels.GetLogsQueryResponseViewModel>>),
			statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]

		[Microsoft.AspNetCore.Mvc.ProducesResponseType
			(type: typeof(FluentResults.Result),
			statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest)]
		public
			async
			System.Threading.Tasks.Task
			<Microsoft.AspNetCore.Mvc.IActionResult>
			Get([Microsoft.AspNetCore.Mvc.FromRoute] int? count)
		{
			var request =
				new Application.Logs.Queries.GetLogsQuery
				{
					Count = count,
				};

			var result =
				await Mediator.Send(request);

			return FluentResult(result: result);
		}
		#endregion /Get (Get Some Logs)
	}
}
