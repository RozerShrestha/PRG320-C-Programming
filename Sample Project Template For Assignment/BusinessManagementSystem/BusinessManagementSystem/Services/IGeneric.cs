using BusinessManagementSystem.Dto;
using System.Linq.Expressions;

namespace BusinessManagementSystem.Services
{
    public interface IGeneric<T> where T : class
    {
        public ResponseDto<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProperties = null, bool tracked = false);
        public ResponseDto<T> GetSingleOrDefault(string includeProperties = null, bool tracked = false);
        public ResponseDto<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool tracked=false);
        public ResponseDto<T> GetById(int id);
        public ResponseDto<T> Insert(T Entity);
        public ResponseDto<T> Update(T Entity);
        public ResponseDto<T> Delete(T entity);
        public ResponseDto<T> DeleteRange(IEnumerable<T> entities);
    }
}
