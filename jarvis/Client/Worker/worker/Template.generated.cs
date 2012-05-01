using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using jarvis.common.dtos;
using jarvis.common.dtos.Workflow;
using jarvis.client.common.Actions.ActionCaller;
using Ninject;

namespace jarvis.client.worker
{
    public class %TASKNAME% : CompiledTask
    {
        %INJECTEDPROPERTIES%

        public override int run(List<ParameterDto> parameters)
        {
            %RUNCODE%
        }
    }
}
