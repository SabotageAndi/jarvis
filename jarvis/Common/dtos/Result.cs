using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.common.dtos
{
    [DataContract]
    public class ResultDto<T>
    {
        public ResultDto(T value)
        {
            Result = value;
        }

        public ResultDto()
        {
            Result = default(T);
        }

        [DataMember]
        public T Result { get; set; }
    }
}
