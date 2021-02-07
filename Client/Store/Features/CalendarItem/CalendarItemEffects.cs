﻿using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Proto.CodeFirst;

namespace Tomi.Calendar.Mono.Client.Store.Features.CalendarItem
{
    public class CalendarItemEffects
    {
        private readonly CalendarHttpService _calendarHttpService;
        private readonly GrpcCalendarItemServiceClient _grpcCalendarItemService;
        private readonly ILogger<CalendarItemEffects> _logger;
        private readonly IState<CalendarState> _state;

        public CalendarItemEffects(
            CalendarHttpService calendarHttpService,
            GrpcCalendarItemServiceClient grpcCalendarItemServiceClient,
            ILogger<CalendarItemEffects> logger,
            IState<CalendarState> state)
        {
            _calendarHttpService = calendarHttpService;
            _grpcCalendarItemService = grpcCalendarItemServiceClient;
            _logger = logger;
            _state = state;
        }

        [EffectMethod]
        public async Task HandleAsync(LoadCalendarItemsAction action, IDispatcher dispatcher)
        {
            try
            {
                GetCalendarItemsResponse calendarItemsResponse = await _grpcCalendarItemService.GetCalendarItems(new GetCalendarItemsRequest()
                {
                });
                dispatcher.Dispatch(new LoadCalendarItemsSuccessAction(calendarItemsResponse.CalendarItems));
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
                GetCalendarItemsResponse calendarItemsResponse = await _grpcCalendarItemService.GetCalendarItems(new GetCalendarItemsRequest()
                {
                    CalendarItemIds = new Guid[] { action.Id }
                });
                if (calendarItemsResponse.CalendarItems.Any())
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
                GetCalendarItemsResponse calendarItemsResponse = await _grpcCalendarItemService.GetCalendarItems(new GetCalendarItemsRequest()
                {
                    CalendarItemIds = new Guid[] { action.Id }
                });
                CalendarItemDto calendarItemDto = calendarItemsResponse.CalendarItems.FirstOrDefault();
                if (calendarItemDto != null)
                {
                    dispatcher.Dispatch(new LoadCalendarItemDetailSuccessAction(calendarItemDto));
                }
                else
                {
                    dispatcher.Dispatch(new LoadCalendarItemDetailFailureAction("Resource does not exist"));
                }
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
                calendarItemDto.StartTime = action.CalendarItemDto.StartTime;
                calendarItemDto.EndTime = action.CalendarItemDto.EndTime;

                SaveCalendarItemsResponse calendarItemsResponse = await _grpcCalendarItemService.SaveCalendarItems(new SaveCalendarItemsRequest()
                {
                    CalendarItems = new CalendarItemDto[] { calendarItemDto }
                });
                if (calendarItemsResponse.CalendarItems.Any())
                {
                    dispatcher.Dispatch(new UpdateCalendarItemSuccessAction(calendarItemsResponse.CalendarItems.FirstOrDefault()));
                }
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
                calendarItemDto.Title = action.CalendarItemDto.Title;
                calendarItemDto.Description = action.CalendarItemDto.Description;
                calendarItemDto.StartDate = action.CalendarItemDto.StartDate;
                calendarItemDto.EndDate = action.CalendarItemDto.EndDate;
                calendarItemDto.StartTime = action.CalendarItemDto.StartTime;
                calendarItemDto.EndTime = action.CalendarItemDto.EndTime;

                SaveCalendarItemsResponse calendarItemsResponse = await _grpcCalendarItemService.SaveCalendarItems(new SaveCalendarItemsRequest()
                {
                    CalendarItems = new CalendarItemDto[] { calendarItemDto }
                });
                if (calendarItemsResponse.CalendarItems.Any())
                {
                    dispatcher.Dispatch(new UpdateCalendarItemSuccessAction(calendarItemsResponse.CalendarItems.FirstOrDefault()));
                }
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
                DeleteCalendarItemsResponse calendarItemsResponse = await _grpcCalendarItemService.DeleteCalendarItems(new DeleteCalendarItemsRequest()
                {
                    CalendarItemIds = new Guid[] { action.Id }
                });
                dispatcher.Dispatch(new DeleteCalendarItemSuccessAction(action.Id));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new DeleteCalendarItemFailureAction(e.Message));
            }
        }
    }
}
