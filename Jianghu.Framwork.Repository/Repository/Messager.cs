using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jianghu.Framwork.Repository.Repository
{
    public class Messager<T> where T : class ,new()
    {
        public T Model { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
