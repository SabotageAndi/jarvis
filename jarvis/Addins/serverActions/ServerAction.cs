using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;
using jarvis.server.common.Database;
using jarvis.server.repositories;
using log4net;

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
        public ILog Log { get; set; }

        [Inject]
        public IClientRepository ClientRepository { get; set; }

        public abstract string ActionGroup { get; }

        public ActionResultDto Execute(ITransactionScope transactionScope, ActionDto actionDto)
        {
            try
            {
                return ExecuteAction(transactionScope, actionDto);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Error at executing server action {0}: {1}", this.GetType().Name, e);
            }

            return null;
        }

        protected abstract ActionResultDto ExecuteAction(ITransactionScope transactionScope, ActionDto actionDto);

        public bool CanExecute(ITransactionScope transactionScope, ActionDto actionDto)
        {
            try
            {
                return CanExecuteAction(transactionScope, actionDto);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Error at checking executing at server action {0}: {1}", this.GetType().Name, e);
            }

            return false;
        }

        protected abstract bool CanExecuteAction(ITransactionScope transactionScope, ActionDto actionDto);

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
