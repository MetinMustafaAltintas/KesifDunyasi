﻿using Project.BLL.Managers.Abstracts;
using Project.BLL.ServiceInjections;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class ProductAndProductAttributeManager : BaseManager<ProductAndProductAttribute> , IProductAndProductAttributeManager
    {
        readonly IProductAndProductAttributeRepository _pPARep;
        public ProductAndProductAttributeManager(IProductAndProductAttributeRepository pPARep) : base(pPARep)
        {
            _pPARep = pPARep;
        }
        public override string Destroy(ProductAndProductAttribute item)
        {
            _pPARep.Destroy(item);
            return "Veri basarıyla yok edildi";
        }

        public override string Add(ProductAndProductAttribute item)
        {
            item.Value = item.Value.ToTitleCase();
            return base.Add(item);
        }
        public override Task AddAsync(ProductAndProductAttribute item)
        {
            item.Value = item.Value.ToTitleCase();
            return base.AddAsync(item);
        }
        public override Task UpdateAsync(ProductAndProductAttribute item)
        {
            item.Value = item.Value.ToTitleCase();
            return base.UpdateAsync(item);
        }
        public override void Updated(ProductAndProductAttribute item, ProductAndProductAttribute originalEntity)
        {
            item.Value = item.Value.ToTitleCase();
            base.Updated(item, originalEntity);
        }
    }
}
