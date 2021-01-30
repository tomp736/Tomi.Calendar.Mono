using System;
using System.Collections.Generic;
using Tomi.Calendar.Mono.Client.Store.Features.Shared;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Mono.Client.Store.Features.CalendarItem
{

    public record NewCalendarItemAction
    {
        public NewCalendarItemAction(Guid id) => Id = id;
        public Guid Id { get; }
    }
    public record NewCalendarItemSuccessAction
    {
        public NewCalendarItemSuccessAction(Guid id) => Id = id;
        public Guid Id { get; }
    }
    public record NewCalendarItemFailureAction : FailureAction
    {
        public NewCalendarItemFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record CreateCalendarItemAction
    {
        public CreateCalendarItemAction(CreateOrUpdateCalendarItemDto note) => CalendarItemDto = note;
        public CreateOrUpdateCalendarItemDto CalendarItemDto { get; }
    }
    public record CreateCalendarItemFailureAction : FailureAction
    {
        public CreateCalendarItemFailureAction(string errorMessage) : base(errorMessage) { }
    }
    public record CreateCalendarItemSuccessAction
    {
        public CreateCalendarItemSuccessAction(CalendarItemDto note) => CalendarItem = note;
        public CalendarItemDto CalendarItem { get; }
    }


    public record LoadCalendarItemsAction { }
    public record LoadCalendarItemsSuccessAction
    {
        public LoadCalendarItemsSuccessAction(IEnumerable<CalendarItemDto> calendarItems) => (CalendarItems) = (calendarItems);
        public IEnumerable<CalendarItemDto> CalendarItems { get; }
    }
    public record LoadCalendarItemsFailureAction : FailureAction
    {
        public LoadCalendarItemsFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record LoadCalendarItemDetailAction
    {
        public LoadCalendarItemDetailAction(Guid id) => Id = id;
        public Guid Id { get; set; }
    }
    public record LoadCalendarItemDetailSuccessAction
    {
        public LoadCalendarItemDetailSuccessAction(CalendarItemDto calendarItem) => (CalendarItem) = (calendarItem);
        public CalendarItemDto CalendarItem { get; }
    }
    public record LoadCalendarItemDetailFailureAction : FailureAction
    {
        public LoadCalendarItemDetailFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record UpdateCalendarItemAction
    {
        public UpdateCalendarItemAction(Guid id, CreateOrUpdateCalendarItemDto calendarItemDto) => (Id, CalendarItemDto) = (id, calendarItemDto);
        public Guid Id { get; }
        public CreateOrUpdateCalendarItemDto CalendarItemDto { get; }
    }
    public record UpdateCalendarItemSuccessAction
    {
        public UpdateCalendarItemSuccessAction(CalendarItemDto calendarItem) => (CalendarItem) = (calendarItem);
        public CalendarItemDto CalendarItem { get; }
    }
    public record UpdateCalendarItemFailureAction : FailureAction
    {
        public UpdateCalendarItemFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record DeleteCalendarItemAction
    {
        public DeleteCalendarItemAction(Guid id) => (Id) = (id);
        public Guid Id { get; }
    }
    public record DeleteCalendarItemSuccessAction
    {
        public DeleteCalendarItemSuccessAction(Guid id) => (Id) = (id);
        public Guid Id { get; }
    }
    public record DeleteCalendarItemFailureAction : FailureAction
    {
        public DeleteCalendarItemFailureAction(string errorMessage) : base(errorMessage) { }
    }
}
