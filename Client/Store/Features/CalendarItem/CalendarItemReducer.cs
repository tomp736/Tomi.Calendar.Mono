using Fluxor;
using System.Collections.Generic;
using System.Linq;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Store.Features.CalendarItem
{
    public static class CalendarItemReducer
    {
        #region New

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, NewCalendarItemSuccessAction action) =>
            state with
            {
                CurrentCalendarItem = new CalendarItemDto() { Id = action.Id }
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, NewCalendarItemFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Create

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, CreateCalendarItemAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, CreateCalendarItemSuccessAction action)
        {
            // Grab a reference to the current CalendarItem list, or initialize one if we do not currently have any loaded
            var currentCalendarItems = state.CalendarItems is null ?
                new List<CalendarItemDto>() :
                state.CalendarItems.ToList();
            // Add the newly created CalendarItem to our list and sort by ID
            currentCalendarItems.Add(action.CalendarItem);
            currentCalendarItems = currentCalendarItems
                .OrderBy(t => t.Id)
                .ToList();

            return state with
            {
                CurrentErrorMessage = null,
                CalendarItems = currentCalendarItems
            };
        }

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, CreateCalendarItemFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Load

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, LoadCalendarItemsAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, LoadCalendarItemsSuccessAction action) =>
            state with
            {
                CurrentErrorMessage = null,
                CalendarItems = action.CalendarItems
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, LoadCalendarItemsFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region LoadDetail

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, LoadCalendarItemDetailAction _) =>
            state with
            {
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, LoadCalendarItemDetailSuccessAction action) =>
            state with
            {

                CurrentCalendarItem = action.CalendarItem,
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, LoadCalendarItemDetailFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Update

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, UpdateCalendarItemAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, UpdateCalendarItemSuccessAction action)
        {
            // If the current CalendarItems list is null, set the state with a new list containing the updated CalendarItem
            if (state.CalendarItems is null)
            {
                return state with
                {

                    CurrentErrorMessage = null,
                    CalendarItems = new List<CalendarItemDto> { action.CalendarItem }
                };
            }

            // Rather than mutating in place, let's construct a new list and add our updated item
            var updatedList = state.CalendarItems
                .Where(t => t.Id != action.CalendarItem.Id)
                .ToList();

            // Add the CalendarItem and sort the list
            updatedList.Add(action.CalendarItem);
            updatedList = updatedList
                .OrderBy(t => t.Id)
                .ToList();

            return state with
            {

                CurrentErrorMessage = null,
                CalendarItems = updatedList
            };
        }

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, UpdateCalendarItemFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Delete

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, DeleteCalendarItemAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, DeleteCalendarItemSuccessAction action)
        {
            // Return the default state if no list of CalendarItems is found
            if (state.CalendarItems is null)
            {
                return state with
                {
                    CurrentErrorMessage = null
                };
            }

            // Create a new list with all CalendarItem items excluding the CalendarItem with the deleted ID
            var updatedCalendarItems = state.CalendarItems.Where(t => t.Id != action.Id);
            return state with
            {

                CurrentErrorMessage = null,
                CalendarItems = updatedCalendarItems
            };
        }
        [ReducerMethod]
        public static CalendarState Reduce(CalendarState state, DeleteCalendarItemFailureAction action) =>
            state with
            {

                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion
    }
}
