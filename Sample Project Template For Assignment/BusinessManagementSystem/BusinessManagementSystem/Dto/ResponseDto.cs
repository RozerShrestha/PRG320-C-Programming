using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BusinessManagementSystem.Dto
{
    public class ResponseDto<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<T> Datas { get; set; } = [];
        public dynamic Dynamic_Datas { get; set; }

        public ResponseDto()
        {
            StatusCode = HttpStatusCode.OK;
            Message = "Success";
        }
    }
}
