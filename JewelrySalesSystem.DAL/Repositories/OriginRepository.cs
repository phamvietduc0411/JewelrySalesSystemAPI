﻿using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.Extensions.Logging;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class OriginRepository : GenericRepository<Origin>, IOriginRepository
    {
        public OriginRepository(JewelryDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
