// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Linq;
using System.Web.Mvc;
using jarvis.server.model;
using jarvis.server.web.Areas.Events.Models;

namespace jarvis.server.web.Areas.Events.Controllers
{
    public class TriggeredEventController : Controller
    {
        private readonly IEventLogic _eventLogic;

        public TriggeredEventController(IEventLogic eventLogic)
        {
            _eventLogic = eventLogic;
        }

        //
        // GET: /Events/TriggeredEvent/
        public ActionResult Index()
        {
            return View(_eventLogic.GetLastEvents().Select(e => new TriggeredEvent
                                                                    {
                                                                        EventGroupType = e.EventGroupType,
                                                                        EventType = e.EventType,
                                                                        TriggeredDate = e.TriggeredDate,
                                                                        Data = e.Data
                                                                    }));
        }
    }
}