﻿using SharedKernel;

namespace Bike4Me.FunctionalTests.Contracts;

public sealed class CustomProblemDetails
{
    public string Type { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Status { get; set; }

    public string Detail { get; set; } = string.Empty;

    public List<Error> Errors { get; set; } = [];
}