using DataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.UnitOfWork
{
	public class UnitOfWork : IDisposable
	{
		private WebApiDbEntities _context = null;
		private GenericRepository<User> _userRepository;
		private GenericRepository<Product> _productRepository;
		private GenericRepository<Token> _tokenRepository;

		public UnitOfWork()
		{
			_context = new WebApiDbEntities();
		}

		public GenericRepository<Product> ProductRepository
		{
			get
			{
				if (this._productRepository == null)
					this._productRepository = new GenericRepository<Product>(_context);
				return _productRepository;

			}
		}

		public GenericRepository<User> UserRepository
		{
			get
			{
				if (this._userRepository == null)
					this._userRepository = new GenericRepository<User>(_context);
				return _userRepository;
			}
		}

		public GenericRepository<Token> TokenRepository
		{
			get
			{
				if (this._tokenRepository == null)
					this._tokenRepository = new GenericRepository<Token>(_context);
				return _tokenRepository;
			}
		}

		public void Save()
		{
			try
			{
				_context.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				var outputLines = new List<string>();
				foreach (var eve in e.EntityValidationErrors)
				{
					outputLines.Add(string.Format(
						"{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
						eve.Entry.Entity.GetType().Name, eve.Entry.State));
					foreach (var ve in eve.ValidationErrors)
					{
						outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
					}
				}
				System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

				throw e;
			}

		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if(!this.disposed)
			{
				if(disposing)
				{
					Debug.WriteLine("UnitOfWork is being disposed ");
					_context.Dispose();
				}
				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
}
