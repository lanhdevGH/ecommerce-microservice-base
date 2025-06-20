﻿using Common.Logging;
using MediatR;
using Serilog;
using System.Diagnostics;
using ILogger = Serilog.ILogger;


namespace Ordering.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ICustomLogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

    public PerformanceBehaviour(ICustomLogger<PerformanceBehaviour<TRequest, TResponse>> logger)
    {
        _timer = new Stopwatch();
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500) return response;

        var requestName = typeof(TRequest).Name;
        _logger.Warn($"Application Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) {request}");

        return response;
    }
}    
