﻿using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class PutDeviceValidator : AbstractValidator<PutDeviceDTO>
{
    public PutDeviceValidator()
    {
        RuleFor(d => d.Id).NotEmpty().GreaterThan(0);
        RuleFor(d => d.SerialNumber).NotEmpty();
        RuleFor(d => d.DeviceName).NotEmpty();
        RuleFor(d => d.Status).Matches("På lager|I brug|Defekt");
        RuleFor(d => d.RequestValue).Matches("IkkeSendt|Sendt|Accepteret");
    }
}