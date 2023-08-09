using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeTest.Domain.UseCases
{
    public class AddProduct
    {
        IProductRepository _repository;
        public AddProduct(IProductRepository productRepository) 
        {
            _repository = productRepository;
        }

        public void invoke(Product product)
        {
            _repository.AddProduct(product);
        }
    }
}
