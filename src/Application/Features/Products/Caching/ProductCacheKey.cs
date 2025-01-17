﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace CleanArchitecture.Razor.Application.Features.Products.Caching;

    public static class ProductCacheKey
    {
        public const string GetAllCacheKey = "all-Products";
        public static string GetPagtionCacheKey(string parameters) {
            return "ProductsWithPaginationQuery,{parameters}";
        }
    }

