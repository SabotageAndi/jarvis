using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using jarvis.common.dtos;
using jarvis.common.dtos.Workflow;
using jarvis.client.common.Actions.ActionCaller;

namespace jarvis.client.worker
{
    public class %TASKNAME% : CompiledTask
    {
        public override int run(List<ParameterDto> parameters)
        {
            %RUNCODE%
        }
    }
}
