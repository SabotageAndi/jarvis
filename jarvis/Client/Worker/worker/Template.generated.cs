using System;
using System.Collections;
using System.Collections.Generic;
using jarvis.common.dtos.Workflow;

namespace jarvis.client.worker
{
    public class %TASKNAME% : ICompiledTask
    {
        public int run(List<ParameterDto> parameters)
        {
            %RUNCODE%
        }
    }
}
