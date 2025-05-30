﻿using MediatR;
using SharedKernel;

namespace Bike4Me.Application.Rentals.Commands;

public record CreateRentalCommand(
    Guid BikeId,
    Guid CourierId,
    int RentalPlanDays,
    DateTime RentalStartDate,
    DateTime RentalEndDate,
    DateTime ExpectedReturnDate) : IRequest<Result<Guid>>;