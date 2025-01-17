// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Customers.DTOs;
using CleanArchitecture.Razor.Application.Common.Specification;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using CleanArchitecture.Razor.Application.Customers.Caching;

namespace CleanArchitecture.Razor.Application.Customers.Queries.PaginationQuery
{
    public class CustomersByMeQueryQuery : PaginationRequest,IRequest<PaginatedData<CustomerDto>>, ICacheable
    {
        public string UserId { get; set; }


        public string CacheKey => $"CustomersByMeQueryQuery,userid:{UserId},{this.ToString()}";

        public MemoryCacheEntryOptions Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(CustomerCacheTokenSource.ResetCacheToken.Token));
    }
    public class ByMeCustomersQueryHandler : IRequestHandler<CustomersByMeQueryQuery, PaginatedData<CustomerDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ByMeCustomersQueryHandler(
        
            IApplicationDbContext context,
            IMapper mapper
            )
        {
    
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedData<CustomerDto>> Handle(CustomersByMeQueryQuery request, CancellationToken cancellationToken)
        {
            var filters = PredicateBuilder.FromFilter<Customer>(request.FilterRules);
            var data = await _context.Customers.Specify(new CustomerByMeQuerySpec(request.UserId))
                .Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);

            return data;
        }

        public class CustomerByMeQuerySpec : Specification<Customer>
        {
            public CustomerByMeQuerySpec(string userId)
            {
                Criteria = p => p.CreatedBy == userId;
            }

         
        }
    }
}
