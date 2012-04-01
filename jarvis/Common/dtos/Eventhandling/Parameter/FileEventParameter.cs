using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.common.dtos.Eventhandling.Parameter
{
    public class FileEventParameterDto : EventParameterDto
    {
        public string Filename { get; set; }
        public string Path { get; set; }
    }
}
