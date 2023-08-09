using CleanCodeTest.Domain.Entities;


namespace CleanCodeTest.Domain.Repositories
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
    }
}
