// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.FieldValueMappings.Commands.Create;

public class CreateFieldValueMappingCommandValidator : AbstractValidator<CreateFieldValueMappingCommand>
{
    public CreateFieldValueMappingCommandValidator()
    {
        RuleFor(v => v.FieldName).MaximumLength(256)
          .NotEmpty();
        RuleFor(v => v.Target).MaximumLength(256)
          .NotEmpty();
        RuleFor(v => v.Legacy1)
              .MaximumLength(256)
          .NotEmpty();
        RuleFor(v => v.ObjectFieldId)
             .GreaterThan(0);
    }
}
