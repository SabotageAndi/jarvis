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

            _karmaRepository = new KarmaRepository(Kernel().Get<ITransactionProvider>());
        }

        public override bool CanExecute(ActionDto actionDto)
        {
            return actionDto.ActionGroup == "Karma";
        }

        public override ActionResultDto Execute(ActionDto actionDto)
        {
            switch (actionDto.Action)
            {
                case "Increase":
                    return ChangeKarma(actionDto, 1);
                case "Decrease":
                    return ChangeKarma(actionDto, -1);
                case "GetKarma":
                    return GetKarma();
                case "Stats":
                    return GetStats();

            }

            throw new ArgumentOutOfRangeException();
        }

        private ActionResultDto GetKarma()
        {
            var karmaEntities = _karmaRepository.GetAll();

            Decimal stats = 0;

            if (karmaEntities.Any())
            {
                stats = Decimal.Divide(karmaEntities.Sum(k => k.KarmaValue), karmaEntities.Count());

            }

            return new ActionResultDto() {Data = stats.ToString()};
        }

        private ActionResultDto GetStats()
        {
            var karmaEntities = _karmaRepository.GetAll();

            var result = String.Join(" ", karmaEntities.Select(k => k.KarmaKey + ": " + k.KarmaValue));

            return new ActionResultDto(){Data = result};
        }

        private ActionResultDto ChangeKarma(ActionDto actionDto, int value)
        {
            var keyParam = GetParameter(actionDto, ActionGroup, "Key");
            var key = keyParam.Value;

            var karma = _karmaRepository.GetByKey(key);

            if (karma == null)
            {
                karma = _karmaRepository.Create();
                karma.KarmaKey = key;
                karma.KarmaValue = 0;
            }

            karma.KarmaValue = karma.KarmaValue + value;

            var newValue = karma.KarmaValue;

            if (newValue == 0)
                _karmaRepository.Delete(karma);
            else
                _karmaRepository.Save(karma);

            return new ActionResultDto() {Data = Convert.ToString(newValue)};
        }
    }
}
