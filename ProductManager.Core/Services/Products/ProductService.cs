using AutoMapper;
using ProductManager.Core.Models;
using ProductManager.Core.Services.Common;
using ProductManager.Data.Entities;
using ProductManager.Data.UnitOfWork;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductManager.Core.Services.Products
{
    internal class ProductService : CommonService<ProductModel, ProductEntity>,
        IProductService        
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        #region Public methods
        public async Task GenerateProducts(int count)
        {
            var products = new ProductEntity[count];

            var random = new Random();
            var usedBarcodes = new HashSet<string>();
            var usedPLUs = new HashSet<int>();

            for (int i = 0; i < count; i++)
            {
                var product = new ProductEntity();

                product.Name = GenerateRandomString(random.Next(5, 16));
                product.Price = Math.Round(random.Next(100, 5001) / 10.0) * 10;

                // Initialize the barcode
                if (random.NextDouble() < 0.9)
                {
                    string barcode;
                    do
                    {
                        barcode = GenerateRandomString(13);
                    }
                    while (usedBarcodes.Contains(barcode));

                    product.Barcode = barcode;
                    usedBarcodes.Add(product.Barcode);
                }

                // Initialize the PLU
                int plu;
                do
                {
                    plu = random.Next(1, 100000);
                }
                while (usedPLUs.Contains(plu));

                product.PLU = plu;
                usedPLUs.Add(product.PLU);

                // After initializing, add the product into the collection
                products[i] = product;
            }

            // Write the generated records to the database after deleting all existing records
            await _unitOfWork.Repository<ProductEntity>().Truncate();
            await _unitOfWork.Repository<ProductEntity>().AddRange(products);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> Add(ProductModel model)
        {
            var productsTable = _unitOfWork.Repository<ProductEntity>();

            if (model.Barcode != null && await productsTable.Any(x => x.Barcode == model.Barcode))
            {
                throw BadRequest($"A product with the barcode '{model.Barcode}' already exists");
            }

            if (await productsTable.Any(x => x.PLU == model.PLU))
            {
                throw BadRequest($"A product with the PLU '{model.PLU}' already exists");
            }

            model.Price = Math.Round(model.Price / 10, MidpointRounding.AwayFromZero) * 10;

            var entity = _mapper.Map<ProductEntity>(model);

            await productsTable.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Update(ProductModel model)
        {
            if (model == null)
                throw BadRequest($"Model to be updated was null");

            var entity = _mapper.Map<ProductEntity>(model);

            if (entity.Id == default(int))
                throw BadRequest("Model must have an Id for updating");

            var productsTable = _unitOfWork.Repository<ProductEntity>();

            var existingEntity = await productsTable.GetById(entity.Id);
            EnsureExists(existingEntity, $"There's no record with id {entity.Id} to update");

            if (model.Barcode != null && model.Barcode != entity.Barcode && await productsTable.Any(x => x.Barcode == model.Barcode))
            {
                throw BadRequest($"A product with the barcode '{model.Barcode}' already exists");
            }

            if (model.PLU != entity.PLU && await productsTable.Any(x => x.PLU == model.PLU))
            {
                throw BadRequest($"A product with the PLU '{model.PLU}' already exists");
            }

            model.Price = Math.Round(model.Price / 10, MidpointRounding.AwayFromZero) * 10;

            _mapper.Map(entity, existingEntity);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region Private methods
        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }
        #endregion
    }
}
