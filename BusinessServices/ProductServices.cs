using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BusinessServices
{
	public class ProductServices : IProductServices
	{
		private readonly UnitOfWork _unitOfWork;

		public ProductServices()
		{
			_unitOfWork = new UnitOfWork();
		}

		public int CreateProduct(ProductEntity productEntity)
		{
			using (var scope = new TransactionScope())
			{
				var product = new Product
				{
					ProductName = productEntity.ProductName
				};
				_unitOfWork.ProductRepository.Insert(product);
				_unitOfWork.Save();
				scope.Complete();
				return product.ProductId;

			}
		}

		public bool DeleteProduct(int productId)
		{
			var success = false;
			if (productId > 0)
			{
				using (var scope = new TransactionScope())
				{
					var product = _unitOfWork.ProductRepository.GetByID(productId);
					if (product != null)
					{

						_unitOfWork.ProductRepository.Delete(product);
						_unitOfWork.Save();
						scope.Complete();
						success = true;
					}
				}
			}
			return success;
		}

		public IEnumerable<ProductEntity> GetAllProducts()
		{
			var products = _unitOfWork.ProductRepository.GetAll().ToList();
			if(products.Any())
			{
				Mapper.Initialize(cfg =>
				{
					cfg.CreateMap<Product, ProductEntity>();
				});
				var productEntities = Mapper.Map<List<ProductEntity>>(products);
				return productEntities;
			}
			return null;
		}

		public ProductEntity GetProductById(int productId)
		{
			var product = _unitOfWork.ProductRepository.GetById(productId);
			if(product != null)
			{
				Mapper.Initialize(cfg =>
				{
					cfg.CreateMap<Product, ProductEntity>();
				});
				var productEntity = Mapper.Map<ProductEntity>(product);
				return productEntity;
			}
			return null;
		}

		public bool UpdateProduct(int productId, ProductEntity productEntity)
		{
			var success = false;
			if(productEntity != null)
			{
				using (var scope = new TransactionScope())
				{
					var product = _unitOfWork.ProductRepository.GetById(productId);
					if(product != null)
					{
						product.ProductName = productEntity.ProductName;
						_unitOfWork.ProductRepository.Update(product);
						_unitOfWork.Save();
						scope.Complete();
						success = true;
					}
				}
			}
			return success;
		}
	}
}
