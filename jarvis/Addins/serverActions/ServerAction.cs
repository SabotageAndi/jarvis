﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;
using jarvis.server.repositories;

namespace jarvis.addins.serverActions
{
    public abstract class ServerAction
    {
        private Func<IKernel> _kernel;

        protected Func<IKernel> Kernel
        {
            get { return _kernel; }
        }

        [Inject]
        public IClientRepository ClientRepository { get; set; }

        public abstract string ActionGroup { get; }

        public abstract ActionResultDto Execute(ActionDto actionDto);

        public abstract bool CanExecute(ActionDto actionDto);

        protected  ParameterDto GetParameter(ActionDto actionDto, string category, string name)
        {
            var result = actionDto.Parameters.Where(p => p.Category == category && p.Name == name).SingleOrDefault();
            if (result == null)
            {
                throw new ParameterNotFoundException();
            }

            return result;
        }

        public virtual void Init(Func<IKernel> kernel)
        {
            _kernel = kernel;
        }
    }

    public class ParameterNotFoundException : Exception
    {
    }
}
