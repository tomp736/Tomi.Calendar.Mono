﻿using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Store.Features.Tag
{
    public class TagEffects
    {
        private readonly CalendarHttpService _calendarHttpService;
        private readonly ILogger<TagEffects> _logger;
        private readonly IState<CalendarState> _state;

        public TagEffects(CalendarHttpService calendarHttpService, ILogger<TagEffects> logger, IState<CalendarState> state)
        {
            _calendarHttpService = calendarHttpService;
            _logger = logger;
            _state = state;
        }

        [EffectMethod]
        public async Task HandleAsync(LoadTagsAction action, IDispatcher dispatcher)
        {
            try
            {
                var tags = await _calendarHttpService.GetTagsAsync();
                dispatcher.Dispatch(new LoadTagsSuccessAction(tags));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadTagsFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(LoadTagDetailAction action, IDispatcher dispatcher)
        {
            try
            {
                var tag = await _calendarHttpService.GetTagAsync(action.Id);
                dispatcher.Dispatch(new LoadTagDetailSuccessAction(tag));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadTagDetailFailureAction(e.Message));
            }
        }
    }
}
