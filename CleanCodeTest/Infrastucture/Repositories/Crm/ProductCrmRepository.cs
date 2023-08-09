using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Domain.Repositories;
using Microsoft.Xrm.Sdk;
using System;

namespace CleanCodeTest.Infrastucture.Repositories.Crm
{
    internal class ProductCrmRepository : IProductRepository
    {
        IOrganizationService _crmServcie;

        public ProductCrmRepository(IOrganizationService crmServcie)
        {
            _crmServcie = crmServcie;
        }

        public void AddProduct(Product product)
        {
            Entity newProduct = new Entity("product");

            newProduct["name"] = product.name;
            newProduct["productnumber"] = product.productnumber;
            newProduct["defaultuomscheduleid"] = product.defaultuomscheduleid;
            newProduct["defaultuomid"] = product.defaultuomid;
            newProduct["quantitydecimal"] = product.quantitydecimal;
            newProduct["price"] = product.price;
            newProduct["description"] = product.description;

            Guid createdProductId = _crmServcie.Create(newProduct);

            Entity updatedProduct = new Entity("product", createdProductId);
            updatedProduct["statecode"] = product.optionSetValue;
            _crmServcie.Update(updatedProduct);
        }
    }
}
