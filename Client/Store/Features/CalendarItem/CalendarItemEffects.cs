using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Mono.Client.Store.Features.CalendarItem
{
    public class CalendarItemEffects
    {
        private readonly CalendarHttpService _calendarHttpService;
        private readonly ILogger<CalendarItemEffects> _logger;
        private readonly IState<CalendarState> _state;

        public CalendarItemEffects(CalendarHttpService calendarHttpService, ILogger<CalendarItemEffects> logger, IState<CalendarState> state)
        {
            _calendarHttpService = calendarHttpService;
            _logger = logger;
            _state = state;
        }

        [EffectMethod]
        public async Task HandleAsync(LoadCalendarItemsAction action, IDispatcher dispatcher)
        {
            try
            {
                var calendarItems = await _calendarHttpService.GetCalendarItemsAsync();
                dispatcher.Dispatch(new LoadCalendarItemsSuccessAction(calendarItems));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadCalendarItemsFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(NewCalendarItemAction action, IDispatcher dispatcher)
        {
            try
            {
                var calendarItem = await _calendarHttpService.GetCalendarItemAsync(action.Id);
                if(calendarItem != null)
                {
                    dispatcher.Dispatch(new NewCalendarItemFailureAction($"Resource already exists for {action.Id}"));
                }
                else
                {
                    dispatcher.Dispatch(new NewCalendarItemSuccessAction(action.Id));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new NewCalendarItemFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(LoadCalendarItemDetailAction action, IDispatcher dispatcher)
        {
            try
            {
                var calendarItem = await _calendarHttpService.GetCalendarItemAsync(action.Id);
                dispatcher.Dispatch(new LoadCalendarItemDetailSuccessAction(calendarItem));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadCalendarItemDetailFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(UpdateCalendarItemAction action, IDispatcher dispatcher)
        {
            try
            {
                CalendarItemDto calendarItemDto = new CalendarItemDto();
                calendarItemDto.Id = action.Id;
                calendarItemDto.Title = action.CalendarItemDto.Title;
                calendarItemDto.Description = action.CalendarItemDto.Description;
                calendarItemDto.StartDate = action.CalendarItemDto.StartDate;
                calendarItemDto.EndDate = action.CalendarItemDto.EndDate;

                await _calendarHttpService.Save(calendarItemDto);
                dispatcher.Dispatch(new UpdateCalendarItemSuccessAction(calendarItemDto));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new UpdateCalendarItemFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(CreateCalendarItemAction action, IDispatcher dispatcher)
        {
            try
            {
                CalendarItemDto calendarItemDto = new CalendarItemDto();
                calendarItemDto.Title = action.CalendarItem.Title;
                calendarItemDto.Description = action.CalendarItem.Description;
                calendarItemDto.StartDate = action.CalendarItem.StartDate;
                calendarItemDto.EndDate = action.CalendarItem.EndDate;

                await _calendarHttpService.Save(calendarItemDto);
                dispatcher.Dispatch(new UpdateCalendarItemSuccessAction(calendarItemDto));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new UpdateCalendarItemFailureAction(e.Message));
            }
        }


        [EffectMethod]
        public async Task HandleAsync(DeleteCalendarItemAction action, IDispatcher dispatcher)
        {
            try
            {
                await _calendarHttpService.Delete(action.Id);
                dispatcher.Dispatch(new DeleteCalendarItemSuccessAction(action.Id));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new DeleteCalendarItemFailureAction(e.Message));
            }
        }
    }
}
