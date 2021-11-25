// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Razor.Application.MappingRules.DTOs;

namespace CleanArchitecture.Razor.Application.MappingRules.Commands.AddEdit;

public class AddEditMappingRuleCommand : MappingRuleDto, IRequest<Result<int>>, IMapFrom<MappingRule>
{
    public UploadRequest UploadRequest { get; set; }
}

public class AddEditMappingRuleCommandHandler : IRequestHandler<AddEditMappingRuleCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditMappingRuleCommandHandler> _localizer;
    private readonly IUploadService _uploadService;

    public AddEditMappingRuleCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditMappingRuleCommandHandler> localizer,
            IUploadService uploadService,
            IMapper mapper
            )
    {
        _context = context;
        _localizer = localizer;
        _uploadService = uploadService;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditMappingRuleCommand request, CancellationToken cancellationToken)
    {
     
        

        if (request.Id > 0)
        {
            var item = await _context.MappingRules.FindAsync(new object[] { request.Id }, cancellationToken);
            item = _mapper.Map(request, item);
            if (request.UploadRequest != null && request.UploadRequest.Data != null)
            {
                var result = await _uploadService.UploadAsync(request.UploadRequest);
                item.TemplateFile = result;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<MappingRule>(request);
            if (request.UploadRequest != null && request.UploadRequest.Data != null)
            {
                var result = await _uploadService.UploadAsync(request.UploadRequest);
                item.TemplateFile = result;
            }
            _context.MappingRules.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}
