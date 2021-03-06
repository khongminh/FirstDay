﻿using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ProductController : ApiController
    {
		private readonly IProductServices _productServices;

		public ProductController()
		{
			_productServices = new ProductServices();
		}

        // GET: api/Product
        public HttpResponseMessage Get()
        {
			var products = _productServices.GetAllProducts();
			if(products != null)
			{
				var productEntities = products as List<ProductEntity> ?? products.ToList();
				if (productEntities.Any())
					return Request.CreateResponse(HttpStatusCode.OK, productEntities);
			}
			return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product not found");
        }

        // GET: api/Product/5
        public HttpResponseMessage Get(int id)
        {
			ar product = _productServices.GetProductById(id);
			if (product != null)
				return Request.CreateResponse(HttpStatusCode.OK, product);
			return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No product found for this id");
		}

        // POST: api/Product
        public int Post([FromBody]ProductEntity productEntity)
        {
			return _productServices.CreateProduct(productEntity);
		}

        // PUT: api/Product/5
        public bool Put(int id, [FromBody]ProductEntity productEntity)
        {
			if (id > 0)
			{
				return _productServices.UpdateProduct(id, productEntity);
			}
			return false;
		}

        // DELETE: api/Product/5
        public bool Delete(int id)
        {
			if(id > 0)
			{
				return _productServices.DeleteProduct(id);
			}
			return false;
        }
    }
}
