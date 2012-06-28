using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Util;
using Ninject;
using jarvis.addins.serverActions;
using jarvis.common.dtos.Actionhandling;
using jarvis.server.common.Database;

namespace jarvis.addins.karma.server
{
    public class KarmaServerAction : ServerAction
    {
        private KarmaRepository _karmaRepository;

        public KarmaServerAction()
        {
        }

        public override string ActionGroup
        {
            get { return "Karma"; }
        }

        public override void Init(Func<IKernel> kernel)
        {
            base.Init(kernel);

            _karmaRepository = new KarmaRepository();
        }

        protected override bool CanExecuteAction(ITransactionScope transactionScope, ActionDto actionDto)
        {
            return actionDto.ActionGroup == "Karma";
        }

        protected override ActionResultDto ExecuteAction(ITransactionScope transactionScope, ActionDto actionDto)
        {
            switch (actionDto.Action)
            {
                case "Increase":
                    return ChangeKarma(transactionScope, actionDto, 1);
                case "Decrease":
                    return ChangeKarma(transactionScope, actionDto, -1);
                case "GetKarma":
                    return GetKarma(transactionScope);
                case "Stats":
                    return GetStats(transactionScope);

            }

            throw new ArgumentOutOfRangeException();
        }

        private ActionResultDto GetKarma(ITransactionScope transactionScope)
        {
            var karmaEntities = _karmaRepository.GetAll(transactionScope);

            Decimal stats = 0;

            if (karmaEntities.Any())
            {
                stats = Decimal.Divide(karmaEntities.Sum(k => k.KarmaValue), karmaEntities.Count());

            }

            return new ActionResultDto() {Data = stats.ToString()};
        }

        private ActionResultDto GetStats(ITransactionScope transactionScope)
        {
            var karmaEntities = _karmaRepository.GetAll(transactionScope);

            var result = String.Join(" ", karmaEntities.Select(k => k.KarmaKey + ": " + k.KarmaValue));

            return new ActionResultDto(){Data = result};
        }

        private ActionResultDto ChangeKarma(ITransactionScope transactionScope, ActionDto actionDto, int value)
        {
            var keyParam = GetParameter(actionDto, ActionGroup, "Key");
            var key = keyParam.Value;

            var karma = _karmaRepository.GetByKey(transactionScope, key);

            if (karma == null)
            {
                karma = _karmaRepository.Create();
                karma.KarmaKey = key;
                karma.KarmaValue = 0;
            }

            karma.KarmaValue = karma.KarmaValue + value;

            var newValue = karma.KarmaValue;

            if (newValue == 0)
                _karmaRepository.Delete(transactionScope, karma);
            else
                _karmaRepository.Save(transactionScope, karma);

            return new ActionResultDto() {Data = Convert.ToString(newValue)};
        }
    }
}
