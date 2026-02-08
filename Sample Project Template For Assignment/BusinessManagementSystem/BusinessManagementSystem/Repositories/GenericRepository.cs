using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusinessManagementSystem.Repositories
{
    public class GenericRepository<T> : IGeneric<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        private string _errorMessage = string.Empty;
        private readonly bool _isDisposed;
        protected readonly ApplicationDBContext _dbContext;
        private IDbContextTransaction _objTran;
        private ResponseDto<T> _responseDto;

        public GenericRepository(ApplicationDBContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
            _responseDto = new ResponseDto<T>();
        }
        public ResponseDto<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProperties = null, bool tracked = false)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                if (tracked)
                {
                    query = _dbSet;
                }
                else
                {
                    query = _dbSet.AsNoTracking();
                }
                query = query.Where(filter);
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                if (query.Count() > 0)
                {
                    _responseDto.StatusCode = HttpStatusCode.OK;
                    _responseDto.Data = query.FirstOrDefault();
                }
                else
                {
                    _responseDto.StatusCode = HttpStatusCode.NotFound;
                    _responseDto.Message = "Not Found";
                }

            }
            catch (Exception ex)
            {

                _responseDto.Message = "Failed due to: " + ex.Message;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _responseDto;
        }
        public  ResponseDto<T> GetSingleOrDefault(string includeProperties = null, bool tracked = false)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                if (!tracked)
                {
                    query = _dbSet.AsNoTracking();
                }
                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                if (query.Count() > 0)
                {
                    _responseDto.StatusCode = HttpStatusCode.OK;
                    _responseDto.Data =  query.SingleOrDefault();
                }
                else
                {
                    _responseDto.StatusCode = HttpStatusCode.NotFound;
                    _responseDto.Message = "Not Found";
                }
            }
            catch (Exception ex)
            {

                _responseDto.Message = "Failed due to: " + ex.Message;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _responseDto;
        }
        public  ResponseDto<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool tracked = false)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                if (filter != null)
                    query = query.Where(filter);



                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                foreach (var item in query)
                {

                    PropertyInfo[] properties = item.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(item, HttpUtility.HtmlEncode(property.GetValue(item)));
                        }
                    }

                }

                if (query.Count() > 0)
                {
                    _responseDto.StatusCode = HttpStatusCode.OK;
                    _responseDto.Datas = query.ToList();
                }

                else
                {
                    _responseDto.StatusCode = HttpStatusCode.NotFound;
                    _responseDto.Message = "Not Found";
                }

            }
            catch (Exception ex)
            {
                _responseDto.Message = "Failed due to: " + ex.Message + ex.InnerException;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _responseDto;
        }
        public  ResponseDto<T> Insert(T entity)
        {
            try
            {
                _dbContext.Database.BeginTransaction();
                _dbSet.Add(entity);
                 _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                _responseDto.Data = entity;
                _responseDto.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                _responseDto.Message = "Failed due to: " + ex.Message+ "Inner Exception:"+ ex.InnerException;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Data = entity;
            }
            return _responseDto;
        }
        public  ResponseDto<T> Update(T entity)
        {
            try
            {
                _dbContext.Database.BeginTransaction();
                _dbSet.Update(entity);
                 _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                _responseDto.StatusCode = HttpStatusCode.OK;
                _responseDto.Data = entity;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                _responseDto.Message = "Failed due to: " + ex.Message;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Data = entity;
            }
            return _responseDto;
        }
        public  ResponseDto<T> Delete(T entity)
        {
            try
            {
                _dbContext.Database.BeginTransaction();
                _dbSet.Remove(entity);
                 _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                _responseDto.StatusCode = HttpStatusCode.OK;
                _responseDto.Data = entity;


            }
            catch (Exception ex)
            {
                _responseDto.Message = "Failed due to: " + ex.Message;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Data = entity;
            }
            return _responseDto;
        }
        public  ResponseDto<T> DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                _dbContext.Database.BeginTransaction();
                _dbSet.RemoveRange(entities);
                 _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                _responseDto.StatusCode = HttpStatusCode.OK;
                _responseDto.Datas = entities.ToList();

            }
            catch (Exception ex)
            {
                _responseDto.Message = "Failed due to: " + ex.Message;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Datas = entities.ToList();
            }
            return _responseDto;
        }
        public  ResponseDto<T> GetById(int id)
        {
            try
            {
                var item =  _dbSet.Find(id);
                if (item != null)
                {
                    _responseDto.StatusCode = HttpStatusCode.OK;
                    _responseDto.Data = item;
                }
                else
                {
                    _responseDto.StatusCode = HttpStatusCode.NotFound;
                    _responseDto.Message = "Not Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = "Failed due to: " + ex.Message;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _responseDto;
        }
    }
}
