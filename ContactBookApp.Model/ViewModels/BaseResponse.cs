using Azure;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Model.ViewModels
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public int ResponseCode { get; set; }
        public string Message { get; set; }


        public BaseResponse<T> Success(string message, int statusCode, T data)
        {
            return new BaseResponse<T>
            {
                Message = message,
                ResponseCode = statusCode,
                Data = data
            };
        }

        public BaseResponse<T> Fialed(string message, int statusCode)
        {
            return new BaseResponse<T>
            {
                Message = message,
                ResponseCode = statusCode,
            };
        }

        public BaseResponse<T> Fialed(string message)
        {
            return new BaseResponse<T>
            {
                Message = message,
            };
        }

        public BaseResponse<T> Success(string message, int statusCode)
        {
            return new BaseResponse<T>
            {
                Message = message,
                ResponseCode = statusCode,
            };
        }
    }
}
